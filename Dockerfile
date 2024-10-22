#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 9084
ENV ASPNETCORE_URLS="http://*:9084"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV NUGET_XMLDOC_MODE=none
COPY . .
WORKDIR "/WeatherForecast.Api"
RUN dotnet restore "WeatherForecast.Api.csproj" --configfile "./nuget.config"
RUN dotnet build "WeatherForecast.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherForecast.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WeatherForecast.Api.dll"]