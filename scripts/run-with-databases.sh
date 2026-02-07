#!/usr/bin/env bash
# run-with-databases.sh — Run the app with Postgres + Mongo (Development config). Starts DBs only if not already running.

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

# Start Postgres and Mongo only if not already running
if ! docker compose exec -T postgres pg_isready -U exampleproject -d exampleproject 2>/dev/null; then
  echo "Starting PostgreSQL and MongoDB..."
  "$ROOT/scripts/start-databases.sh"
else
  echo "PostgreSQL and MongoDB already running."
fi

export ASPNETCORE_ENVIRONMENT=Development
echo "Starting ExampleProject with databases (GUI + API at http://localhost:5000, Ctrl+C to stop)..."
dotnet run --project src/ExampleProject.Api/ExampleProject.Api.csproj "$@"
