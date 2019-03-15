#addin nuget:?package=Cake.DocFx&version=0.12.0
#tool nuget:?package=docfx.console&version=2.40.12

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

var slnPath = MakeAbsolute(FilePath.FromString("./Mayhap.sln"));
var mayhapCsprojPath = MakeAbsolute(FilePath.FromString("./src/Mayhap/Mayhap.csproj"));
var artifactsPath = MakeAbsolute(DirectoryPath.FromString("./artifacts"));
var docsPath = MakeAbsolute(DirectoryPath.FromString("./docs"));
var packageArtifact = System.IO.Path.Combine(artifactsPath.FullPath, "Mayhap*.nuspec");

ICollection<string> Props(params string[] properties)
    => properties;

Task("Clean")
    .Does(() => 
    {
        var cleanSettings = new DotNetCoreCleanSettings
        {
            Configuration = configuration
        };
        DotNetCoreClean(slnPath.FullPath, cleanSettings);
        
        var deleteSettings = new DeleteDirectorySettings
        {
            Force = true,
            Recursive = true
        };

        if(DirectoryExists(artifactsPath)) DeleteDirectory(artifactsPath, deleteSettings);

        var docsOutput = System.IO.Path.Combine(docsPath.FullPath, "_site");
        if(DirectoryExists(docsOutput)) DeleteDirectory(docsOutput, deleteSettings);
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
            DotNetCoreBuild(slnPath.FullPath, settings);
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
            DotNetCorePack(mayhapCsprojPath.FullPath, settings);
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

Task("GenerateDocs")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() =>
    {
        // var docFxTemplate = System.IO.Path.Combine(docsPath.FullPath, "docfx.json");
        // var tocFxTemplate = System.IO.Path.Combine(docsPath.FullPath, "toc.template.yml");
        // var docFx = System.IO.Path.Combine(docsPath.FullPath, $"docfx.{nugetVersion}.json");
        // var toc = System.IO.Path.Combine(docsPath.FullPath, "toc.yml");
        
        // TransformTextFile(docFxTemplate, "{{", "}}")
            // .WithToken("reference", nugetVersion)
            // .Save(docFx);
        // TransformTextFile(tocFxTemplate, "{{", "}}")
            // .WithToken("reference", nugetVersion)
            // .Save(toc);

        var docFx = System.IO.Path.Combine(docsPath.FullPath, "docfx.json");
        DocFxBuild(docFx);

        if(!DirectoryExists(artifactsPath)) CreateDirectory(artifactsPath);
        Zip("./docs/_site",
            System.IO.Path.Combine(artifactsPath.FullPath, $"Mayhap.{nugetVersion}.docs.zip"));
        // DeleteFile(docFx);
    });

Task("PublishDocs")
    .WithCriteria(() => shouldPublish)
    .IsDependentOn("Version")
    .IsDependentOn("GenerateDocs");

Task("Build")
    .IsDependentOn("PublishDocs")
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