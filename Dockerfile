FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

COPY --from=build /app/publish .

# Copy the dictionaries directory, needed in runtime
COPY dictionaries /app/dictionaries

# Expose the port and set environment variables
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Set the entry point to the application DLL
ENTRYPOINT ["dotnet", "JmdictGQL.dll"]
