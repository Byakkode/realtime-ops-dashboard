# ADR 002 — MediatR for CQRS and domain event dispatch

**Date:** 2026-03-14
**Status:** Accepted

## Context
Commands (write) and queries (read) have different performance and
consistency requirements. Domain events need to be dispatched after
persistence without tight coupling between handlers.

## Options considered
1. **Service classes** — ResourceService with all methods
2. **MediatR CQRS** — separate Command/Query handlers + INotification
3. **Direct method calls** — controllers call repositories directly

## Decision
MediatR with explicit Command/Query separation and INotificationHandler
for domain events.

## Rationale
- Each handler has a single responsibility — easy to test in isolation
- Domain event handlers (alerts, SignalR notifications) are decoupled
  from the command that triggers them
- Industry-standard pattern in .NET teams

## Consequences
- Slight indirection (controller → mediator → handler)
- Adding cross-cutting concerns (logging, validation) via pipeline behaviors
  becomes trivial