#addin nuget:?package=Cake.Git&version=0.19.0
// #addin nuget:?package=Cake.DocFx&version=0.12.0
// #tool nuget:?package=docfx.console&version=2.40.12

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var shouldPublish = 
        HasEnvironmentVariable("APP_VEYOR_REPO_BRANCH")
            && HasEnvironmentVariable("NUGET_API_KEY")
            && HasEnvironmentVariable("GITHUB_ACCESS_TOKEN")
            && HasEnvironmentVariable("GITHUB_REPO")
            && EnvironmentVariable("APP_VEYOR_REPO_BRANCH")
                .Equals("master", StringComparison.InvariantCulture);
var nugetApiKey = 
    HasEnvironmentVariable("NUGET_API_KEY") 
        ? EnvironmentVariable("NUGET_API_KEY") 
        : null;

var githubAccessToken = 
    HasEnvironmentVariable("GITHUB_ACCESS_TOKEN") 
        ? EnvironmentVariable("GITHUB_ACCESS_TOKEN") 
        : null;

var githubRepo = 
    HasEnvironmentVariable("GITHUB_REPO") 
        ? EnvironmentVariable("GITHUB_REPO") 
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
var docsTempPath = MakeAbsolute(DirectoryPath.FromString("./docs.tmp"));
var packageArtifact = artifactsPath.CombineWithFilePath("Mayhap*.nuspec");

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

        var docsOutput = docsPath.Combine("_site");
        if(DirectoryExists(docsOutput)) DeleteDirectory(docsOutput, deleteSettings);
        if(DirectoryExists(docsTempPath)) DeleteDirectory(docsTempPath, deleteSettings);
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

        DotNetCoreNuGetPush(packageArtifact.FullPath);
    });

Task("GenerateDocs")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() =>
    {
        var docFxTemplate = docsTempPath.CombineWithFilePath("docfx.template.json");
        var tocFxTemplate = docsTempPath.CombineWithFilePath("toc.template.yml");
        var docFx = docsTempPath.CombineWithFilePath("docfx.json");
        var toc = docsTempPath.CombineWithFilePath("toc.yml");
        var tempOutput = docsTempPath.Combine("_site");

        Information("Making a copy of the docs generation config...");
        CopyDirectory(docsPath, docsTempPath);

        Information("Preparing a versioned docs structure...");
        CopyDirectory(docsTempPath.Combine("reference"), docsTempPath.Combine(nugetVersion));
        
        TransformTextFile(docFxTemplate, "{{", "}}")
            .WithToken("reference", nugetVersion)
            .WithToken("sourceRoot", MakeAbsolute(Directory("./src")).FullPath)
            .Save(docFx);
        TransformTextFile(tocFxTemplate, "{{", "}}")
            .WithToken("reference", nugetVersion)
            .Save(toc);

        Information("Building the docs...");
        StartProcess("docfx", docFx.FullPath);

        if(!DirectoryExists(artifactsPath)) CreateDirectory(artifactsPath);
        
        Information("Making a docs artifact...");
        Zip(tempOutput,
            System.IO.Path.Combine(artifactsPath.FullPath, $"Mayhap.{nugetVersion}.docs.zip"));
    });

Task("PublishDocs")
    // .WithCriteria(() => shouldPublish)
    .IsDependentOn("Version")
    .IsDependentOn("GenerateDocs")
    .Does(() =>
        {
            githubRepo = TransformText(githubRepo, "{{", "}}")
                .WithToken("token", githubAccessToken)
                .ToString();
            var ghPagesPublishPath = DirectoryPath.FromString("../ghpages");

            var articlesPath = DirectoryPath.FromString("../ghpages/articles");
            var tempOutput = docsTempPath.Combine("_site");

            Information("Cloning the docs branch...");
            var cloneSettings = new GitCloneSettings()
            {
                BranchName = "gh-pages",
                Checkout = true,
            };
            GitClone(githubRepo, ghPagesPublishPath, cloneSettings);


            if(DirectoryExists(articlesPath)) DeleteDirectory(
                articlesPath,
                new DeleteDirectorySettings { Force = true, Recursive = true });

            CopyFiles("./docs.tmp/_site/**/*", "../ghpages", true);

            GitAddAll(ghPagesPublishPath);
            if(GitHasStagedChanges(ghPagesPublishPath))
            {
                Information("Publishing the docs...");
                GitCommit(ghPagesPublishPath, "AppVeyor", "appveyor@mayhap", $"Docs update {nugetVersion}");
                GitPush(ghPagesPublishPath);
            }
            else
            {
                Information("No changes to publish");
            }
        });

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