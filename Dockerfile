# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY PcbDispatchService/PcbDispatchService.csproj ./ 
RUN dotnet restore

# Копируем все файлы и собираем приложение
COPY PcbDispatchService/. ./
RUN dotnet publish -c Release -o out

# Используем официальный образ ASP.NET Core для запуска приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime


ENV PATH="$PATH:/root/.dotnet/tools"


# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем собранное приложение из предыдущего этапа
COPY --from=build /app/out .

# Открываем порты для PostgreSQL и ASP.NET Core
EXPOSE 5432
EXPOSE 80

# Копируем скрипт entrypoint.sh и даем ему права на выполнение
COPY entrypoint.sh ./
RUN chmod +x ./entrypoint.sh
RUN chmod -R a+X /usr/share/dotnet

RUN sed -i 's/\r$//' entrypoint.sh

# Запускаем скрипт при старте контейнера
CMD ["./entrypoint.sh"]