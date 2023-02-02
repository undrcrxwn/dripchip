FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DripChip.Api/DripChip.Api.csproj", "DripChip.Api/"]
RUN dotnet restore "src/DripChip.Api/DripChip.Api.csproj"
COPY . .
WORKDIR "/src/DripChip.Api"
RUN dotnet build "DripChip.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DripChip.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DripChip.Api.dll"]
