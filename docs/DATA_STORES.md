# Data stores — PostgreSQL and MongoDB

The app uses optional **PostgreSQL** (EF Core) and **MongoDB** for persistence. It runs with neither, one, or both; configuration controls which are used.

---

## PostgreSQL (EF Core)

| Table | Purpose | Key fields |
|-------|---------|------------|
| **FlexibilityOffers** | Short-term flexibility offers | Id, Name, Status, CreatedAt |
| **Plants** | Registered plants/assets | Id, Name, AssetType, CapacityMw, Status, RegisteredAt |
| **MeterReadings** | Time-series MW per plant | Id, PlantId, Timestamp, Value, MetricType |
| **Alerts** | Detected anomalies | Id, PlantId, Type, Severity, Message, Timestamp, ResolvedAt |

Anomaly types in seed data: **Spike** (>3× baseline), **ThresholdExceeded**, **System** (e.g. data gap).

**Config:** `ConnectionStrings:DefaultConnection`. Empty or missing ⇒ null repositories; API and UI still run.

---

## MongoDB

| Collection | Purpose | Key fields |
|------------|---------|------------|
| **audit** | Audit log | Id, Action, UserId, Timestamp, Details |
| **market_signals** | Market signals | Id, Market, Timestamp, Price, Volume |
| **dispatch_log** | Dispatch entries | Id, PlantName, Timestamp, VolumeMw, Market, OfferId, Direction |

**Config:** `Mongo:ConnectionString`, `Mongo:DatabaseName`, etc. Empty or missing ⇒ null stores; app still runs.

---

## UI and API

- **Dashboard** — Time-series chart (one line per plant, names from DB), red anomaly dots on the correct plant line, summary cards, recent alerts and meter readings tables; refresh every minute.
- **Architecture** — Single page describing stores and data flow.
- **Offers, Plants, Operations, Audit** — List and CRUD pages for the above data.

REST endpoints: `/api/offers`, `/api/plants`, `/api/meter-readings/recent`, `/api/alerts/recent`, `/api/audit/recent`, market signals, dispatch log.

---

## Population (seed data)

| Script | Purpose |
|--------|--------|
| `population/populate_postgres.py` | Creates tables if missing; seeds all four PostgreSQL tables (industry-themed). |
| `population/populate_mongo.py` | Seeds audit, market_signals, dispatch_log (clear then insert). |
| `population/run_all.py` | Runs both. |

See **[population/README.md](../population/README.md)** for prerequisites, `.env` setup, and order of operations.

**Flow:** Start Postgres and Mongo (e.g. `docker compose up -d postgres mongo`), run the app once so schema exists (or let population create tables), then `cd population && python run_all.py`. Configure the app with the same connection strings.

---

## Docker

- **API:** `./scripts/docker-run.sh` runs only the API container. Use `./scripts/docker-stop.sh` to stop it.
- **Databases:** Run separately, e.g. `docker compose up -d postgres mongo`. From the API container, use host `host.docker.internal` in connection strings to reach them.

---

*Last updated: 2026-02*
