#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/inzibackend.Web.Mvc/inzibackend.Web.Mvc.csproj", "src/inzibackend.Web.Mvc/"]
COPY ["src/inzibackend.Web.Core/inzibackend.Web.Core.csproj", "src/inzibackend.Web.Core/"]
COPY ["src/inzibackend.Application/inzibackend.Application.csproj", "src/inzibackend.Application/"]
COPY ["src/inzibackend.Application.Shared/inzibackend.Application.Shared.csproj", "src/inzibackend.Application.Shared/"]
COPY ["src/inzibackend.Core.Shared/inzibackend.Core.Shared.csproj", "src/inzibackend.Core.Shared/"]
COPY ["src/inzibackend.Core/inzibackend.Core.csproj", "src/inzibackend.Core/"]
COPY ["src/inzibackend.EntityFrameworkCore/inzibackend.EntityFrameworkCore.csproj", "src/inzibackend.EntityFrameworkCore/"]
COPY ["src/inzibackend.GraphQL/inzibackend.GraphQL.csproj", "src/inzibackend.GraphQL/"]
RUN dotnet restore "src/inzibackend.Web.Mvc/inzibackend.Web.Mvc.csproj"
COPY . .
WORKDIR "/src/src/inzibackend.Web.Mvc"
RUN dotnet build "inzibackend.Web.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "inzibackend.Web.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "inzibackend.Web.Mvc.dll"]
