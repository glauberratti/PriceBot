FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

# Copy everything
COPY ./src/ ./
# Restore as distinct layers
RUN dotnet restore "./PriceBot.API/PriceBot.API.csproj"
# Build and publish a release
RUN dotnet publish "./PriceBot.API/PriceBot.API.csproj" -c Release -o /publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
WORKDIR /App
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet", "PriceBot.API.dll"]