# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

# Copy project file first to maximize restore-layer caching.
COPY ["eCommerce.UI/eCommerce.UI.csproj", "eCommerce.UI/"]
RUN dotnet restore "eCommerce.UI/eCommerce.UI.csproj"

FROM restore AS build
COPY . .
WORKDIR /src/eCommerce.UI
RUN dotnet build "eCommerce.UI.csproj" -c Release --no-restore

FROM build AS publish
RUN dotnet publish "eCommerce.UI.csproj" -c Release -o /app/publish --no-build --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Default container port; can be overridden by ASPNETCORE_URLS at runtime.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eCommerce.UI.dll"]

