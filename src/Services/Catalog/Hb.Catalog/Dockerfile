#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Catalog/Hb.Catalog/Hb.Catalog.csproj", "Services/Catalog/Hb.Catalog/"]
RUN dotnet restore "Services/Catalog/Hb.Catalog/Hb.Catalog.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Hb.Catalog"
RUN dotnet build "Hb.Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hb.Catalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hb.Catalog.dll"]
