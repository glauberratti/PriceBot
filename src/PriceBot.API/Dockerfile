#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/PriceBot.API/PriceBot.API.csproj", "src/PriceBot.API/"]
COPY ["src/PriceBot.Application/PriceBot.Application.csproj", "src/PriceBot.Application/"]
COPY ["src/PriceBot.CrossCutting.ExchangeRateApi/PriceBot.CrossCutting.CurrencyApi.csproj", "src/PriceBot.CrossCutting.ExchangeRateApi/"]
COPY ["src/PriceBot.CrossCutting.Log/PriceBot.CrossCutting.Log.csproj", "src/PriceBot.CrossCutting.Log/"]
COPY ["src/PriceBot.CrossCutting.Settings/PriceBot.CrossCutting.Settings.csproj", "src/PriceBot.CrossCutting.Settings/"]
COPY ["src/PriceBot.Domain/PriceBot.Domain.csproj", "src/PriceBot.Domain/"]
COPY ["src/PriceBot.Infra.Data/PriceBot.Infra.Data.csproj", "src/PriceBot.Infra.Data/"]
COPY ["src/PriceBot.Infra.IoC/PriceBot.Infra.IoC.csproj", "src/PriceBot.Infra.IoC/"]
RUN dotnet restore "src/PriceBot.API/PriceBot.API.csproj"
COPY . .
WORKDIR "/src/src/PriceBot.API"
RUN dotnet build "PriceBot.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PriceBot.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceBot.API.dll"]