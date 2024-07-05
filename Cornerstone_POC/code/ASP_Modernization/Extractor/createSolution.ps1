# createSolution.ps1

# Define the output directory and solution name
$outputDir = "$$OUTPUTPATH$$"
$solutionName = "MySolution"

# Create the solution file
cd $outputDir
dotnet new sln -n $solutionName

# Get a list of project files in the output directory
$projects = Get-ChildItem -Path $outputDir -Filter *.csproj -Recurse

# Add each project to the solution
foreach ($project in $projects) {
    dotnet sln $solutionName.sln add $project.FullName
}

Write-Host "Solution file and project addition process is complete."

