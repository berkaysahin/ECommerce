FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/ECommerce.Shared/*.csproj Shared/ECommerce.Shared/
COPY Services/Discount/ECommerce.Services.Discount/*.csproj Services/Discount/ECommerce.Services.Discount/
RUN dotnet restore Services/Discount/ECommerce.Services.Discount/*.csproj
COPY . .
RUN dotnet publish Services/Discount/ECommerce.Services.Discount/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "ECommerce.Services.Discount.dll" ]