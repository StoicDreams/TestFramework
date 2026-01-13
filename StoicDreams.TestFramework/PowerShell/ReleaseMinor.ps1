<#
.SYNOPSIS
    Automated Release Workflow
.DESCRIPTION
    1. Runs Unit Tests (Stops on failure)
    2. Runs PreBuild.ps1
    3. Builds the Solution (Release Configuration)
    4. Runs PostBuild.ps1
#>

Clear-Host
Set-Location $PSScriptRoot
Set-Location ../../
$ErrorActionPreference = "Stop"
$SolutionPath = (Get-ChildItem -Filter *.sln | Select-Object -First 1).FullName
$Configuration = "Release"

function Assert-LastCommandSuccess {
    param([string]$StepName)
    if ($LASTEXITCODE -ne 0) {
        Write-Error "🛑 FAILED: $StepName returned exit code $LASTEXITCODE. Terminating release process."
        exit 1
    }
}

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "   STARTING RELEASE WORKFLOW" -ForegroundColor Cyan
Write-Host "========================================="

Write-Host "`n[1/4] Running Unit Tests..." -ForegroundColor Yellow
dotnet test $SolutionPath --configuration $Configuration -maxcpucount:1

Assert-LastCommandSuccess "Unit Tests"
Write-Host "✅ Tests Passed." -ForegroundColor Green
Set-Location $PSScriptRoot

Write-Host "`n[2/4] Executing PreBuild.ps1..." -ForegroundColor Yellow
if (Test-Path ".\PreBuild.ps1") {
    & ".\PreBuild.ps1" -minor
    Assert-LastCommandSuccess "PreBuild Script"
    Write-Host "✅ PreBuild Script Completed." -ForegroundColor Green
}
else {
    Write-Warning "⚠️ PreBuild.ps1 not found. Skipping."
}

Write-Host "`n[3/4] Building Solution ($Configuration)..." -ForegroundColor Yellow
dotnet build $SolutionPath --configuration $Configuration

Assert-LastCommandSuccess "Solution Build"
Write-Host "✅ Build Success." -ForegroundColor Green
Set-Location $PSScriptRoot

Write-Host "`n[4/4] Executing PostBuild.ps1..." -ForegroundColor Yellow
if (Test-Path ".\PostBuild.ps1") {
    & ".\PostBuild.ps1"
    Assert-LastCommandSuccess "PostBuild Script"
    Write-Host "✅ PostBuild Script Completed." -ForegroundColor Green
}
else {
    Write-Warning "⚠️ PostBuild.ps1 not found. Skipping."
}

Write-Host "`n=========================================" -ForegroundColor Cyan
Write-Host "   RELEASE COMPLETED SUCCESSFULLY" -ForegroundColor Cyan
Write-Host "========================================="
