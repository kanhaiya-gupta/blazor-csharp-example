# ExampleProject — Documentation

This folder holds extra documentation and assets for the project.

## Contents

- **images/** — Screenshots and diagrams
  - `Frontend_GUI.png` — Screenshot of the Blazor front-end (update this when the GUI changes)
- **CI.md** — GitHub Actions CI/CD (workflows, jobs, triggers)
- **ACT_LOCAL_CICD.md** — Local CI/CD testing with act
- **DATA_STORES.md** — PostgreSQL and MongoDB: tables, collections, config, population, Docker
- **README.md** (this file)

## Main docs

- **README.md** (repo root) — Quick start, scripts, structure, Docker, tests, and storyline.

## App overview

- **Front-end:** Blazor Server UI with:
  - **Home** — Hero and links
  - **Architecture** — Diagram of PostgreSQL tables, MongoDB collections, and data flow
  - **Offers** — Flexibility offers (PostgreSQL)
  - **Plants** — Registered plants (PostgreSQL)
  - **Operations** — Market signals and dispatch log (MongoDB)
  - **Dashboard** — Time-series chart (meter readings per plant), anomaly dots (red), summary cards, recent alerts and meter readings tables
  - **Audit** — Audit log (MongoDB)
- **API:** REST endpoints on the same host (port 5000) for offers, plants, meter readings, alerts, audit, market signals, dispatch log.
- **Run:** `./scripts/run.sh` (local) or `./scripts/docker-run.sh` (Docker, API only).

## Storyline (short)

1. **PostgreSQL** — Relational data: offers, plants, meter readings (time-series MW), alerts (anomalies: spike >3×, threshold, system).
2. **MongoDB** — Document data: audit log, market signals, dispatch log.
3. **Dashboard** — Chart shows one line per plant (names from DB); red dots mark anomalies on the correct plant line; data refreshes every minute.
