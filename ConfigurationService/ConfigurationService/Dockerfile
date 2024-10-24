# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ConfigurationService/ConfigurationService/ ./ConfigurationService/
COPY CommonService/ ./CommonService/
RUN dotnet remove ./ConfigurationService/ConfigurationService.csproj reference ../../CommonService/CommonService.csproj
RUN dotnet add ./ConfigurationService/ConfigurationService.csproj reference ./CommonService/CommonService.csproj
RUN dotnet restore ./ConfigurationService/ConfigurationService.csproj
RUN dotnet build ./ConfigurationService/ConfigurationService.csproj -c Release -o /app/build

# # Publish the service project
FROM build AS publish
RUN dotnet publish ./ConfigurationService/ConfigurationService.csproj -c Release -o /app/publish /p:UseAppHost=false

# # Production stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigurationService.dll"]