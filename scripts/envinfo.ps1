if($env:SUPPRESS_ENV_INFO -ne $true) {
	Get-ChildItem env: | Where-Object { $_.Name.StartsWith("APPVEYOR") -or $_.Name.StartsWith("CI") }
	Write-Output "dotnet --version"
	dotnet --version
}
