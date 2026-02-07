#!/usr/bin/env bash
# start-databases.sh — Start only PostgreSQL and MongoDB containers for this project.
# Use this before running the population scripts (population/run_all.py).

set -e
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "Starting PostgreSQL and MongoDB..."
docker compose up -d postgres mongo

echo "Waiting for databases to be ready..."
sleep 3
for i in 1 2 3 4 5; do
  if docker compose exec -T postgres pg_isready -U exampleproject -d exampleproject 2>/dev/null; then
    echo "PostgreSQL is ready."
    break
  fi
  if [ "$i" -eq 5 ]; then echo "PostgreSQL not ready after 15s; population may still work."; fi
  sleep 2
done

echo ""
echo "PostgreSQL: localhost:5432 (user/password/db: exampleproject)"
echo "MongoDB:    localhost:27017"
echo ""
echo "Next: cd population && pip install -r requirements.txt && python run_all.py"
echo "To stop: ./scripts/stop-databases.sh  (or: docker compose stop postgres mongo)"
