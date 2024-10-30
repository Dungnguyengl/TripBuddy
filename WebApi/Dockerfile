# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 433

# Build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY WebApi/ ./WebApi/
COPY CommonService/ ./CommonService/
RUN dotnet remove ./WebApi/WebApi.csproj reference ../CommonService/CommonService.csproj
RUN dotnet add ./WebApi/WebApi.csproj reference ./CommonService/CommonService.csproj
RUN dotnet restore ./WebApi/WebApi.csproj
RUN dotnet build ./WebApi/WebApi.csproj -c Release -o /app/build

# # Publish the service project
FROM build AS publish
RUN dotnet publish ./WebApi/WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# # Production stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENV ASPNETCORE_URLS=https://+:443;http://+:80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "WebApi.dll"]