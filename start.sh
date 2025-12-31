#!/bin/bash

# Restaurar dependências
dotnet restore

# Build da aplicação
dotnet build --configuration Release

# Executar a aplicação
cd GuardiaoDasMemorias
dotnet run --configuration Release --no-build
