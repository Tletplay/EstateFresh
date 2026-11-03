# Estate Fresh

A premium laundry pickup and delivery platform built for residents of Zimbali Coastal Resort in Ballito. Customers book a pickup online, a driver collects and weighs their laundry, and the system handles the full lifecycle — payment, processing, delivery, and balance settlement — without the driver ever handling cash.

Built as a full-stack ASP.NET Core MVC application with SQL Server, demonstrating end-to-end CRUD, role-based access control, third-party payment integration, background services, and live location tracking.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture Decisions](#architecture-decisions)
- [Getting Started](#getting-started)
- [Test Accounts](#test-accounts)
- [Known Limitations & Production Considerations](#known-limitations--production-considerations)
- [Deployment Status](#deployment-status)

---

## Features

All features below are fully built and tested end-to-end.

### Phase 1 — Core Customer & Driver Flow
| Feature | Description |
|---|---|
| Account management | Registration with email verification, login, forgot/reset password |
| Book Pickup | Service selection, delivery slot, sneaker add-on, gate access permission |
| Deposit payment | R50 deposit via PayFast sandbox |
| Driver dashboard | View assigned jobs, record actual weight, update delivery status |
| Order tracking | Customers see live status, weight, and balance on their orders |
| Balance payment | Remaining balance settled via PayFast after delivery |

### Phase 2 — Manager & Admin Operations
| Feature | Description |
|---|---|
| Auto driver assignment | System assigns the next available driver the moment a deposit clears |
| Weight dispute resolution | Customers can dispute a recorded weight; a Manager reviews and resolves it, recalculating the balance if corrected |
| Missed pickup & storage | Two failed pickup attempts move an order into storage, accruing a daily fee |
| Unclaimed laundry donation | Admin can mark storage orders as donated once they pass 60 days (server-side enforced) |
| User management | Admin can view, filter by role, and change role/active status for any user |
| Service management | Admin can create, edit, and delete laundry services (blocked if services are in use) |
| Reports dashboard | Manager sees order volume, revenue by service, order status breakdown, and pending disputes |

### Phase 3 — Standout Features
| Feature | Description |
|---|---|
| Live driver tracking | While an order is "Out for Delivery," the driver's live GPS location streams to the customer's map in real time |
| ETA calculation | Real driving time/distance calculated via a free public routing API |
| Automatic balance reminders | A background service emails customers with an unpaid balance, 24 hours after delivery, up to 3 times |
| Weather warnings | Customers and drivers see a warning banner if rain is forecast for a pickup date |

---

## Tech Stack

- **Framework:** ASP.NET Core MVC (.NET 9)
- **ORM / Database:** Entity Framework Core + SQL Server (LocalDB for local development)
- **Authentication:** Session-based, with BCrypt password hashing
- **Payments:** PayFast (sandbox)
- **Email:** Gmail SMTP
- **Weather:** OpenWeatherMap API (free tier)
- **Live mapping:** Leaflet.js + OpenStreetMap tiles (no paid API key required)
- **Routing / ETA:** OSRM (Open Source Routing Machine) public demo server
- **Background jobs:** .NET `BackgroundService` for scheduled balance reminders

---

## Architecture Decisions

A few notable engineering decisions made during development:

- **No paid mapping API.** Live driver tracking uses Leaflet.js with free OpenStreetMap tiles and the free OSRM routing API, rather than Google Maps — avoiding a billing-account requirement while still delivering a real, working live map with ETA.
- **ETA is calculated to the estate entrance, not individual addresses.** The system doesn't store per-customer delivery coordinates (all customers share the Zimbali estate address), so ETA reflects driving time to the estate itself — an honest reflection of what the system actually knows, and still practically useful.
- **Idempotent seed data.** `Data/SeedData.cs` holds a set of `Ensure...Async` methods that check for existing data before inserting, so the full test environment (driver, admin, manager, and sample orders at every stage of the lifecycle) rebuilds itself safely on every app start.
- **Server-side enforcement of business rules.** Time- and state-based rules (the 60-day donation eligibility, the 2-attempt storage trigger, the 24-hour reminder gap) are all re-validated server-side at the moment of action, not just hidden/disabled in the UI — preventing a user from bypassing a rule by manipulating the client.

---

## Getting Started

### Prerequisites
- Visual Studio 2022
- .NET 9 SDK
- SQL Server LocalDB (included with Visual Studio)

### Setup

1. Clone the repository and open `EstateFresh.sln` in Visual Studio.

2. Configure your local secrets. Right-click the project → **Manage User Secrets**, and add:
   ```json
   {
     "OpenWeatherMap": {
       "ApiKey": "<your OpenWeatherMap API key>"
     },
     "EmailSettings": {
       "SenderPassword": "<your Gmail app password>"
     }
   }
   ```
   (`appsettings.json` intentionally ships with these fields empty — User Secrets keeps real credentials out of source control.)

3. Apply the database migrations (Package Manager Console):
   ```
   Update-Database
   ```

4. Run the app (F5). On first launch, `Program.cs` automatically seeds:
   - The next 14 days of delivery slots
   - Test Driver, Admin, and Manager accounts
   - A handful of test orders at various stages of the lifecycle, so every feature can be tested immediately without manually stepping through the full booking flow

### Testing Payments

PayFast's sandbox is used for both deposit and balance payments. **Local development note:** PayFast's real payment confirmation normally arrives via a server-to-server callback, which can't reach `localhost`. As a documented, clearly-marked workaround, the app currently treats a successful redirect back from PayFast as payment confirmation during local testing (see `PaymentController.cs`, marked with `TEMPORARY DEV-ONLY SHORTCUT` comments). This must be replaced with proper server-to-server validation before accepting real payments in production.

---

## Test Accounts

| Role | Email | Password |
|---|---|---|
| Driver | driver@estatefresh.com | driver123 |
| Admin | admin@estatefresh.com | admin123 |
| Manager | manager@estatefresh.com | manager123 |
| Customer | (register your own, or use the seeded test customer in the database) | — |

---

## Known Limitations & Production Considerations

This project was built and tested thoroughly for its intended scope, but a few things are explicitly flagged as **not production-ready as-is**:

- **PayFast local dev shortcut** — see above. Needs a public deployment (or an `ngrok` tunnel during further local testing) and proper server-to-server ITN validation before real payments.
- **OSRM public routing server** — the free demo server used for ETA calculation is suitable for development/testing only; it has no uptime guarantee and should be self-hosted or swapped for a commercial provider before go-live.
- **OpenWeatherMap API key** — currently configured via User Secrets for local development. In a real deployment, this would move to Azure App Service's Application Settings (or another secrets manager), never committed to source control.

---

## Deployment Status

This application is fully built, tested, and **deployment-ready** for Azure App Service + Azure SQL Database. Deployment setup was carried through (Azure SQL Database creation, migration strategy, secrets configuration) but could not be completed to a live, publicly browsable URL because the Azure for Students subscription's free credit had already been exhausted by other coursework on the same account.

The application runs correctly against Azure SQL once a connection string is supplied, and all configuration (connection strings, API keys, email credentials) is already structured for environment-based configuration via Azure App Settings rather than hardcoded values — the only blocker to a live deployment was available cloud credit, not application readiness.
