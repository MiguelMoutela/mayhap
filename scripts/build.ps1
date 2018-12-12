if ($env:APPVEYOR_REPO_TAG -eq $true -and (-not $env:APPVEYOR_REPO_TAG_NAME.StartsWith('release'))) {
    $env:Prerelease = $env:APPVEYOR_REPO_TAG_NAME + "." + $env:APPVEYOR_BUILD_NUMBER
}

Write-Output $env:Prerelease

dotnet build -c Release
dotnet pack --no-build -c Release src/Mayhap/Mayhap.csproj