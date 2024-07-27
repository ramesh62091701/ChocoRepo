$ErrorActionPreference = 'Stop' 
$fileLocation = 'D:\CHOCO\builds\22.0.0.50000\MSI-Files\MSIDemo.msi'

$packageArgs = @{
    packageName    = $env:ChocolateyPackageName
    fileType       = 'MSI'
    file           = $fileLocation
    softwareName = 'MSIDemo'
    checksum64 = 'D726C5E50B879FA0BF828A844A090181D8C4874A9209C5AC69BFB96EA6444BC6'
    checksumType64 = 'sha256'

    silentArgs     = '/qn /norestart'
    validExitCodes = @(0, 3010, 1641)
}

Install-ChocolateyPackage @packageArgs 


