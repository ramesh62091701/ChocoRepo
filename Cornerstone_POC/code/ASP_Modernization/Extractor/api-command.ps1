# Define the output directory
$outputDir = "$$OUTPUTPATH$$"

# Create the output directory if it doesn't exist
if (-Not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir
}

# Define the content for each file
$bffServiceCode = @"
$$BFFSERVICECODE$$
"@

$controllerCode = @"
$$CONTROLLERCODE$$
"@

$dataServiceCode = @"
$$DATASERVICECODE$$
"@

$dataRepositoryCode = @"
$$DATAREPOSITORYCODE$$
"@

# Escape newlines and special characters
$bffServiceCodeEscaped = $bffServiceCode -replace "`t", "\\t" -replace '"', '\"'
$controllerCodeEscaped = $controllerCode -replace "`t", "\\t" -replace '"', '\"'
$dataServiceCodeEscaped = $dataServiceCode -replace "`t", "\\t" -replace '"', '\"'
$dataRepositoryCodeEscaped = $dataRepositoryCode -replace "`t", "\\t" -replace '"', '\"'

# Run the dotnet new command using the escaped code
cd $outputDir

dotnet new apicustomtemplate -o "$$FILENAME$$" `
    --BFFService "$bffServiceCodeEscaped" `
    --Service "$controllerCodeEscaped" `
    --DataService "$dataServiceCodeEscaped" `
    --DataRepository "$dataRepositoryCodeEscaped" `
    --Framework "$$FRAMEWORK$$" `
    --BFF_Folder "true" `
    --Controller_Folder "true" `
    --DataRepo_Folder "true" `
    --DataService_Folder "true" `
    **ENABLESWAGGER**

Write-Host "The placeholder replacement and template creation process is complete."
