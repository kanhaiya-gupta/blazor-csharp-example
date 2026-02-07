# Database population scripts

Python scripts that seed **PostgreSQL** and **MongoDB** with sample data. The .NET app uses the same databases via connection strings.

## Why Python

- Simple, one-off scripts; no need to run the .NET app or EF migrations from the seed step.
- Mature drivers: `psycopg` (PostgreSQL) and `pymongo` (MongoDB).
- Run after databases are up; the app reads the same data.

## Prerequisites

- **Python 3.10+**
- **Docker** (optional, for running PostgreSQL and MongoDB).
- **PostgreSQL** and **MongoDB** running. From the repo root:
  ```bash
  docker compose up -d postgres mongo
  ```
  To stop later: `docker compose stop postgres mongo`.
- PostgreSQL schema: the app’s `EnsureCreated()` or migrations create the tables; the scripts also create tables if missing (see below).

## Setup

```bash
cd population
python -m venv .venv
# Windows:
.venv\Scripts\activate
# Linux/macOS:
source .venv/bin/activate
pip install -r requirements.txt
cp .env.example .env
# Edit .env with your connection details (or keep defaults for local Docker).
```

If you get `ModuleNotFoundError` for `pymongo` or `psycopg`, run: `pip install -r requirements.txt` (from `population/` with venv active).

## Configuration

Copy `.env.example` to `.env` and set:

| Variable | Description |
|----------|-------------|
| `POSTGRES_HOST`, `POSTGRES_PORT`, `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB` | PostgreSQL connection. |
| `MONGO_URI`, `MONGO_DB`, `MONGO_AUDIT_COLLECTION` | MongoDB connection and audit collection name. |

The app must use the **same** connection strings when you want it to use this data (see root `README.md` and `appsettings.*.json`).

## Run

```bash
# Activate venv first, then:
python populate_postgres.py
python populate_mongo.py
```

Or both:

```bash
python run_all.py
```

- **PostgreSQL** — Creates tables if missing; inserts into **FlexibilityOffers**, **Plants**, **MeterReadings**, **Alerts**. Script clears and re-seeds for idempotency. Meter readings are hourly (3 days × 3 plants); alerts use anomaly types (Spike >3×, ThresholdExceeded, System).
- **MongoDB** — Inserts into **audit**, **market_signals**, **dispatch_log**. Clear-then-insert for idempotency.

## Order of operations (full stack)

1. Start databases: from repo root, **`docker compose up -d postgres mongo`**.
2. Start the app once (or run migrations) so PostgreSQL schema exists; or let the population script create tables.
3. Populate: `cd population && pip install -r requirements.txt && python run_all.py`.
4. Run the app with connection strings pointing at the same Postgres and Mongo; the Dashboard and other pages will use the populated data.

## Schema alignment

**PostgreSQL (populate_postgres.py):**

| Table | Key columns |
|-------|-------------|
| FlexibilityOffers | Id, Name, Status, CreatedAt |
| Plants | Id, Name, AssetType, CapacityMw, Status, RegisteredAt |
| MeterReadings | Id, PlantId, Timestamp, Value, MetricType |
| Alerts | Id, PlantId, Type, Severity, Message, Timestamp, ResolvedAt |

Must match the app’s entities and EF Core configuration.

**MongoDB (populate_mongo.py):**

- **Audit** — Action, UserId, Timestamp, Details (and Id).
- **market_signals** — Market, Timestamp, Price, Volume (and Id).
- **dispatch_log** — PlantName, Timestamp, VolumeMw, Market, OfferId, Direction (and Id).

Collection names and field shapes must match what the app’s stores expect.

See **docs/DATA_STORES.md** for the app’s data model and data stores.
