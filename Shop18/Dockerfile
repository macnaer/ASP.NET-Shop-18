#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Shop18/Shop18.csproj", "Shop18/"]
RUN dotnet restore "Shop18/Shop18.csproj"
COPY . .
WORKDIR "/src/Shop18"
RUN dotnet build "Shop18.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shop18.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop18.dll"]