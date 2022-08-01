FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/ECommerce.Shared/*.csproj Shared/ECommerce.Shared/
COPY Services/Order/ECommerce.Services.Order.Application/*.csproj Services/Order/ECommerce.Services.Order.Application/
COPY Services/Order/ECommerce.Services.Order.Domain/*.csproj Services/Order/ECommerce.Services.Order.Domain/
COPY Services/Order/ECommerce.Services.Order.Domain.Core/*.csproj Services/Order/ECommerce.Services.Order.Domain.Core/
COPY Services/Order/ECommerce.Services.Order.Infrastructure/*.csproj Services/Order/ECommerce.Services.Order.Infrastructure/
COPY Services/Order/ECommerce.Services.Order.API/*.csproj Services/Order/ECommerce.Services.Order.API/
RUN dotnet restore Services/Order/ECommerce.Services.Order.API/*.csproj
COPY . .
RUN dotnet publish Services/Order/ECommerce.Services.Order.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "ECommerce.Services.Order.API.dll" ]