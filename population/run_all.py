#!/usr/bin/env python3
"""Run PostgreSQL and MongoDB population scripts in order."""
import subprocess
import sys

scripts = ["populate_postgres.py", "populate_mongo.py"]
for script in scripts:
    print(f"\n--- {script} ---")
    rc = subprocess.call([sys.executable, script])
    if rc != 0:
        print(f"{script} failed with exit code {rc}", file=sys.stderr)
        sys.exit(rc)
print("\nAll population scripts completed.")
