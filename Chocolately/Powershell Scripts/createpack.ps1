# param(
#     [string]$msicommand
# )

$msiCommand = '"D:\CHOCO\builds\22.0.0.43559\MSI-Files\MSIDemo.msi" US_LOCAL_SERVER="sonatashrdss" US_LOCAL_AUTHENTICATION="1" US_LOCAL_USERNAME="sa" US_LOCAL_PASSWORD="7QCGn94An8fz" US_MSDTC_MUST_REINSTALL="1" REBOOT="ReallySuppress" /l*vx "C:\Program Files (x86)\Common Files\US Group\Install Logs\Super Site Server.msi.log"'

if ($msiCommand -match '^"([^"]+)"\s+(.*)$') {
    $msiPath = $matches[1]
    $msiArgs = $matches[2]
} else {
    Write-Error "Invalid command format. Unable to parse the MSI path and arguments."
    exit 1
}

#need confirmation
if ($msiPath -match '\\(\d+\.\d+\.\d+\.\d+)\\') {
    $packVersion = $matches[1]
} else {
    Write-Error "Unable to extract the version from the MSI path."
    exit 1
}

$escapedMsiArgs = $msiArgs -replace '"', '`"'

# Determine file type based on extension
$fileExtension = [System.IO.Path]::GetExtension($msiPath).ToLower()
$fileType = if ($fileExtension -eq '.msi') { 'MSI' } elseif ($fileExtension -eq '.exe') { 'EXE' } else { 
    Write-Error "Unsupported file type: $fileExtension"
    exit 1
}

if ($fileType -eq 'MSI') {
    $silentArgs = "/qn /i $escapedMsiArgs"
}
$fileName = [System.IO.Path]::GetFileNameWithoutExtension($msiPath)

# Calculate checksum
$checksum = Get-FileHash -Path $msiPath -Algorithm SHA256 | Select-Object -ExpandProperty Hash

# Define package variables
$packageName = $fileName

# Create a new Chocolatey package
choco new $packageName 

# Set paths
$packageDir = Join-Path -Path (Get-Location) -ChildPath $packageName
$nuspecPath = Join-Path -Path $packageDir -ChildPath "$packageName.nuspec"
$installScriptPath = Join-Path -Path $packageDir -ChildPath "tools\chocolateyinstall.ps1"

#replace it with xml.
# Update nuspec file
$nuspecContent = @"
<?xml version="1.0"?>
<package>
  <metadata>
    <id>$packageName</id>
    <version>$packVersion</version>
    <title>Super Site Server</title>
    <authors>UKG</authors>
    <owners>UKG</owners>
    <description>Installs $fileName</description>
    <dependencies>
    </dependencies>
  </metadata>
</package>
"@
Set-Content -Path $nuspecPath -Value $nuspecContent

$installScriptContent=@"
`$ErrorActionPreference = 'Stop' 
`$fileLocation = '$msiPath'

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

# Pack the package
choco pack $nuspecPath
