# Load the JSON file
$jsonPath = "localmsicommands.json"
$jsonContent = Get-Content -Path $jsonPath | ConvertFrom-Json

# Iterate through each key-value pair and run the command
foreach ($key in $jsonContent.PSObject.Properties) {
    Write-Output "Running command for $($key.Name)"
    Invoke-Expression $key.Value
}

Set-Location -Path "D:\CHOCO\packs" 
choco install msidemo --version 22.0.0.43559 -s . -y --force

choco upgrade msidemo --version 22.0.0.50000 -s . -y