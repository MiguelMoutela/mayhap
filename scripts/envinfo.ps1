Get-ChildItem env: | Where-Object { $_.Name.StartsWith("APPVEYOR") -or $_.Name.StartsWith("CI") }
dotnet --version