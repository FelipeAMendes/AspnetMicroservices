#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApps/AspNetWebApp/AspNetWebApp.csproj", "WebApps/AspNetWebApp/"]
RUN dotnet restore "WebApps/AspNetWebApp/AspNetWebApp.csproj"
COPY . .
WORKDIR "/src/WebApps/AspNetWebApp"
RUN dotnet build "AspNetWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetWebApp.dll"]
