# Build stage for .NET
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-backend
WORKDIR /src

# Copy everything and build
COPY backend/ backend/
RUN dotnet publish backend/src/NextStep.Api/NextStep.Api.csproj -c Release -o /app/publish

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
