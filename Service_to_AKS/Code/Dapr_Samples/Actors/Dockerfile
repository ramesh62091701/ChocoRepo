# Use the .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore ./service/SmartDevice.Service.csproj
# Build and publish a release
RUN dotnet publish ./service/SmartDevice.Service.csproj -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /app/out .

# Expose the port used by your Dapr actor application
EXPOSE 5001


# Run your Dapr actor application
CMD ["dotnet", "SmartDevice.Service.dll"]

