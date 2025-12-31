#!/bin/bash
set -e

echo "Iniciando build da aplicação .NET..."

# Restaurar dependências
echo "Restaurando dependências..."
dotnet restore GuardiaoDasMemorias.sln

# Publicar a aplicação
echo "Publicando aplicação..."
dotnet publish GuardiaoDasMemorias/GuardiaoDasMemorias.csproj -c Release -o out

# Executar a aplicação
echo "Iniciando aplicação..."
cd out
exec dotnet GuardiaoDasMemorias.dll
