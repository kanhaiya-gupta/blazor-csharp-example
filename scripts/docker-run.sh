#!/usr/bin/env bash
# docker-run.sh — Build and run the app in Docker in the background (GUI + API at http://localhost:5000).

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "Building and starting the app in Docker (detached)..."
docker compose up --build -d
echo "App is running at http://localhost:5000 (terminal is free)."
echo "To stop: docker compose down  (or ./scripts/docker-stop.sh)"
echo "To view logs: docker compose logs -f"
