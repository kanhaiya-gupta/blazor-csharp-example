#!/usr/bin/env bash
# run.sh — Run the app (front-end + API on one host). Run from repo root or scripts folder.

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "Starting ExampleProject (GUI + API at http://localhost:5000, Ctrl+C to stop)..."
dotnet run --project src/ExampleProject.Api/ExampleProject.Api.csproj "$@"
