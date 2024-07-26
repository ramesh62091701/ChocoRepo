# Load the JSON file
$jsonPath = "msicommands.json"
$jsonContent = Get-Content -Path $jsonPath | ConvertFrom-Json

# Iterate through each key-value pair and run the command
foreach ($key in $jsonContent.PSObject.Properties) {
    Write-Output "Running command for $($key.Name)"
    Invoke-Expression $key.Value
}