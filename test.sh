#!/usr/bin/env bash
set -e
dotnet restore
dotnet build
dotnet test
