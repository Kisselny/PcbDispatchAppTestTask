FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /app

# �������� ������ ���� ������� ��� ����������� restore
COPY ["PcbDispatchService/PcbDispatchService.csproj", "./PcbDispatchService/"]

RUN dotnet restore "./PcbDispatchService/PcbDispatchService.csproj"

# �������� ���� �������� ���
COPY . .

WORKDIR "/app/PcbDispatchService"
RUN dotnet build "PcbDispatchService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "PcbDispatchService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PcbDispatchService.dll"]
