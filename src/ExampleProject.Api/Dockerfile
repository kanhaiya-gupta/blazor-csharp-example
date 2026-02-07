# Build from repository root: docker build -f src/ExampleProject.Api/Dockerfile -t exampleproject-api .
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files first for better layer caching (context = repo root)
COPY src/ExampleProject.Api/ExampleProject.Api.csproj src/ExampleProject.Api/
COPY src/ExampleProject.Core/ExampleProject.Core.csproj src/ExampleProject.Core/
COPY src/ExampleProject.Infrastructure/ExampleProject.Infrastructure.csproj src/ExampleProject.Infrastructure/

RUN dotnet restore src/ExampleProject.Api/ExampleProject.Api.csproj
COPY src/ ./src/
RUN dotnet publish src/ExampleProject.Api/ExampleProject.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ExampleProject.Api.dll"]
