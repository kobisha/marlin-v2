FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Marlin.sqlite.csproj", "."]
RUN dotnet restore "./Marlin.sqlite.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Marlin.sqlite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Marlin.sqlite.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .



ENTRYPOINT ["dotnet", "Marlin.sqlite.dll"]
