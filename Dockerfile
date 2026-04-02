# Этап 1: Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта
COPY IsLabApp.csproj .
RUN dotnet restore

# Копируем весь код и собираем
COPY . .
RUN dotnet publish -c Release -o /app

# Этап 2: Запуск приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Переменные окружения по умолчанию
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Открываем порт
EXPOSE 8080

# Запускаем приложение
ENTRYPOINT ["dotnet", "IsLabApp.dll"]