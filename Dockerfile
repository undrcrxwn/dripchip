FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 8080

# Copy project files
COPY /src .
# Restore as distinct layers
RUN dotnet restore "DripChip.Api/DripChip.Api.csproj"
# Build and publish a release
RUN dotnet publish "DripChip.Api/DripChip.Api.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "DripChip.Api.dll"]