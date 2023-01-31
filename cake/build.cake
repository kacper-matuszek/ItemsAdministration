#tool "dotnet:?package=GitVersion.Tool&version=5.10.3"
#addin nuget:?package=Cake.FileHelpers&version=5.0.0

#load "version.cake"

var projectName = "ItemsAdministration";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var publishDirectory = Directory("..\\publish");
var solutionFile = File($@"..\{projectName}.sln");
var webHostFile = File($@"..\src\WebHost\{projectName}.WebHost.csproj");

Task("Clean").Does(() => CleanDirectory(publishDirectory));

Task("Restore")
    .IsDependentOn("DirectoryBuildProps")
    .Does(() => {
        DotNetRestore(solutionFile, new DotNetCoreRestoreSettings {
            NoCache = true
        });
    });

Task("Publish")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetPublish(webHostFile, new DotNetPublishSettings{
            Configuration = configuration,
            NoRestore = true,
            OutputDirectory = publishDirectory
        });
    });

Task("Default")
    .IsDependentOn("Publish");

RunTarget(target);