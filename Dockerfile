# Используем базовый образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# Копируем csproj файл и восстанавливаем зависимости
COPY IdentityService.csproj .
RUN dotnet restore

# Копируем остальные файлы проекта и собираем приложение
COPY . .
RUN dotnet publish -c Release -o out

# Используем базовый образ ASP.NET для запуска
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /app
COPY --from=build /app/out .

# Устанавливаем переменные окружения
ENV ASPNETCORE_URLS=http://*:8000

# Запускаем сервис
ENTRYPOINT ["dotnet", "IdentityService.dll"]