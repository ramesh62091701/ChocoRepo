# Define the output directory
$outputDir = "$$OUTPUTPATH$$"

# Create the output directory if it doesn't exist
if (-Not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir
}

# Define the content for each file
$serviceCode = @"
$$SERVICECODE$$
"@

# Escape newlines and special characters
$serviceCodeEscaped = $serviceCode -replace "`t", "\\t" -replace '"', '\"'

# Run the dotnet new command using the escaped code
cd $outputDir

dotnet new $$PROJECTTEMPLATE$$ -o "$$FILENAME$$" `
    --Service "$serviceCodeEscaped" `
    --Framework "$$FRAMEWORK$$" `
    **ENABLESWAGGER**

Write-Host "The placeholder replacement and template creation process is complete."
