# Changelog

All notable changes to this project will be documented in this file.
Format: [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

---

## [1.0.0] — 2026-03-14

### Added
- **Domain layer** — Resource, Alert, AlertThreshold entities with
  encapsulated business rules and domain events
- **Infrastructure layer** — EF Core persistence with Fluent API,
  SQL Server, repository pattern, Unit of Work
- **Application layer** — CQRS with MediatR: 3 queries, 3 commands,
  1 domain event handler (auto-alert on threshold breach)
- **API layer** — REST controllers for Resources and Alerts,
  global exception middleware (RFC 7807), Swagger at root
- **Real-time layer** — SignalR hub with role-based groups,
  push notifications on status change and alert creation
- **Frontend** — Blazor WASM dashboard with live resource grid
  and alert panel, REST initial load + SignalR delta updates
- **Infrastructure** — Docker multi-stage builds, full Compose stack
  (SQL Server + API + Nginx/Blazor), automatic migrations on startup
- **CI/CD** — GitHub Actions pipeline (build + test on push and PR)
- **Seed data** — 6 resources with configured alert thresholds

### Technical decisions
- Clean Architecture with strict layer dependency rules
- Domain events dispatched via MediatR IPublisher after persistence
- SignalR groups by role (Admin / Operator / Viewer / All)
- Critical alerts routed to Admins group only
- Enums stored as strings in database for readability
- Non-root user in production Docker image

---

## [Unreleased]

### Planned for v2.0
- JWT authentication with role claims
- Resource status history and audit trail endpoint
- Export dashboard data to CSV
- Dark/light theme toggle
- Integration tests with Testcontainers