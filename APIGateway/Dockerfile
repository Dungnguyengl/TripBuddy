# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 81
# EXPOSE 433

# Build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY APIGateway/ ./APIGateway/
COPY CommonService/ ./CommonService/
RUN dotnet remove ./APIGateway/APIGateway.csproj reference ../CommonService/CommonService.csproj
RUN dotnet add ./APIGateway/APIGateway.csproj reference ./CommonService/CommonService.csproj
RUN dotnet restore ./APIGateway/APIGateway.csproj
RUN dotnet build ./APIGateway/APIGateway.csproj -c Release -o /app/build

# # Publish the service project
FROM build AS publish
RUN dotnet publish ./APIGateway/APIGateway.csproj -c Release -o /app/publish /p:UseAppHost=false

# # Production stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:81
ENTRYPOINT ["dotnet", "APIGateway.dll"]