#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["Koishibot.Web/Koishibot.Web.csproj", "Koishibot.Web/"]
RUN dotnet restore "./Koishibot.Web/Koishibot.Web.csproj"
COPY . .
WORKDIR "/src/Koishibot.Web"
RUN dotnet build "./Koishibot.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Koishibot.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Koishibot.Web.dll"]