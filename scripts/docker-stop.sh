#!/usr/bin/env bash
# docker-stop.sh — Stop only the API container (leave Postgres and Mongo running).

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

docker compose stop api
echo "Docker API stopped. Postgres and Mongo are still running (use ./scripts/stop-databases.sh to stop them)."
