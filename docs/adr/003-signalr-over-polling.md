# ADR 003 — SignalR over polling for real-time updates

**Date:** 2026-03-14
**Status:** Accepted

## Context
The dashboard needs to reflect resource status changes within seconds
of them occurring, without user-initiated refresh.

## Options considered
1. **Client polling** — GET /api/resources every N seconds
2. **Server-Sent Events (SSE)** — one-way push from server
3. **SignalR** — WebSocket with HTTP fallback

## Decision
SignalR with IHubContext for server-initiated push.

## Rationale
- Polling creates O(n) load with connected client count — unacceptable
  at scale
- SSE is unidirectional — clients cannot join role groups
- SignalR provides WebSocket with automatic fallback to long-polling
  for restricted network environments (common in healthcare/enterprise)
- Role-based groups allow routing critical alerts to admins only

## Consequences
- Persistent connections require sticky sessions in load-balanced deployments
- IRealtimeNotifier abstraction keeps Application layer free of SignalR dependency