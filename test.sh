#!/usr/bin/env bash
set -e

if ! command -v dotnet >/dev/null; then
    echo "dotnet SDK no encontrado. Ejecuta ./setup.sh primero." >&2
    exit 1
fi

pushd src >/dev/null
dotnet restore
dotnet build
dotnet test
popd >/dev/null
