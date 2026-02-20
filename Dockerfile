# ── Stage 1: Build ──
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj first and restore (caches NuGet layer)
COPY PersonApi.csproj .
RUN dotnet restore

# Copy everything else and publish
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ── Stage 2: Runtime ──
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Create a directory for the JSON data file to persist
RUN mkdir -p /app/Data

# ASP.NET Core listens on 8080 by default in .NET 8+
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "PersonApi.dll"]
