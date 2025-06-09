#!/usr/bin/env bash
set -e

if command -v dotnet >/dev/null; then
    echo "dotnet SDK ya está instalado"
    exit 0
fi

wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-9.0 # or the appropriate preview version
