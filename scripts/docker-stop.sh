#!/usr/bin/env bash
# docker-stop.sh — Stop the app running in Docker.

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

docker compose down
echo "Docker app stopped."
