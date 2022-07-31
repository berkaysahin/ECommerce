FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
COPY Shared/ECommerce.Shared/*.csproj Shared/ECommerce.Shared/
COPY IdentityServer/ECommerce.IdentityServer/*.csproj IdentityServer/ECommerce.IdentityServer/
RUN dotnet restore IdentityServer/ECommerce.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/ECommerce.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "ECommerce.IdentityServer.dll" ]