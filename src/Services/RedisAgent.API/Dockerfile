FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/RedisAgent.API/RedisAgent.API.csproj", "Services/RedisAgent.API/"]
RUN dotnet restore "Services/RedisAgent.API/RedisAgent.API.csproj"
COPY . .
WORKDIR "/src/Services/RedisAgent.API"
RUN dotnet build "RedisAgent.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RedisAgent.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisAgent.API.dll"]
