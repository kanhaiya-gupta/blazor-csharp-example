#!/usr/bin/env bash
# build.sh — Build the solution. Run from repo root or scripts folder.

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "Building solution..."
dotnet build ExampleProject.sln "$@"
