# ADR 004 — Blazor WASM over React/Angular

**Date:** 2026-03-14
**Status:** Accepted

## Context
The dashboard requires a rich client-side UI with real-time updates.
The team (currently solo) has strong .NET/C# skills.

## Options considered
1. **React + TypeScript** — industry-dominant, large ecosystem
2. **Blazor Server** — C# on server, thin client, latency-sensitive
3. **Blazor WASM** — C# compiled to WebAssembly, runs in browser

## Decision
Blazor WebAssembly.

## Rationale
- Eliminates context switching between C# (backend) and JavaScript (frontend)
- Shares domain models and DTOs between client and server without duplication
- Demonstrates .NET ecosystem depth — relevant for .NET-focused teams
- SignalR client library has first-class Blazor integration

## Consequences
- Initial load larger than React SPA (WASM runtime download, mitigated by
  Nginx gzip compression and browser caching)
- Smaller community than React — fewer UI component libraries available