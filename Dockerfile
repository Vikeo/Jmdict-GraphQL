FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

# Build the project and publish the output to the /out directory
RUN dotnet publish -c Release -o /out

# Build the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-env /out .

# Copy the dictionaries directory from the build context into the final image
COPY dictionaries /app/dictionaries

EXPOSE 8080

ENTRYPOINT ["dotnet", "JmdictGQL.dll"]
