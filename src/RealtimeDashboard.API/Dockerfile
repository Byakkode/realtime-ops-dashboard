# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/RealtimeDashboard.API/RealtimeDashboard.API.csproj src/RealtimeDashboard.API/
COPY src/RealtimeDashboard.Application/RealtimeDashboard.Application.csproj src/RealtimeDashboard.Application/
COPY src/RealtimeDashboard.Domain/RealtimeDashboard.Domain.csproj src/RealtimeDashboard.Domain/
COPY src/RealtimeDashboard.Infrastructure/RealtimeDashboard.Infrastructure.csproj src/RealtimeDashboard.Infrastructure/

RUN dotnet restore "src/RealtimeDashboard.API/RealtimeDashboard.API.csproj"

COPY . .

RUN dotnet build "src/RealtimeDashboard.API/RealtimeDashboard.API.csproj" -c Release -o /app/build --no-restore

# Publish
FROM build AS publish
RUN dotnet publish "src/RealtimeDashboard.API/RealtimeDashboard.API.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN useradd --create-home --shell /bin/bash appuser && chown -R appuser:appuser /app
USER appuser

COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "RealtimeDashboard.API.dll"]