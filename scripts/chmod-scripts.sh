#!/usr/bin/env bash
# chmod-scripts.sh — Make all .sh files in scripts/ executable. Run once (e.g. after clone).

set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$SCRIPT_DIR"
chmod +x *.sh
echo "Execute permission set for scripts/*.sh"
