FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ExperimentAgent.API/ExperimentAgent.API.csproj", "Services/ExperimentAgent.API/"]
COPY ["Services/BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "Services/BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/ExperimentAgent.API/ExperimentAgent.API.csproj"
COPY . .
WORKDIR "/src/Services/ExperimentAgent.API"
RUN dotnet build "ExperimentAgent.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExperimentAgent.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExperimentAgent.API.dll"]
