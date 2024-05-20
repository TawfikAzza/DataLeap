FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MigrationService/MigrationService.csproj", "MigrationService/"]
RUN dotnet restore "MigrationService/MigrationService.csproj"
COPY . .
WORKDIR "/src/MigrationService"
RUN dotnet build "MigrationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MigrationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MigrationService.dll"]
