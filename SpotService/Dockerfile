# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 83
# EXPOSE 433

# Build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY SpotService/ ./SpotService/
COPY CommonService/ ./CommonService/
RUN dotnet remove ./SpotService/SpotService.csproj reference ../CommonService/CommonService.csproj
RUN dotnet add ./SpotService/SpotService.csproj reference ./CommonService/CommonService.csproj
RUN dotnet restore ./SpotService/SpotService.csproj
RUN dotnet build ./SpotService/SpotService.csproj -c Release -o /app/build

# # Publish the service project
FROM build AS publish
RUN dotnet publish ./SpotService/SpotService.csproj -c Release -o /app/publish /p:UseAppHost=false

# # Production stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:83
ENTRYPOINT ["dotnet", "SpotService.dll"]