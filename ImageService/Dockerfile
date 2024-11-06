# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ImageService/ ./ImageService/
COPY CommonService/ ./CommonService/
RUN dotnet remove ./ImageService/ImageService.csproj reference ../CommonService/CommonService.csproj
RUN dotnet add ./ImageService/ImageService.csproj reference ./CommonService/CommonService.csproj
RUN dotnet restore ./ImageService/ImageService.csproj
RUN dotnet build ./ImageService/ImageService.csproj -c Release -o /app/build

# # Publish the service project
FROM build AS publish
RUN dotnet publish ./ImageService/ImageService.csproj -c Release -o /app/publish /p:UseAppHost=false

# # Production stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageService.dll"]