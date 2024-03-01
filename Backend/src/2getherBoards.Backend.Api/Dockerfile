FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/2getherBoards.Backend.Api/2getherBoards.Backend.Api.csproj", "src/2getherBoards.Backend.Api/"]
RUN dotnet restore "src/2getherBoards.Backend.Api/2getherBoards.Backend.Api.csproj"
COPY . .
WORKDIR "/src/src/2getherBoards.Backend.Api"
RUN dotnet build "2getherBoards.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "2getherBoards.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "2getherBoards.Backend.Api.dll"]
