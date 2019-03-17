#addin nuget:?package=Cake.Git&version=0.19.0

// primary parameters
var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var shouldPublish = 
        HasEnvironmentVariable("APP_VEYOR_REPO_BRANCH")
            && HasEnvironmentVariable("NUGET_API_KEY")
            && HasEnvironmentVariable("GITHUB_ACCESS_TOKEN")
            && HasEnvironmentVariable("GITHUB_REPO")
            && EnvironmentVariable("APP_VEYOR_REPO_BRANCH")
                .Equals("master", StringComparison.InvariantCulture);
var shouldPublishDocs = false;

// nuget parameters
var nugetApiKey = 
    HasEnvironmentVariable("NUGET_API_KEY") 
        ? EnvironmentVariable("NUGET_API_KEY") 
        : null;

if(!HasEnvironmentVariable("NUGET_API_KEY"))
{
    Warning("Nuget API key not specified");
}

// github parameters
var githubAccessToken = 
    HasEnvironmentVariable("GITHUB_ACCESS_TOKEN") 
        ? EnvironmentVariable("GITHUB_ACCESS_TOKEN") 
        : null;

var githubRepo = 
    HasEnvironmentVariable("GITHUB_REPO") 
        ? EnvironmentVariable("GITHUB_REPO") 
        : null;

var commit =
    HasEnvironmentVariable("APPVEYOR_REPO_COMMIT")
        ? EnvironmentVariable("APPVEYOR_REPO_COMMIT")
        : null;

if(!HasEnvironmentVariable("GITHUB_ACCESS_TOKEN") || !HasEnvironmentVariable("GITHUB_REPO"))
{
    Warning("GitHub path and/or PAT not specified");
}

// version paramteres
string nugetVersion = null;
string informationalVersion = null;
string version = null;
string docsVersion = null;

// paths
var slnPath = MakeAbsolute(FilePath.FromString("./Mayhap.sln"));
var mayhapCsprojPath = MakeAbsolute(FilePath.FromString("./src/Mayhap/Mayhap.csproj"));
var artifactsPath = MakeAbsolute(DirectoryPath.FromString("./artifacts"));
var packageArtifact = artifactsPath.CombineWithFilePath("Mayhap*.nuspec");
var docsPath = MakeAbsolute(DirectoryPath.FromString("./docs"));
var docsTempPath = MakeAbsolute(DirectoryPath.FromString("./docs.tmp"));
var docFxTemplate = docsTempPath.CombineWithFilePath("docfx.template.json");
var tocFxTemplate = docsTempPath.CombineWithFilePath("toc.template.yml");
var docFx = docsTempPath.CombineWithFilePath("docfx.json");
var toc = docsTempPath.CombineWithFilePath("toc.yml");
var docsTempOutput = docsTempPath.Combine("_site");
var ghPagesPublishPath = DirectoryPath.FromString("../ghpages");
var articlesPath = DirectoryPath.FromString("../ghpages/articles");
var ghTagPublish = DirectoryPath.FromString("../tagPublish");

ICollection<string> Props(params string[] properties)
    => properties;

void Clone(string branch, string outputPath)
{
    githubRepo = TransformText(githubRepo, "{{", "}}")
                .WithToken("token", githubAccessToken)
                .ToString();

    var cloneSettings = new GitCloneSettings()
    {
        BranchName = branch,
        Checkout = true,
    };
    GitClone(githubRepo, outputPath, cloneSettings);
}

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
        var v = new Version(version);
        docsVersion = $"{v.Major}.{v.Minor}";
        shouldPublishDocs = shouldPublish && v.Build == 0;
        Information("NuGetVersionV2: {0}", nugetVersion);
        Information("InformationalVersion: {0}", informationalVersion);
        Information("SemVer: {0}", version);
        Information("DocsVersion: {0}", docsVersion);
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

        
        var tagName = $"v{nugetVersion}";
        Clone("master", ghTagPublish.FullPath);
        GitCheckout(ghTagPublish, commit);
        GitTag(ghTagPublish, tagName);
        GitPushRef(ghTagPublish, "origin", tagName);
    });

Task("GenerateDocs")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() =>
    {
        Information("Making a copy of the docs generation config...");
        CopyDirectory(docsPath, docsTempPath);

        Information("Preparing a versioned docs structure...");
        CopyDirectory(docsTempPath.Combine("reference"), docsTempPath.Combine(docsVersion));
        
        TransformTextFile(docFxTemplate, "{{", "}}")
            .WithToken("reference", docsVersion)
            .WithToken("sourceRoot", MakeAbsolute(Directory("./src")).FullPath)
            .Save(docFx);
        TransformTextFile(tocFxTemplate, "{{", "}}")
            .WithToken("reference", docsVersion)
            .Save(toc);

        Information("Building the docs...");
        StartProcess("docfx", docFx.FullPath);

        if(!DirectoryExists(artifactsPath)) CreateDirectory(artifactsPath);
        
        Information("Making a docs artifact...");
        Zip(docsTempOutput,
            artifactsPath.CombineWithFilePath($"Mayhap.{docsVersion}.docs.zip"));
    });

Task("PublishDocs")
    .WithCriteria(shouldPublishDocs)
    .IsDependentOn("GenerateDocs")
    .Does(() =>
        {
            Clone("gh-pages", ghPagesPublishPath.FullPath);
            if(DirectoryExists(articlesPath)) DeleteDirectory(
                articlesPath,
                new DeleteDirectorySettings { Force = true, Recursive = true });

            CopyFiles(docsTempOutput.CombineWithFilePath("**/*").FullPath, ghPagesPublishPath, true);

            GitAddAll(ghPagesPublishPath);
            if(GitHasStagedChanges(ghPagesPublishPath))
            {
                Information("Publishing the docs...");
                GitCommit(ghPagesPublishPath, "AppVeyor", "appveyor@mayhap", $"Docs update: {docsVersion}");
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