#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BackEnd.API/BackEnd.API.csproj", "BackEnd.API/"]
COPY ["BackEnd.Infrastructure/BackEnd.Infrastructure.csproj", "BackEnd.Infrastructure/"]
COPY ["BackEnd.Core/BackEnd.Core.csproj", "BackEnd.Core/"]
RUN dotnet restore "BackEnd.API/BackEnd.API.csproj"
COPY . .
WORKDIR "/src/BackEnd.API"
RUN dotnet build "BackEnd.API.csproj" -c Release -o /app/build
COPY ["BackEnd.API/appsettings.json", "BackEnd.API/"]

FROM build AS publish
RUN dotnet publish "BackEnd.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackEnd.API.dll"]

