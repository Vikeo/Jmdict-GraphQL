# Use the official ASP.NET Core runtime image for linux/amd64
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim-amd64 AS base
WORKDIR /app

# Use the SDK image to build the application for linux/amd64
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image for the final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copy the published output from the build stage
# COPY --from=build /out .

# Copy the dictionaries directory from the build context into the final image
COPY dictionaries /app/dictionaries

# Expose the port the app runs on
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Set the entry point to the application DLL
ENTRYPOINT ["dotnet", "JmdictGQL.dll"]
