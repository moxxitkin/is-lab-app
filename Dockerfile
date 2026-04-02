# Этап 1: Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY IsLabApp.csproj .
RUN dotnet restore

# Копируем весь код и публикуем
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Этап 2: Запуск приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

# Копируем опубликованное приложение из этапа сборки
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "IsLabApp.dll"]