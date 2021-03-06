# This post build process will update any local projects that include a reference to this StoicDreams.TestFramework nuget package.

$rgxTargetGetVersion = '<Version>(.+)</Version>'
$rgxTargetXML = '<PackageReference Include="StoicDreams.TestFramework" Version="([0-9\.]+)" />'
Clear-Host;

Get-ChildItem -Path .\ -Filter *TestFramework.csproj -Recurse -File | ForEach-Object {
	$result = Select-String -Path $_.FullName -Pattern $rgxTargetGetVersion
	if($result.Matches.Count -gt 0) {
		$version = $result.Matches[0].Groups[1].Value
	}
}

function UpdateProjectVersion {
	Param (
		[string] $projectPath,
		[string] $version
	)	
	
	$rgxTargetXML = '<PackageReference Include="StoicDreams.TestFramework" Version="([0-9\.]+)" />'
	$newXML = '<PackageReference Include="StoicDreams.TestFramework" Version="'+$version+'" />'

	if(!(Test-Path -Path $projectPath)) {
		Write-Host "Not found - $projectPath" -BackgroundColor Red -ForegroundColor White
		return;
	}
	$content = Get-Content -Path $projectPath
	$oldMatch = $content -match $rgxTargetXML
	if($oldMatch.Length -eq 0) {
		#Write-Host "Doesn't use TestFramework - $projectPath"
		return;
	}
	$matches = $content -match $newXML
	if($matches.Length -eq 1) {
		Write-Host "Already up to date - $projectPath" -ForegroundColor Cyan
		return;
	}
	$newContent = $content -replace $rgxTargetXML, $newXML
	if ($projectPath.Contains("TestFramework.Blazor")) {
		Write-Host "Special content update $projectPath"
		$newContent = $newContent -replace '<Version>([0-9\.]+)</Version>', ('<Version>'+$version+'</Version>')
	}
	$newContent | Set-Content -Path $projectPath
	Write-Host "Updated   - $projectPath" -ForegroundColor Green
}

if($version -ne $null) {
	Write-Host Found Version: $version -ForegroundColor Green
	$rootpath = Get-Location
	$rootpath = $rootpath.ToString().ToLower()
	Write-Host Path: $rootpath
	while($rootpath.Contains('testframework')) {
		cd ..
		$rootpath = Get-Location
		$rootpath = $rootpath.ToString().ToLower()
		Write-Host $rootpath
	}
	$newXML = '<PackageReference Include="StoicDreams.TestFramework" Version="'+$version+'" />'
	Get-ChildItem -Path .\ -Filter *.csproj -Recurse -File | ForEach-Object {
		UpdateProjectVersion $_.FullName $version
	}
	Get-ChildItem -Path .\ -Filter *README.md -Recurse -File | ForEach-Object {
		UpdateProjectVersion $_.FullName $version
	}
	Write-Host "Finished updates to Version: $version"
} else {
	Write-Host Current version was not found -ForegroundColor Red
}
