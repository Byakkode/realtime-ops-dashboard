# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/RealtimeDashboard.Client/RealtimeDashboard.Client.csproj src/RealtimeDashboard.Client/

RUN dotnet restore "src/RealtimeDashboard.Client/RealtimeDashboard.Client.csproj"

COPY . .

RUN dotnet publish "src/RealtimeDashboard.Client/RealtimeDashboard.Client.csproj" -c Release -o /app/publish --no-restore

# Nginx
FROM nginx:alpine AS final

COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY src/RealtimeDashboard.Client/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80