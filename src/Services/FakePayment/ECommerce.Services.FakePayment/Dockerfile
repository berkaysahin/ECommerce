FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/ECommerce.Shared/*.csproj Shared/ECommerce.Shared/
COPY Services/FakePayment/ECommerce.Services.FakePayment/*.csproj Services/FakePayment/ECommerce.Services.FakePayment/
RUN dotnet restore Services/FakePayment/ECommerce.Services.FakePayment/*.csproj
COPY . .
RUN dotnet publish Services/FakePayment/ECommerce.Services.FakePayment/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "ECommerce.Services.FakePayment.dll" ]