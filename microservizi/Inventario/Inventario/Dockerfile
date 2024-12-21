FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Inventario/Inventario.csproj", "Inventario/"]
COPY ["Inventario.Business/Inventario.Business.csproj", "Inventario.Business/"]
COPY ["Inventario.Repository/Inventario.Repository.csproj", "Inventario.Repository/"]
COPY ["Inventario.Shared/Inventario.Shared.csproj", "Inventario.Shared/"]
RUN dotnet restore "./Inventario/Inventario.csproj"
COPY . .
WORKDIR "/src/Inventario"
RUN dotnet build "./Inventario.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Inventario.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventario.dll"]