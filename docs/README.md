# ExampleProject — Documentation

This folder holds extra documentation and assets for the project.

## Contents

- **images/** — Screenshots and diagrams
  - `Frontend_GUI.png` — Screenshot of the Blazor front-end (home page)
- **CI.md** — GitHub Actions CI/CD (workflows, jobs, triggers)
- **ACT_LOCAL_CICD.md** — Local CI/CD testing with act (CI, CD, Release)
- **README.md** (this file)

## Main docs

- **README.md** (repo root) — Quick start, scripts, structure, Docker, and tests.

## App overview

- **Front-end:** Blazor Server UI with Home, Overview, and Example tasks. Navigation in the top bar.
- **API:** REST endpoint at `/api`; same host as the UI (port 5000).
- **Run:** `./scripts/run.sh` (local) or `./scripts/docker-run.sh` (Docker, detached).
