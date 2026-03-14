# ADR 001 — Clean Architecture layer structure

**Date:** 2026-03-14
**Status:** Accepted

## Context
The project needs a structure that enforces separation of concerns,
makes business logic independently testable, and is recognizable
by .NET teams in enterprise environments.

## Options considered
1. **Vertical slice architecture** — feature folders, all layers together
2. **Clean Architecture** — strict concentric layers with dependency rules
3. **Simple MVC** — controllers directly calling EF Core

## Decision
Clean Architecture with 4 layers: Domain → Application → Infrastructure → API.
Dependency rule: outer layers depend on inner layers, never the reverse.

## Rationale
- Domain and Application layers have zero infrastructure dependencies
  → testable with pure unit tests, no database required
- Recognized pattern in .NET enterprise and healthtech teams
- Demonstrates architectural thinking beyond tutorial-level code

## Consequences
- More initial boilerplate than MVC
- Interface-based design enables easy mocking in tests
- Adding new use cases follows a predictable, consistent pattern