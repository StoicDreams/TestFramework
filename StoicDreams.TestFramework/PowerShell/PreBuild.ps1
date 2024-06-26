﻿# This pre-build process will update the current version number within this solution.

Clear-Host;

$start_loc = Get-Location;
Set-Location $PSScriptRoot;
Set-Location ..

if (!(Test-Path './StoicDreams.TestFramework.csproj')) {
	throw "This script is expected to be run from the root of the StoicDreams.TestFramework project."
}

$rgxTargetGetVersion = '<Version>([0-9]+)\.([0-9]+)\.([0-9]+)</Version>'

$alphaversion = 1
$betaversion = 0
$rcversion = 0

Get-ChildItem -Path .\ -Filter *TestFramework.csproj -Recurse -File | ForEach-Object {
	Write-Host $_
	$result = Select-String -Path $_.FullName -Pattern $rgxTargetGetVersion
	if ($result.Matches.Count -gt 0) {
		$alphaversion = [int]$result.Matches[0].Groups[1].Value
		$betaversion = [int]$result.Matches[0].Groups[2].Value
		$rcversion = [int]$result.Matches[0].Groups[3].Value
	}
}

Write-Host "Release Version: [$($alphaversion)]_[$($betaversion)]_[$($rcversion)]";
$rcversion = $rcversion + 1;

$version = "$alphaversion.$betaversion.$rcversion";
Write-Host "New Version: $($version)";

function UpdateProjectVersion {
	Param (
		[string] $projectPath,
		[string] $version
	)

	$rgxTargetXML = '<Version>([0-9\.]+)</Version>'
	$newXML = '<Version>' + $version + '</Version>'

	if (!(Test-Path -Path $projectPath)) {
		Write-Host "Not found - $projectPath" -BackgroundColor Red -ForegroundColor White
		return;
	}
	$content = Get-Content -Path $projectPath
	$oldMatch = $content -match $rgxTargetXML
	if ($oldMatch.Length -eq 0) {
		#Write-Host "Doesn't use TestFramework - $projectPath"
		return;
	}
	$matches = $content -match $newXML
	if ($matches.Length -eq 1) {
		Write-Host "Already up to date - $projectPath" -ForegroundColor Cyan
		return;
	}
	$newContent = $content -replace $rgxTargetXML, $newXML
	$newContent | Set-Content -Path $projectPath
	Write-Host "Updated   - $projectPath" -ForegroundColor Green
}
Write-Host Version: $version
if ($version -ne $null) {
	$rootpath = Get-Location
	$rootpath = $rootpath.ToString().ToLower()

	while ($rootpath.Contains('testframework')) {
		cd ..
		$rootpath = Get-Location
		$rootpath = $rootpath.ToString().ToLower()
		Write-Host $rootpath
	}

	Get-ChildItem -Path .\ -Filter *TestFramework.csproj -Recurse -File | ForEach-Object {
		UpdateProjectVersion $_.FullName $version
	}
}

Set-Location $start_loc;
