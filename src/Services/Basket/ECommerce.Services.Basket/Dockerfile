FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/ECommerce.Shared/*.csproj Shared/ECommerce.Shared/
COPY Services/Basket/ECommerce.Services.Basket/*.csproj Services/Basket/ECommerce.Services.Basket/
RUN dotnet restore Services/Basket/ECommerce.Services.Basket/*.csproj
COPY . .
RUN dotnet publish Services/Basket/ECommerce.Services.Basket/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "ECommerce.Services.Basket.dll" ]