#!/usr/bin/env python3
"""
Seed MongoDB with sample documents for three collections (MongoDB story):
- audit: who did what (actions)
- market_signals: incoming market/price data (inputs)
- dispatch_log: flexibility dispatch records (outputs)
Database and collection names must match the app's Mongo config.
"""
import os
from datetime import datetime, timezone

from pymongo import MongoClient
from dotenv import load_dotenv

load_dotenv()

def get_client():
    uri = os.getenv("MONGO_URI", "mongodb://localhost:27017")
    return MongoClient(uri)

def get_db(client):
    return client[os.getenv("MONGO_DB", "exampleproject")]

def seed_audit(db):
    coll = db[os.getenv("MONGO_AUDIT_COLLECTION", "audit")]
    docs = [
        {"Action": "PlantRegistered", "UserId": "operator-01", "Timestamp": datetime(2025, 1, 10, 9, 5, 0, tzinfo=timezone.utc), "Details": "CHP plant Musterstadt registered for short-term flexibility"},
        {"Action": "ContractSigned", "UserId": "client-municipal-01", "Timestamp": datetime(2025, 1, 12, 14, 35, 0, tzinfo=timezone.utc), "Details": "Battery storage 2 MW – one-stop flexibility agreement"},
        {"Action": "MarketBidSubmitted", "UserId": "operator-01", "Timestamp": datetime(2025, 1, 14, 11, 15, 0, tzinfo=timezone.utc), "Details": "Day-ahead bid submitted for industrial load"},
        {"Action": "BalancingEnergyDelivered", "UserId": "system", "Timestamp": datetime(2025, 1, 15, 8, 30, 0, tzinfo=timezone.utc), "Details": "Gas turbine 50 MW – balancing energy delivered"},
        {"Action": "PortfolioUpdated", "UserId": "operator-02", "Timestamp": datetime(2025, 1, 16, 10, 10, 0, tzinfo=timezone.utc), "Details": "VPP portfolio extended by 5 MW decentral assets"},
        {"Action": "CO2CertificateIssued", "UserId": "system", "Timestamp": datetime(2025, 1, 17, 9, 0, 0, tzinfo=timezone.utc), "Details": "CO2 savings certified for Q4 flexibility volume"},
    ]
    coll.delete_many({})
    result = coll.insert_many(docs)
    print(f"Inserted {len(result.inserted_ids)} audit documents into {coll.name}.")

def seed_market_signals(db):
    coll_name = os.getenv("MONGO_MARKET_SIGNALS_COLLECTION", "market_signals")
    coll = db[coll_name]
    docs = [
        {"Timestamp": datetime(2025, 1, 15, 6, 0, 0, tzinfo=timezone.utc), "Market": "DayAhead", "PriceEurPerMwh": 72.50, "VolumeMw": 100.0, "Region": "DE"},
        {"Timestamp": datetime(2025, 1, 15, 12, 0, 0, tzinfo=timezone.utc), "Market": "DayAhead", "PriceEurPerMwh": 85.20, "VolumeMw": 80.0, "Region": "DE"},
        {"Timestamp": datetime(2025, 1, 15, 18, 0, 0, tzinfo=timezone.utc), "Market": "DayAhead", "PriceEurPerMwh": 92.00, "VolumeMw": 120.0, "Region": "DE"},
        {"Timestamp": datetime(2025, 1, 16, 8, 0, 0, tzinfo=timezone.utc), "Market": "Intraday", "PriceEurPerMwh": 68.30, "VolumeMw": 15.0, "Region": "DE"},
        {"Timestamp": datetime(2025, 1, 16, 14, 0, 0, tzinfo=timezone.utc), "Market": "Balancing", "PriceEurPerMwh": 105.00, "VolumeMw": 25.0, "Region": "DE"},
        {"Timestamp": datetime(2025, 1, 17, 7, 0, 0, tzinfo=timezone.utc), "Market": "DayAhead", "PriceEurPerMwh": 78.40, "VolumeMw": 90.0, "Region": "DE"},
        {"Timestamp": datetime(2025, 1, 17, 10, 0, 0, tzinfo=timezone.utc), "Market": "Intraday", "PriceEurPerMwh": 81.00, "VolumeMw": 10.0, "Region": "DE"},
    ]
    coll.delete_many({})
    result = coll.insert_many(docs)
    print(f"Inserted {len(result.inserted_ids)} market signal documents into {coll.name}.")

def seed_dispatch_log(db):
    coll_name = os.getenv("MONGO_DISPATCH_LOG_COLLECTION", "dispatch_log")
    coll = db[coll_name]
    docs = [
        {"Timestamp": datetime(2025, 1, 15, 7, 15, 0, tzinfo=timezone.utc), "PlantName": "CHP Musterstadt", "VolumeMw": 2.5, "Market": "DayAhead", "Direction": "Up"},
        {"Timestamp": datetime(2025, 1, 15, 13, 30, 0, tzinfo=timezone.utc), "PlantName": "Battery North 2 MW", "VolumeMw": 1.8, "Market": "Intraday", "Direction": "Down"},
        {"Timestamp": datetime(2025, 1, 16, 9, 0, 0, tzinfo=timezone.utc), "PlantName": "Gas turbine Site A", "VolumeMw": 12.0, "Market": "Balancing", "Direction": "Up"},
        {"Timestamp": datetime(2025, 1, 16, 15, 45, 0, tzinfo=timezone.utc), "PlantName": "Steel mill North – load", "VolumeMw": 5.0, "Market": "DayAhead", "Direction": "Down"},
        {"Timestamp": datetime(2025, 1, 17, 8, 20, 0, tzinfo=timezone.utc), "PlantName": "VPP portfolio 15 MW", "VolumeMw": 3.0, "Market": "Intraday", "Direction": "Up"},
        {"Timestamp": datetime(2025, 1, 17, 11, 0, 0, tzinfo=timezone.utc), "PlantName": "Power-to-heat Stadtwerke", "VolumeMw": 1.2, "Market": "DayAhead", "Direction": "Down"},
    ]
    coll.delete_many({})
    result = coll.insert_many(docs)
    print(f"Inserted {len(result.inserted_ids)} dispatch log documents into {coll.name}.")

def main():
    print("Connecting to MongoDB...")
    client = get_client()
    try:
        db = get_db(client)
        seed_audit(db)
        seed_market_signals(db)
        seed_dispatch_log(db)
    finally:
        client.close()
    print("MongoDB population done.")

if __name__ == "__main__":
    main()
