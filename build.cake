var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var shouldPublish = 
        HasEnvironmentVariable("APP_VEYOR_REPO_BRANCH")
            && HasEnvironmentVariable("NUGET_API_KEY")
            && EnvironmentVariable("APP_VEYOR_REPO_BRANCH")
                .Equals("master", StringComparison.InvariantCulture);
var nugetApiKey = 
    HasEnvironmentVariable("NUGET_API_KEY") 
        ? EnvironmentVariable("NUGET_API_KEY") 
        : null;

if(!HasEnvironmentVariable("NUGET_API_KEY"))
{
    Warning("Nuget API key not specified");
}

string nugetVersion = null;
string informationalVersion = null;
string version = null;
var slnPath = System.IO.Path.GetFullPath("./Mayhap.sln");
var mayhapCsprojPath = System.IO.Path.GetFullPath("./src/Mayhap/Mayhap.csproj");
var artifactsPath = System.IO.Path.GetFullPath("./artifacts");
var packageArtifact = System.IO.Path.Combine(artifactsPath, "Mayhap*.nuspec");

ICollection<string> Props(params string[] properties)
    => properties;

Task("Clean")
    .Does(() => 
    {
        var cleanSettings = new DotNetCoreCleanSettings
        {
            Configuration = configuration
        };
        DotNetCoreClean(slnPath, cleanSettings);
        
        var deleteSettings = new DeleteDirectorySettings
        {
            Force = true,
            Recursive = true
        };
        if(DirectoryExists(artifactsPath)) DeleteDirectory(artifactsPath, deleteSettings);
    });

Task("Version")
    .Does(() => 
    {
        var gitVersion = GitVersion();
        nugetVersion = gitVersion.NuGetVersionV2;
        informationalVersion = gitVersion.InformationalVersion;
        version = gitVersion.AssemblySemVer;
        Information("NuGetVersionV2: {0}", nugetVersion);
        Information("InformationalVersion: {0}", informationalVersion);
        Information("SemVer: {0}", version);
    });

Task("Compile")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() => 
        {
            var settings = new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                MSBuildSettings = new DotNetCoreMSBuildSettings() 
            };
            settings.MSBuildSettings.Properties["Version"] = Props(version);
            settings.MSBuildSettings.Properties["FileVersion"] = Props(version);
            settings.MSBuildSettings.Properties["InformationalVersion"] = Props(version);
            settings.MSBuildSettings.Properties["PackageVersion"] = Props(nugetVersion);
            DotNetCoreBuild(slnPath, settings);
        });

Task("Test")
    .IsDependentOn("Compile")
    .DoesForEach(GetFiles("tests/**/*.csproj"), 
        f => 
        {
            var settings = new DotNetCoreTestSettings()
            {
                Configuration = configuration,
                NoBuild = true,
                NoRestore = true
            };
            DotNetCoreTest(f.FullPath, settings);
        });

Task("Pack")
    .IsDependentOn("Compile")
    .Does(() => 
        {
            var settings = new DotNetCorePackSettings()
            {
                NoBuild = true,
                Configuration = configuration,
                OutputDirectory = artifactsPath,
                MSBuildSettings = new DotNetCoreMSBuildSettings()
            };
            settings.MSBuildSettings.Properties["PackageVersion"] = Props(nugetVersion);
            DotNetCorePack(mayhapCsprojPath, settings);
        });

Task("Publish")
    .WithCriteria(shouldPublish)
    .IsDependentOn("Test")
    .IsDependentOn("Pack")
    .Does(() =>
    {
        var settings = new DotNetCoreNuGetPushSettings
        {
            ApiKey = nugetApiKey,
            IgnoreSymbols = false,
            ForceEnglishOutput = true
        };

        DotNetCoreNuGetPush(packageArtifact);
    });

Task("Build")
    .IsDependentOn("Publish");

Task("List")
    .Does(() =>
    {
        foreach(var task in Tasks)
        {
            var dependencies = 
                string.Join(", ", task.Dependencies.Select(t => $"{t.Name}[Required={t.Required}]"));
            if(task.Dependencies.Any())
            {
                dependencies = $"({dependencies})";
            }
            Information($"{task.Name}{dependencies}");
        }
    });

RunTarget(target);