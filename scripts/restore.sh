#!/usr/bin/env bash
# restore.sh — Restore NuGet packages for the solution. Run from repo root or scripts folder.

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "Restoring packages..."
dotnet restore ExampleProject.sln "$@"
