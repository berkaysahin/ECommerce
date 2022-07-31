FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/ECommerce.Shared/*.csproj Shared/ECommerce.Shared/
COPY Services/Catalog/ECommerce.Services.Catalog/*.csproj Services/Catalog/ECommerce.Services.Catalog/
RUN dotnet restore Services/Catalog/ECommerce.Services.Catalog/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/ECommerce.Services.Catalog/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "ECommerce.Services.Catalog.dll" ]