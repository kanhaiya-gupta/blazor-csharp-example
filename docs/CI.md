# CI/CD — GitHub Actions

The repository uses GitHub Actions for continuous integration, deployment, and releases. All workflows are **manual trigger** (`workflow_dispatch`) unless noted.

## Workflow files

- **`.github/workflows/ci.yml`** — CI: build, test, format, Docker build (manual trigger).
- **`.github/workflows/cd.yml`** — CD: build and push Docker image to GHCR (manual trigger, input: `image_tag`).
- **`.github/workflows/release.yml`** — Release: build artifacts, GitHub Release, versioned Docker image (manual with `version` input, or on tag push `v*`).

## Local testing with act

You can run these workflows locally with [act](https://github.com/nektos/act) before pushing. See **[ACT_LOCAL_CICD.md](./ACT_LOCAL_CICD.md)** for install, commands, and job names.

## Jobs

### 1. Build & Test

- **Runner:** Ubuntu latest
- **Steps:**
  - Checkout
  - Setup .NET 9
  - Cache NuGet packages (by hash of `*.csproj` files)
  - `dotnet restore ExampleProject.sln`
  - `dotnet build ExampleProject.sln -c Release --no-restore`
  - `dotnet test ExampleProject.sln -c Release --no-build`
- **Purpose:** Ensures the solution compiles in Release and all tests pass.

### 2. Code format

- **Runner:** Ubuntu latest
- **Steps:**
  - Checkout
  - Setup .NET 9
  - Install `dotnet-format` globally
  - `dotnet format ExampleProject.sln --verify-no-changes`
- **Purpose:** Fails if C# style doesn’t match the project’s `.editorconfig` (indent, newlines, etc.). Run `dotnet format` locally to fix before pushing.

### 3. Docker build

- **Runner:** Ubuntu latest
- **Steps:**
  - Checkout
  - Docker Buildx
  - Build the image from `src/ExampleProject.Api/Dockerfile` (context: repo root)
- **Purpose:** Ensures the Dockerfile builds and the app can be containerized. Image is not pushed; cache uses GitHub Actions cache.

## Concurrency

- CI uses concurrency so only the latest run per ref is kept; in-progress runs are cancelled when a new one starts.

## Local checks before push

To avoid CI failures:

```bash
./scripts/build.sh
./scripts/test.sh
dotnet format ExampleProject.sln --verify-no-changes   # install: dotnet tool install -g dotnet-format
```

Or run the full CI workflow locally with act:

```bash
act workflow_dispatch -W .github/workflows/ci.yml
```

## Triggers

- **CI / CD / Release** — Run manually from the Actions tab or via act (see [ACT_LOCAL_CICD.md](./ACT_LOCAL_CICD.md)).
- **Release** — Also runs on **tag push** `v*` (e.g. `v1.0.0`); version is taken from the tag.
