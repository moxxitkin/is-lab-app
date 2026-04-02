FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

# Копируем опубликованное приложение
COPY bin/Release/net8.0/publish/ .

ENTRYPOINT ["dotnet", "IsLabApp.dll"]