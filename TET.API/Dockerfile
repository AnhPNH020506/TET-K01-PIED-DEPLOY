FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TET.API/TET.API.csproj", "TET.API/"]
COPY ["Tet.Service/Tet.Service.csproj", "Tet.Service/"]
COPY ["Tet.Repository/Tet.Repository.csproj", "Tet.Repository/"]
RUN dotnet restore "TET.API/TET.API.csproj"
COPY . .
WORKDIR "/src/TET.API"
RUN dotnet build "./TET.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TET.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TET.API.dll"]
