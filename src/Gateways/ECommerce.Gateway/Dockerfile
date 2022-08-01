FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Gateways/ECommerce.Gateway/*.csproj Gateways/ECommerce.Gateway/
RUN dotnet restore Gateways/ECommerce.Gateway/*.csproj
COPY . .
RUN dotnet publish Gateways/ECommerce.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "ECommerce.Gateway.dll" ]