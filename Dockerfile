# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["GuardiaoDasMemorias/GuardiaoDasMemorias.csproj", "GuardiaoDasMemorias/"]
RUN dotnet restore "GuardiaoDasMemorias/GuardiaoDasMemorias.csproj"

# Copiar todo o código e fazer build
COPY . .
WORKDIR "/src/GuardiaoDasMemorias"
RUN dotnet build "GuardiaoDasMemorias.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "GuardiaoDasMemorias.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GuardiaoDasMemorias.dll"]
