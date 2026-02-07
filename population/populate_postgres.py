#!/usr/bin/env python3
"""
Seed PostgreSQL with sample data for four tables (PostgreSQL story):
- FlexibilityOffers (Id, Name, Status, CreatedAt)
- Plants (Id, Name, AssetType, CapacityMw, Status, RegisteredAt)
- MeterReadings (Id, PlantId, Timestamp, Value, MetricType) – time-series for anomaly detection
- Alerts (Id, PlantId, Type, Severity, Message, Timestamp, ResolvedAt) – detected anomalies
Creates tables if they do not exist, then inserts sample rows.
"""
import os
import uuid
from datetime import datetime, timezone

import psycopg
from dotenv import load_dotenv

load_dotenv()

def get_connection_string():
    return (
        f"host={os.getenv('POSTGRES_HOST', 'localhost')} "
        f"port={os.getenv('POSTGRES_PORT', '5432')} "
        f"user={os.getenv('POSTGRES_USER', 'exampleproject')} "
        f"password={os.getenv('POSTGRES_PASSWORD', 'exampleproject')} "
        f"dbname={os.getenv('POSTGRES_DB', 'exampleproject')}"
    )

def ensure_tables(conn):
    with conn.cursor() as cur:
        cur.execute("""
            CREATE TABLE IF NOT EXISTS "FlexibilityOffers" (
                "Id" uuid PRIMARY KEY,
                "Name" text NOT NULL,
                "Status" text NOT NULL,
                "CreatedAt" timestamp with time zone NOT NULL
            );
        """)
        cur.execute("""
            CREATE TABLE IF NOT EXISTS "Plants" (
                "Id" uuid PRIMARY KEY,
                "Name" text NOT NULL,
                "AssetType" text NOT NULL,
                "CapacityMw" numeric(10,2) NULL,
                "Status" text NOT NULL,
                "RegisteredAt" timestamp with time zone NOT NULL
            );
        """)
        cur.execute("""
            CREATE TABLE IF NOT EXISTS "MeterReadings" (
                "Id" uuid PRIMARY KEY,
                "PlantId" uuid NULL,
                "Timestamp" timestamp with time zone NOT NULL,
                "Value" numeric(10,2) NOT NULL,
                "MetricType" text NULL
            );
        """)
        cur.execute("""
            CREATE TABLE IF NOT EXISTS "Alerts" (
                "Id" uuid PRIMARY KEY,
                "PlantId" uuid NULL,
                "Type" text NOT NULL,
                "Severity" text NOT NULL,
                "Message" text NOT NULL,
                "Timestamp" timestamp with time zone NOT NULL,
                "ResolvedAt" timestamp with time zone NULL
            );
        """)
    conn.commit()

def seed(conn):
    # Flexibility offers: short-term marketing of electricity/gas flexibility (industry, municipal, battery, VPP, etc.)
    rows = [
        ("CHP plant flexibility – Stadtwerke Musterstadt", "Active", datetime(2025, 1, 10, 9, 0, 0, tzinfo=timezone.utc)),
        ("Battery storage 2 MW – Cross-market optimization", "Active", datetime(2025, 1, 12, 14, 30, 0, tzinfo=timezone.utc)),
        ("Industrial load management – Steel mill North", "Active", datetime(2025, 1, 14, 11, 0, 0, tzinfo=timezone.utc)),
        ("Biogas flexibility – North region cluster", "Pending", datetime(2025, 1, 15, 8, 0, 0, tzinfo=timezone.utc)),
        ("VPP aggregation – 15 MW decentral portfolio", "Active", datetime(2025, 1, 16, 10, 0, 0, tzinfo=timezone.utc)),
        ("Gas turbine 50 MW – Balancing energy", "Completed", datetime(2025, 1, 8, 16, 0, 0, tzinfo=timezone.utc)),
        ("Power-to-heat municipal utility – Day-ahead", "Active", datetime(2025, 1, 17, 9, 30, 0, tzinfo=timezone.utc)),
    ]
    with conn.cursor() as cur:
        cur.execute('DELETE FROM "FlexibilityOffers";')  # Idempotent: clear then insert
        for name, status, created_at in rows:
            id_ = uuid.uuid4()
            cur.execute(
                """
                INSERT INTO "FlexibilityOffers" ("Id", "Name", "Status", "CreatedAt")
                VALUES (%s, %s, %s, %s);
                """,
                (id_, name, status, created_at),
            )
    conn.commit()
    print(f"Inserted {len(rows)} sample rows into FlexibilityOffers.")


def seed_plants(conn):
    # Registered flexibility assets (CHP, battery, turbine, industrial, VPP, etc.)
    rows = [
        ("CHP Musterstadt", "CHP", 4.5, "Active", datetime(2025, 1, 9, 10, 0, 0, tzinfo=timezone.utc)),
        ("Battery North 2 MW", "Battery", 2.0, "Active", datetime(2025, 1, 11, 14, 0, 0, tzinfo=timezone.utc)),
        ("Steel mill North – load", "IndustrialLoad", 12.0, "Active", datetime(2025, 1, 13, 9, 0, 0, tzinfo=timezone.utc)),
        ("Biogas cluster North", "Biogas", 1.5, "Pending", datetime(2025, 1, 14, 11, 0, 0, tzinfo=timezone.utc)),
        ("VPP portfolio 15 MW", "VPP", 15.0, "Active", datetime(2025, 1, 15, 8, 0, 0, tzinfo=timezone.utc)),
        ("Gas turbine Site A", "GasTurbine", 50.0, "Active", datetime(2025, 1, 7, 16, 0, 0, tzinfo=timezone.utc)),
        ("Power-to-heat Stadtwerke", "PowerToHeat", 3.0, "Active", datetime(2025, 1, 16, 10, 30, 0, tzinfo=timezone.utc)),
    ]
    with conn.cursor() as cur:
        cur.execute('DELETE FROM "Plants";')
        for name, asset_type, capacity_mw, status, registered_at in rows:
            id_ = uuid.uuid4()
            cur.execute(
                """
                INSERT INTO "Plants" ("Id", "Name", "AssetType", "CapacityMw", "Status", "RegisteredAt")
                VALUES (%s, %s, %s, %s, %s, %s);
                """,
                (id_, name, asset_type, capacity_mw, status, registered_at),
            )
    conn.commit()
    print(f"Inserted {len(rows)} sample rows into Plants.")


def seed_meter_readings(conn):
    # Time-series readings (MW or kWh) per plant – data source for anomaly detection
    with conn.cursor() as cur:
        cur.execute('SELECT "Id" FROM "Plants" ORDER BY "RegisteredAt" LIMIT 3')
        plant_ids = [row[0] for row in cur.fetchall()]
    if not plant_ids:
        print("No plants found; skipping MeterReadings.")
        return
    # Simulate hourly readings for a few days (one reading per plant per hour)
    rows = []
    base = datetime(2025, 1, 14, 0, 0, 0, tzinfo=timezone.utc)
    for hour in range(24 * 3):  # 3 days
        ts = base.replace(hour=hour % 24, day=14 + hour // 24)
        for i, pid in enumerate(plant_ids):
            # Slight variation: plant 0 ~2–4 MW, plant 1 ~1–2 MW, plant 2 ~8–14 MW
            value = (2.0 + (i * 3) + (hour % 10) * 0.2) if i < 2 else (8.0 + (hour % 6) + (i * 0.5))
            rows.append((uuid.uuid4(), pid, ts, round(value, 2), "MW"))
    with conn.cursor() as cur:
        cur.execute('DELETE FROM "MeterReadings";')
        for id_, plant_id, ts, value, metric in rows:
            cur.execute(
                """
                INSERT INTO "MeterReadings" ("Id", "PlantId", "Timestamp", "Value", "MetricType")
                VALUES (%s, %s, %s, %s, %s);
                """,
                (id_, plant_id, ts, value, metric),
            )
    conn.commit()
    print(f"Inserted {len(rows)} sample rows into MeterReadings.")


def seed_alerts(conn):
    # Detected anomalies (spike, threshold exceeded, etc.)
    with conn.cursor() as cur:
        cur.execute('SELECT "Id" FROM "Plants" ORDER BY "RegisteredAt" LIMIT 2')
        plant_ids = [row[0] for row in cur.fetchall()]
    plant_id_1 = plant_ids[0] if plant_ids else None
    plant_id_2 = plant_ids[1] if len(plant_ids) > 1 else None
    rows = [
        (plant_id_1, "Spike", "High", "Consumption > 3× rolling average (plant)", datetime(2025, 1, 15, 14, 30, 0, tzinfo=timezone.utc), None),
        (plant_id_2, "ThresholdExceeded", "Medium", "Power export exceeded 1.8 MW", datetime(2025, 1, 16, 9, 0, 0, tzinfo=timezone.utc), None),
        (None, "System", "Low", "Data gap: missing readings for 1 interval", datetime(2025, 1, 16, 12, 0, 0, tzinfo=timezone.utc), datetime(2025, 1, 16, 13, 0, 0, tzinfo=timezone.utc)),
        (plant_id_1, "Spike", "Medium", "Load > 3× baseline (short spike)", datetime(2025, 1, 17, 8, 15, 0, tzinfo=timezone.utc), None),
    ]
    with conn.cursor() as cur:
        cur.execute('DELETE FROM "Alerts";')
        for plant_id, atype, severity, message, ts, resolved in rows:
            id_ = uuid.uuid4()
            cur.execute(
                """
                INSERT INTO "Alerts" ("Id", "PlantId", "Type", "Severity", "Message", "Timestamp", "ResolvedAt")
                VALUES (%s, %s, %s, %s, %s, %s, %s);
                """,
                (id_, plant_id, atype, severity, message, ts, resolved),
            )
    conn.commit()
    print(f"Inserted {len(rows)} sample rows into Alerts.")


def main():
    conn_str = get_connection_string()
    print("Connecting to PostgreSQL...")
    with psycopg.connect(conn_str) as conn:
        ensure_tables(conn)
        seed(conn)
        seed_plants(conn)
        seed_meter_readings(conn)
        seed_alerts(conn)
    print("PostgreSQL population done.")

if __name__ == "__main__":
    main()
