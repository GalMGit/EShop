FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/EShop.Api/EShop.Api.csproj", "src/backend/EShop.Api/"]
COPY ["src/backend/Common/EShop.Shared/EShop.Shared.csproj", "src/backend/Common/EShop.Shared/"]
COPY ["src/backend/Common/EShop.Contracts/EShop.Contracts.csproj", "src/backend/Common/EShop.Contracts/"]
COPY ["src/backend/Modules/Identity/Modules.Identity/Modules.Identity.csproj", "src/backend/Modules/Identity/Modules.Identity/"]
RUN dotnet restore "src/backend/EShop.Api/EShop.Api.csproj"
COPY . .
WORKDIR "/src/src/backend/EShop.Api"
RUN dotnet build "./EShop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.Api.dll"]
