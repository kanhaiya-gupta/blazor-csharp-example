# ExampleProject

A **C#** .NET 9 microservice with a **Blazor** front-end and REST API in a single app. It demonstrates **PostgreSQL** for relational data and **MongoDB** for document-based data, with a **Dashboard** for time-series and anomaly detection.

## What this project does

1. **Start the databases** (PostgreSQL and MongoDB), e.g. `docker compose up -d postgres mongo`.
2. **Populate the data** — Run the Python scripts in `population/` to seed sample offers, plants, meter readings, alerts, audit entries, market signals, and dispatch log.
3. **Run the app** — Start the Blazor + API server and open http://localhost:5000.
4. **Use the UI** — **Dashboard** shows a time-series chart (one line per plant), anomaly markers, summary cards, and recent alerts/readings. **Offers**, **Plants**, **Operations**, and **Audit** pages list and manage the seeded data. **Architecture** describes the data stores and flow.

You can run the app **locally** (`.NET` + `./scripts/run.sh`) or **in Docker** (`./scripts/docker-run.sh`); the C# framework (Blazor + API) runs in a container. Databases (PostgreSQL, MongoDB) can run separately, e.g. via `docker compose up -d postgres mongo`.

Without databases or population, the app still runs; the Dashboard and data pages are just empty or use in-memory stubs.

![Front-end](docs/images/Frontend_GUI.png)

## Storyline

- **PostgreSQL** — Flexibility offers, plants, **meter readings** (time-series MW per plant), and **alerts** (detected anomalies: spike >3× baseline, threshold exceeded, system). Seeded via `population/populate_postgres.py`.
- **MongoDB** — Audit log, market signals, and dispatch log (operations story). Seeded via `population/populate_mongo.py`.
- **Dashboard** — Time-series chart (one line per plant, plant names from DB), red dots for anomalies on the correct plant line, summary cards, and tables for recent alerts and meter readings. Updates every minute.
- **Architecture** — Single page describing stores (PostgreSQL tables, MongoDB collections) and data flow.

## Features

- **Single app** — Blazor UI and API on one host (port 5000)
- **Layered design** — Core (domain), Infrastructure (persistence), Api (Blazor + endpoints)
- **Blazor Server** — Home, Architecture, Offers, Plants, Operations, Dashboard, Audit (no Overview or Tasks)
- **REST API** — Endpoints for offers, plants, meter readings, alerts, audit, market signals, dispatch log
- **Dual data stores** — Optional **PostgreSQL** (EF Core) and **MongoDB**; app runs with neither, one, or both via config
- **Tests** — xUnit unit and integration tests (Core, Infrastructure, Api)
- **Docker** — The C# app (Blazor + API) runs in a container (`docker-run.sh`); databases (PostgreSQL, MongoDB) run separately (e.g. `docker compose up -d postgres mongo`)
- **Population** — Python scripts in `population/` seed sample data (see [population/README.md](population/README.md) and [docs/DATA_STORES.md](docs/DATA_STORES.md))

## Tech stack

| Layer | Project | Role |
|-------|---------|------|
| API & UI | **ExampleProject.Api** | Blazor Server + minimal API (single host) |
| Domain | **ExampleProject.Core** | Entities, interfaces |
| Data | **ExampleProject.Infrastructure** | EF Core (PostgreSQL), MongoDB, repositories/stores |

**C#** · **.NET 9** · **Blazor Server** · **Entity Framework Core** · **PostgreSQL** · **MongoDB** · **xUnit** · **Docker**

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (C#)
- **Git** (for scripts; on Windows use **Git Bash**)
- **Docker** (optional, for containerized run)
- **PostgreSQL** and **MongoDB** (optional; for full data and Dashboard chart)

## Quick start

**1. Clone and build**

```bash
git clone <repo-url>
cd ExampleProject
./scripts/build.sh
```

**2. Run the app**

```bash
./scripts/run.sh
```

Open **http://localhost:5000** — you’ll see the Blazor UI (Home, Architecture, Offers, Plants, Operations, Dashboard, Audit).

**3. With data (Dashboard chart and tables)**

Start PostgreSQL and MongoDB (e.g. `docker compose up -d postgres mongo`), run the population scripts, then run the app with connection strings set (see [population/README.md](population/README.md)).

**4. Run tests**

```bash
./scripts/test.sh
```

**5. Run the app (framework) in Docker**

```bash
./scripts/docker-run.sh
# Stop when done:
./scripts/docker-stop.sh
```

The **C# app** (Blazor + API) runs in a single container. Databases (PostgreSQL, MongoDB) are not included; start them separately (e.g. `docker compose up -d postgres mongo`) and point the app at them (e.g. `host.docker.internal`) if you want the Dashboard to show data.

## Scripts

Run from the repo root (use **Git Bash** on Windows).

| Script | Purpose |
|--------|--------|
| `scripts/build.sh` | Build the solution |
| `scripts/run.sh` | Run the app (GUI + API at http://localhost:5000) |
| `scripts/docker-run.sh` | Run the API in Docker (detached) |
| `scripts/docker-stop.sh` | Stop the Docker API container |
| `scripts/test.sh` | Run all tests |
| `scripts/restore.sh` | Restore NuGet packages |
| `scripts/chmod-scripts.sh` | Make all scripts executable (run once after clone) |

First time: `chmod +x scripts/*.sh` or `bash scripts/chmod-scripts.sh`.

## Project structure

```
ExampleProject/
├── .github/
│   └── workflows/                  # CI, CD, release (ci.yml, cd.yml, release.yml)
├── src/
│   ├── ExampleProject.Api/          # Blazor + API (entry point)
│   │   ├── Components/              # Pages (Home, Architecture, Dashboard, etc.), layout, shared
│   │   ├── wwwroot/                 # Static assets (CSS)
│   │   └── Program.cs
│   ├── ExampleProject.Core/         # Entities, interfaces
│   └── ExampleProject.Infrastructure/  # EF Core, MongoDB, repositories/stores
├── tests/
│   ├── ExampleProject.Api.Tests/
│   ├── ExampleProject.Core.Tests/
│   └── ExampleProject.Infrastructure.Tests/
├── docs/
│   ├── images/                      # Screenshots (e.g. Frontend_GUI.png)
│   ├── CI.md, ACT_LOCAL_CICD.md
│   ├── DATA_STORES.md
│   └── README.md
├── population/                      # Python scripts to seed PostgreSQL and MongoDB
├── scripts/
├── docker-compose.yml
└── ExampleProject.sln
```

## Documentation

- **[docs/](docs/)** — Extra docs, CI, and database plan
- **docs/images/** — UI screenshots (update `Frontend_GUI.png` for the latest GUI)
- **[population/README.md](population/README.md)** — How to run population scripts and align schema

## Docker

The API runs in a single container (port 8080 inside, mapped to 5000). Start databases separately if needed (e.g. `docker compose up -d postgres mongo`). To use them from the container, set connection strings to `host.docker.internal` for Mongo/Postgres host.

```bash
./scripts/docker-run.sh    # Build and start API (detached)
./scripts/docker-stop.sh   # Stop API
docker compose logs -f api # View API logs
```

## Line endings (WSL)

If you see `$'\r': command not found` in WSL, fix scripts once:

```bash
sed -i 's/\r$//' scripts/*.sh
chmod +x scripts/*.sh
```

## License

Use as a learning or portfolio project as you see fit.
