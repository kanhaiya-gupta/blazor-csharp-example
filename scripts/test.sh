#!/usr/bin/env bash
# test.sh — Run all tests. Run from repo root or scripts folder.

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "Running tests..."
dotnet test ExampleProject.sln "$@"
