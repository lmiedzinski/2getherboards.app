FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TogetherBoardsApp.Backend.Api/TogetherBoardsApp.Backend.Api.csproj", "src/TogetherBoardsApp.Backend.Api/"]
RUN dotnet restore "src/TogetherBoardsApp.Backend.Api/TogetherBoardsApp.Backend.Api.csproj"
COPY . .
WORKDIR "/src/src/TogetherBoardsApp.Backend.Api"
RUN dotnet build "TogetherBoardsApp.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TogetherBoardsApp.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TogetherBoardsApp.Backend.Api.dll"]
