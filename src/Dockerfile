# Build stage for .NET
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-backend
WORKDIR /src

# Copy csproj files and restore
COPY backend/src/NextStep.Domain/*.csproj backend/src/NextStep.Domain/
COPY backend/src/NextStep.Application/*.csproj backend/src/NextStep.Application/
COPY backend/src/NextStep.Infrastructure/*.csproj backend/src/NextStep.Infrastructure/
COPY backend/src/NextStep.Api/*.csproj backend/src/NextStep.Api/
COPY backend/nuget.config backend/
RUN dotnet restore backend/src/NextStep.Api/NextStep.Api.csproj --configfile backend/nuget.config

# Copy source and build
COPY backend/src/ backend/src/
RUN dotnet publish backend/src/NextStep.Api/NextStep.Api.csproj -c Release -o /app/publish --no-restore

# Build stage for React
FROM node:20-alpine AS build-frontend
WORKDIR /app

# Copy package files and install
COPY frontend/package*.json ./
RUN npm ci

# Copy source and build
COPY frontend/ ./
RUN npm run build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published backend
COPY --from=build-backend /app/publish .

# Copy frontend build to wwwroot
COPY --from=build-frontend /app/dist ./wwwroot

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "NextStep.Api.dll"]
