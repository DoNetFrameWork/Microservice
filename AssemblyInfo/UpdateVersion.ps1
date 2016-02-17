param([string]$version = "version", [string]$subversion = "sub")
Write-Host "BUILD_BUILDNUMBER contents: $Env:BUILD_BUILDNUMBER"
IF([string]::IsNullOrWhitespace($version)){$version=$Env:BUILD_BUILDNUMBER}
Write-Host "Version:" $version;
IF(![string]::IsNullOrWhitespace($subversion)){$nugetversion=$version+"-"+$subversion}
Write-Host "Nuget Version:" $nugetversion;
(Get-Content AssemblyInfo\SharedAssemblyInfo.cs).replace('0.0.0.0', $version) | Set-Content AssemblyInfo\SharedAssemblyInfo.cs
Get-ChildItem "**\*.nuspec" -Recurse | ForEach-Object -Process {
    (Get-Content $_) -Replace '{ps1.replace}', $nugetversion | Set-Content $_
}
Write-Host "Over and out."