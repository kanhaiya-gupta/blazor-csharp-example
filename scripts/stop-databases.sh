#!/usr/bin/env bash
# stop-databases.sh — Stop only PostgreSQL and MongoDB containers.

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

docker compose stop postgres mongo
echo "PostgreSQL and MongoDB stopped."
