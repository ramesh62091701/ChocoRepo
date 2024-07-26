param (
    [string]$version,
    [string]$msipath,
    [string]$msiargs,
    [string]$packagepath,
    [bool]$upgrade
)

$currentPath = Get-Location

$escapedMsiArgs = $msiargs -replace '"', '`"'
$fileExtension = [System.IO.Path]::GetExtension($msipath).ToLower()
$fileType = if ($fileExtension -eq '.msi') { 'MSI' } elseif ($fileExtension -eq '.exe') { 'EXE' } else { 
    Write-Error "Unsupported file type: $fileExtension"
    exit 1
}

if ($fileType -eq 'MSI') {
    # $silentArgs = "/qn /i $escapedMsiArgs" # For main MSI files
    $silentArgs = $escapedMsiArgs #for local testing

}
$fileName = [System.IO.Path]::GetFileNameWithoutExtension($msipath)

# Calculate checksum
$checksum = Get-FileHash -Path $msipath -Algorithm SHA256 | Select-Object -ExpandProperty Hash

# Set paths
Set-Location -Path $packagepath
$packageDir = Join-Path -Path (Get-Location) -ChildPath $fileName
$nuspecPath = Join-Path -Path $packageDir -ChildPath "$fileName.nuspec"
$installScriptPath = Join-Path -Path $packageDir -ChildPath "tools\chocolateyinstall.ps1"

if (-not $upgrade) {
    # Create a new Chocolatey package
    choco new $fileName --version $version

    $xml = [xml](Get-Content $nuspecPath)
    $xml.package.metadata.id = $fileName
    $xml.package.metadata.title = $fileName
    $xml.package.metadata.authors = "UKG"
    $xml.package.metadata.summary = ""
    $xml.package.metadata.description = $fileName
    $projectUrlNode = $xml.package.metadata.SelectSingleNode("projectUrl")
    $authorsNode = $xml.package.metadata.SelectSingleNode("authors")
    if ($projectUrlNode) {
        $xml.package.metadata.RemoveChild($projectUrlNode)
    }
    if($authorsNode){
        $xml.package.metadata.RemoveChild($authorsNode)
    }
    $xml.Save($nuspecPath)

    $installScriptContent=@"
`$ErrorActionPreference = 'Stop' 
`$fileLocation = '$msipath'

`$packageArgs = @{
    packageName    = `$env:ChocolateyPackageName
    fileType       = '$fileType'
    file           = `$fileLocation
    softwareName   = '$fileName'
    checksum64     = '$checksum'
    checksumType64 = 'sha256'

    silentArgs     = '$silentArgs'
    validExitCodes = @(0, 3010, 1641)
}

Install-ChocolateyPackage @packageArgs 

"@
    Set-Content -Path $installScriptPath -Value $installScriptContent

} else {
    # Update existing Chocolatey package
    $xml = [xml](Get-Content $nuspecPath)
    $xml.package.metadata.version = $version
    $xml.Save($nuspecPath)

    # Read the current content of the chocolateyInstall.ps1
    $installScriptContent = Get-Content -Path $installScriptPath -Raw

    # Replace checksum and file name
    $installScriptContent = $installScriptContent -replace "checksum64\s*=\s*\'\S*\'", "checksum64 = '$checksum'"
    $installScriptContent = $installScriptContent -replace "softwareName\s*=\s*\'\S*\'", "softwareName = '$fileName'"
    $installScriptContent = $installScriptContent -replace "fileLocation\s*=\s*\'\S*\'", "fileLocation = '$msipath'"
    $installScriptContent = $installScriptContent -replace "silentArgs\s*=\s*\'\S*\'", "silentArgs = '$silentArgs'"

    # Write updated content back to chocolateyInstall.ps1
    Set-Content -Path $installScriptPath -Value $installScriptContent
}

# Pack the package
choco pack $nuspecPath
Set-Location -Path $currentPath


Write-Output "Package processed successfully: $fileName"
