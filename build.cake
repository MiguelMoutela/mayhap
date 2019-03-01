var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

string nugetVersion = null;
string informationalVersion = null;
string version = null;
var slnPath = System.IO.Path.GetFullPath("./Mayhap.sln");
var mayhapCsprojPath = System.IO.Path.GetFullPath("./src/Mayhap/Mayhap.csproj");
var artifactsPath = System.IO.Path.GetFullPath("./artifacts");

Task("Clean")
    .Does(() => 
    {
        var settings = new DotNetCoreCleanSettings() { Configuration = configuration };
        DotNetCoreClean(slnPath, settings);
        if(DirectoryExists(artifactsPath)) DeleteDirectory(artifactsPath, true);
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

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() => 
        {
            var settings = new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                MSBuildSettings = new DotNetCoreMSBuildSettings() 
            };
            settings.MSBuildSettings.Properties["Version"] = new [] {version};
            settings.MSBuildSettings.Properties["FileVersion"] = new [] {version};
            settings.MSBuildSettings.Properties["InformationalVersion"] = new [] {version};
            settings.MSBuildSettings.Properties["PackageVersion"] = new [] {nugetVersion};
            DotNetCoreBuild(slnPath, settings);
        });

Task("Pack")
    .IsDependentOn("Build")
    .Does(() => 
        {
            var settings = new DotNetCorePackSettings()
            {
                NoBuild = true,
                Configuration = configuration,
                OutputDirectory = artifactsPath,
                MSBuildSettings = new DotNetCoreMSBuildSettings()
            };
            settings.MSBuildSettings.Properties["PackageVersion"] = new [] {nugetVersion};
            DotNetCorePack(mayhapCsprojPath, settings);
        });

Task("Test")
    .IsDependentOn("Build")
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

RunTarget(target);