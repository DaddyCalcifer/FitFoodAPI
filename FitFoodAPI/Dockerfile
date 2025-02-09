# Базовый образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем только файл проекта
COPY FitFoodAPI/FitFoodAPI.csproj FitFoodAPI/
RUN dotnet restore FitFoodAPI/FitFoodAPI.csproj

# Копируем все файлы проекта
COPY FitFoodAPI/ FitFoodAPI/
WORKDIR /src/FitFoodAPI
RUN dotnet build FitFoodAPI.csproj -c $BUILD_CONFIGURATION -o /app/build

# Выполняем publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish FitFoodAPI.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ для запуска приложения
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitFoodAPI.dll"]