# ==========================
# 1. Build Stage
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY InmobiliaryMgmt.Api/*.csproj InmobiliaryMgmt.Api/
COPY InmobiliaryMgmt.Application/*.csproj InmobiliaryMgmt.Application/
COPY InmobiliaryMgmt.Domain/*.csproj InmobiliaryMgmt.Domain/
COPY InmobiliaryMgmt.Infrastructure/*.csproj InmobiliaryMgmt.Infrastructure/

RUN dotnet restore InmobiliaryMgmt.Api/InmobiliaryMgmt.Api.csproj

# Copiar todo el código
COPY . .

RUN dotnet build InmobiliaryMgmt.Api/InmobiliaryMgmt.Api.csproj -c Release -o /app/build
RUN dotnet publish InmobiliaryMgmt.Api/InmobiliaryMgmt.Api.csproj -c Release -o /app/publish

# ==========================
# 2. Runtime Stage
# ==========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "InmobiliaryMgmt.Api.dll"]
