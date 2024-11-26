#!/bin/sh

set -e



apt-get update 
#&& \
 # apt-get install -y dotnet-sdk-9.0

apt-get install -y wget
wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb



dotnet tool install --global dotnet-ef


# Выполнение миграций
echo "Applying migrations..."
dotnet ef database update

# Запуск приложения
echo "Starting application..."
exec dotnet PcbDispatchService.dll  # Замените PcbDispatchService.dll на имя вашего сборки, если оно отличается.