FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ConfigAgent.API/ConfigAgent.API.csproj", "Services/ConfigAgent.API/"]
COPY ["Services/BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "Services/BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/ConfigAgent.API/ConfigAgent.API.csproj"
COPY . .
WORKDIR "/src/Services/ConfigAgent.API"
RUN dotnet build "ConfigAgent.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConfigAgent.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigAgent.API.dll"]
