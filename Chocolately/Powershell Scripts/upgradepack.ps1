# Define paths and new values
# add multiple arguments with --
$msiCommand = '"D:\CHOCO\builds\22.0.0.50000\MSI-Files\MSIDemo2.msi" US_LOCAL_SERVER="sonatashrdss" US_LOCAL_AUTHENTICATION="1" US_LOCAL_USERNAME="sa" US_LOCAL_PASSWORD="7QCGn94An8fz" US_MSDTC_MUST_REINSTALL="1" REBOOT="ReallySuppress" /l*vx "C:\Program Files (x86)\Common Files\US Group\Install Logs\Super Site Server.msi.log"'

if ($msiCommand -match '^"([^"]+)"\s+(.*)$') {
    $msiPath = $matches[1]
    $msiArgs = $matches[2]
} else {
    Write-Error "Invalid command format. Unable to parse the MSI path and arguments."
    exit 1
}

if ($msiPath -match '\\(\d+\.\d+\.\d+\.\d+)\\') {
    $packVersion = $matches[1]
} else {
    Write-Error "Unable to extract the version from the MSI path."
    exit 1
}

$escapedMsiArgs = $msiArgs -replace '"', '`"'

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

# replce
$packageDir = "MSIDemo" # Directory where your Chocolatey package is located
$packageNuspecFile = Join-Path $packageDir "$packageDir.nuspec" # Path to the .nuspec file
$newVersion = $packVersion
$newFileName = $fileName # New MSI or EXE file name
$newChecksum = $checksum # New checksum value

# Update .nuspec file
$xml = [xml](Get-Content $packageNuspecFile)
$xml.package.metadata.version = $newVersion
$xml.package.metadata.title = $newFileName
$xml.Save($packageNuspecFile)

# Update checksum and file paths in chocolateyInstall.ps1
$installScriptPath = Join-Path $packageDir "tools\chocolateyInstall.ps1"

# Read the current content of the chocolateyInstall.ps1
$installScriptContent = Get-Content -Path $installScriptPath -Raw

# Replace checksum and file name
$installScriptContent = $installScriptContent -replace "checksum64\s*=\s*\'\S*\'", "checksum64 = '$newChecksum'"
$installScriptContent = $installScriptContent -replace "softwareName\s*=\s*\'\S*\'", "softwareName = '$newFileName'"
$installScriptContent = $installScriptContent -replace "fileLocation\s*=\s*\'\S*\'", "fileLocation = '$msiPath'"
$installScriptContent = $installScriptContent -replace "`silentArgs\s*=\s*\'\S*\'", "silentArgs = '$silentArgs'"

# Write updated content back to chocolateyInstall.ps1
Set-Content -Path $installScriptPath -Value $installScriptContent

choco pack $packageNuspecFile

Write-Output "Package updated successfully:"
Write-Output "Version: $newVersion"
Write-Output "File Name: $newFileName"
Write-Output "Checksum: $newChecksum"
