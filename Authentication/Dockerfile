# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 82
# EXPOSE 433

# Build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Authentication/ ./Authentication/
COPY CommonService/ ./CommonService/
RUN dotnet remove ./Authentication/Authentication.csproj reference ../CommonService/CommonService.csproj
RUN dotnet add ./Authentication/Authentication.csproj reference ./CommonService/CommonService.csproj
RUN dotnet restore ./Authentication/Authentication.csproj
RUN dotnet build ./Authentication/Authentication.csproj -c Release -o /app/build

# # Publish the service project
FROM build AS publish
RUN dotnet publish ./Authentication/Authentication.csproj -c Release -o /app/publish /p:UseAppHost=false

# # Production stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:82
ENTRYPOINT ["dotnet", "Authentication.dll"]