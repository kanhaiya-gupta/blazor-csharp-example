# Local CI/CD testing with `act`

Test GitHub Actions workflows (CI, CD, Release) locally using [act](https://github.com/nektos/act) before pushing to GitHub.

## Overview

- **CI** — Build, test, format, Docker build (no push). Fully runnable locally.
- **CD** — Build and push Docker image to GHCR. Build works locally; push uses GitHub token (run on GitHub for real pushes).
- **Release** — Build artifacts, GitHub Release, versioned Docker image. Build job runs locally; release creation and image push are GitHub-only.

## Prerequisites

### 1. Docker

`act` runs jobs in Docker containers. Install and start Docker:

- **Windows:** [Docker Desktop](https://www.docker.com/products/docker-desktop) (WSL2 backend recommended).
- **macOS:** Docker Desktop or `brew install --cask docker`.
- **Linux/WSL:** `sudo apt-get install docker.io && sudo service docker start` (add your user to `docker` group if needed).

Verify:

```bash
docker --version
docker ps
```

### 2. Install act

**Windows (PowerShell as admin, or use WSL):**

```powershell
# Chocolatey
choco install act-cli

# Or use WSL and the Linux installer below
```

**WSL / Linux / macOS:**

```bash
# Official installer
curl -s https://raw.githubusercontent.com/nektos/act/master/install.sh | sudo bash

# macOS with Homebrew
brew install act
```

Check:

```bash
act --version
```

### 3. (Optional) act config

Create `~/.config/act/actrc` or project `.actrc` to use a runner image that matches .NET/Docker needs:

```
-P ubuntu-latest=catthehacker/ubuntu:act-latest
```

First run, act may ask for a default image size; **Medium** is usually enough.

## Basic usage

### List workflows and jobs

From the repo root (e.g. `ExampleProject`):

```bash
act -l
```

Shows workflow names, job names, and which events (e.g. `workflow_dispatch`) each workflow supports.

### Dry-run (no execution)

See what would run without starting containers:

```bash
act workflow_dispatch -W .github/workflows/ci.yml -n
act workflow_dispatch -W .github/workflows/cd.yml --input image_tag=latest -n
act workflow_dispatch -W .github/workflows/release.yml --input version=1.0.0 -n
```

## Testing each workflow

### CI workflow

Runs **Build & Test**, **Code format**, and **Docker build**. No inputs. Best candidate for full local runs.

```bash
# Dry-run
act workflow_dispatch -W .github/workflows/ci.yml -n

# Full run (all jobs)
act workflow_dispatch -W .github/workflows/ci.yml

# Single job
act workflow_dispatch -W .github/workflows/ci.yml -j build-and-test
act workflow_dispatch -W .github/workflows/ci.yml -j format
act workflow_dispatch -W .github/workflows/ci.yml -j docker
```

**Jobs:** `build-and-test`, `format`, `docker`

### CD workflow

Builds the API image and (on GitHub) pushes to GitHub Container Registry. Locally you can validate the build; push may fail without a real GitHub token/permissions.

```bash
# Dry-run
act workflow_dispatch -W .github/workflows/cd.yml --input image_tag=latest -n

# Run (build will run; push may fail locally)
act workflow_dispatch -W .github/workflows/cd.yml --input image_tag=latest
```

**Input:** `image_tag` (default `latest`) — e.g. `staging`, `v1.0.0`.

**Job:** `build-and-push`

### Release workflow

Builds publish artifacts, creates a GitHub Release, and pushes a versioned image. Locally, **build-artifacts** is useful; **release** and **docker** depend on GitHub APIs and token.

```bash
# Dry-run
act workflow_dispatch -W .github/workflows/release.yml --input version=1.0.0 -n

# Run only the build job (validates .NET publish + zip)
act workflow_dispatch -W .github/workflows/release.yml --input version=1.0.0 -j build-artifacts

# Full run (release/docker steps may fail locally)
act workflow_dispatch -W .github/workflows/release.yml --input version=1.0.0
```

**Input (manual trigger):** `version` — e.g. `1.0.0` (no `v` prefix; workflow adds it for the tag).

**Jobs:** `build-artifacts`, `release`, `docker`

**Note:** On GitHub, Release also runs on tag push (e.g. `v1.0.0`). Locally with act you only simulate `workflow_dispatch` with `--input version=1.0.0`.

## Simulating release by tag

To simulate the tag-based trigger (version from tag):

```bash
act push -W .github/workflows/release.yml -e event.json -n
```

Create `event.json` in the repo root:

```json
{
  "ref": "refs/tags/v1.0.0"
}
```

Then:

```bash
act push -W .github/workflows/release.yml -e event.json
```

Use this only if you need to test the “version from tag” path; for most local checks, `workflow_dispatch --input version=1.0.0` is enough.

## Useful options

| Option | Description |
|--------|-------------|
| `-n` | Dry-run: list jobs/steps, don’t run |
| `-j <job>` | Run only this job (can repeat for multiple) |
| `-W <file>` | Workflow file to run |
| `--input key=value` | `workflow_dispatch` input |
| `-v` | Verbose logs |
| `-e .env` | Load env from `.env` file |

Examples:

```bash
# Verbose
act workflow_dispatch -W .github/workflows/ci.yml -v

# Two jobs
act workflow_dispatch -W .github/workflows/ci.yml -j build-and-test -j docker
```

## What works locally vs on GitHub

| Feature | Local (act) | GitHub |
|---------|-------------|--------|
| CI: build, test, format, Docker build | Yes | Yes |
| CD: Docker build | Yes | Yes |
| CD: Push to GHCR | Often no (token/perms) | Yes |
| Release: build-artifacts | Yes | Yes |
| Release: upload-artifact | Limited / different | Yes |
| Release: download-artifact | Limited / different | Yes |
| Release: Create GitHub Release | No | Yes |
| Release: Push image to GHCR | Often no | Yes |

Recommendation: use act to validate **CI** and the **build** parts of CD/Release; do real pushes and release creation on GitHub.

## Quick reference

```bash
# List workflows
act -l

# CI
act workflow_dispatch -W .github/workflows/ci.yml -n
act workflow_dispatch -W .github/workflows/ci.yml
act workflow_dispatch -W .github/workflows/ci.yml -j build-and-test

# CD
act workflow_dispatch -W .github/workflows/cd.yml --input image_tag=latest -n
act workflow_dispatch -W .github/workflows/cd.yml --input image_tag=latest

# Release (manual trigger)
act workflow_dispatch -W .github/workflows/release.yml --input version=1.0.0 -n
act workflow_dispatch -W .github/workflows/release.yml --input version=1.0.0 -j build-artifacts
```

## See also

- [act](https://github.com/nektos/act) — Run GitHub Actions locally
- [CI.md](./CI.md) — Workflow descriptions and triggers
- Root [README.md](../README.md) — Build, test, and run scripts
