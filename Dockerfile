# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy rest of source code
COPY . .

# Publish in Release mode
RUN dotnet publish -c Release -o /app/publish


# Stage 2: Runtime (smaller image)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# App MUST listen on port 5215
ENV ASPNETCORE_URLS=http://+:5215

# Expose container port
EXPOSE 5215

# Copy published output
COPY --from=build /app/publish .

# Start application
ENTRYPOINT ["dotnet", "Track_Backend.dll"]