FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app

COPY . ./
RUN dotnet build -c Release

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "FlightAPI.dll"]
