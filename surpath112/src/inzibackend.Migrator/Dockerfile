#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/inzibackend.Migrator/inzibackend.Migrator.csproj", "src/inzibackend.Migrator/"]
COPY ["src/inzibackend.EntityFrameworkCore/inzibackend.EntityFrameworkCore.csproj", "src/inzibackend.EntityFrameworkCore/"]
COPY ["src/inzibackend.Core/inzibackend.Core.csproj", "src/inzibackend.Core/"]
COPY ["src/inzibackend.Core.Shared/inzibackend.Core.Shared.csproj", "src/inzibackend.Core.Shared/"]
RUN dotnet restore "src/inzibackend.Migrator/inzibackend.Migrator.csproj"
COPY . .
WORKDIR "/src/src/inzibackend.Migrator"
RUN dotnet build "inzibackend.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "inzibackend.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "inzibackend.Migrator.dll", "-s"]
