# CLAUDE.md — HRMS Project Instructions

This file is the permanent, authoritative instruction set for Claude Code (or any AI coding assistant) working in this repository. It is derived entirely from a verified, read-only architecture audit of the actual source code (see `docs/ARCHITECTURE.md`, `docs/MODULES.md`, `docs/DEVELOPMENT_RULES.md` for the full detail behind every rule here). Follow it exactly. When any instruction here appears to conflict with a specific user request, point out the conflict and ask before proceeding — do not silently pick one side.

## 1. Project Identity & Technology Stack

- **HRMS** — a Human Resource Management System built as a **.NET 9 modular monolith**, single deployable ASP.NET Core Web API (`HRMS.API`).
- Stack: ASP.NET Core 9 Web API, Entity Framework Core 9 (SQL Server provider), MediatR (CQRS), FluentValidation, AutoMapper, JWT Bearer authentication with a custom permission-based authorization system.
- Modules (business areas): **Employee, Department, Identity, Companies, Leave, Attendance, Foundation** — each its own `Domain`/`Application`/`Infrastructure` class library. `Designation` is a scaffolded-but-empty module (not in the solution). `HRMS.Application` (top-level) is dead/orphaned — never use it.
- Persistence is **centralized**: one `ApplicationDbContext` (in `HRMS.Persistence`), one migration history, for all modules.
- Only one test project exists: `tests/HRMS.Modules.Identity.Authorization.Tests`.

Read `docs/ARCHITECTURE.md`, `docs/MODULES.md`, and `docs/DEVELOPMENT_RULES.md` before making non-trivial changes. Those documents are the canonical, detailed reference; this file is the enforceable summary.

## 2. Non-Negotiable Architecture Rules

1. **Do not change the existing project or folder structure** (module layout, Domain/Application/Infrastructure split, `Features/<Entity>/{Commands|Queries}/<Verb>` shape) unless the user explicitly approves it.
2. **Do not invent a new architecture, layer, design pattern, or module boundary** when an existing convention already covers the situation — see `docs/DEVELOPMENT_RULES.md` §9 for what to do when no convention exists.
3. **Do not create duplicate implementations** of shared abstractions already in `HRMS.BuildingBlocks` (repositories, exceptions, pagination, `IUserContext`) or `HRMS.Persistence` (the DbContext, repository base classes).
4. **Before implementing a feature, read at least two existing similar implementations** (ideally in the same module) and mirror their structure.
5. **Before creating a new file, determine the correct existing project, folder, namespace, and module** using `docs/MODULES.md`. Do not create a new top-level project or a new module folder without approval.
6. **Do not move, rename, or delete existing files** without explicit approval.
7. **Do not add new NuGet packages or frameworks** without explaining why and getting approval — the current stack (MediatR, FluentValidation, AutoMapper, EF Core, JWT Bearer) already covers CQRS, validation, mapping, persistence, and auth.
8. **Do not change established CQRS/MediatR, repository, validation, or authorization patterns** without concrete evidence from the codebase and explicit approval.
9. **Never bypass authentication, authorization, validation, or business-rule (BusinessRules class) logic.** Authorization decisions are made server-side only — never trust a client-supplied permission/role claim.
10. **Do not expose internal Domain or Infrastructure types through API contracts** — controllers return DTOs, never entities directly.
11. **Never assume an endpoint, entity, table, or service exists — verify it** in `docs/MODULES.md`/the source before referencing it.
12. **Do not produce frontend implementation guidance or code that assumes a backend endpoint that doesn't exist** — report the missing backend piece first. (A React frontend consumes this API via the `ReactApp` CORS policy in `Program.cs`, but it is outside this repository.)
13. **Preserve existing behavior and backward compatibility** — routes, DTO shapes, and permission code strings are a public contract for an external frontend.
14. **If a requested change conflicts with established architecture, stop and explain the conflict before writing code.**
15. **If an architectural decision is ambiguous or a pattern is missing, ask the user — do not invent a solution.**
16. **Never claim a feature is complete without verifying the actual code, a successful build, and (where relevant) the migration/database change.**

## 3. Authorization-Specific Standing Rules

(Carried forward from the ongoing Permission/Authorization refactor; apply to any authorization-adjacent work.)

- Keep all Identity authorization infrastructure inside the Identity module — never let it leak into other modules.
- Authorization logic stays out of controllers and repositories; it lives in the `[PermissionAuthorize]` attribute → `PermissionAuthorizationPolicyProvider` → `PermissionAuthorizationHandler` chain (`docs/ARCHITECTURE.md` §9).
- Permission assignment is strictly database-driven through **User → UserRole → Role → RolePermission → Permission** — no parallel/shortcut assignment path.
- Permission codes must never be magic strings — always add them to `PermissionNames.cs` and register them in `PermissionCatalog.cs`.
- Do not create one ASP.NET Core authorization policy per permission — the existing generic `"Permission:"`-prefixed policy provider already scales; extend it, don't replace it.
- Do not put permissions into the JWT unless the user explicitly decides to do so — this is a deliberate, open architectural decision.
- Do not silently add or remove authorization attributes on existing endpoints as a side effect of an unrelated change, even where a real gap exists (several controllers currently have missing/commented-out authorization — see `docs/ARCHITECTURE.md` §9.1). Flag it; don't fix it opportunistically.

## 4. Rules for Extending Existing Modules

- Follow the exact feature-folder shape documented in `docs/ARCHITECTURE.md` §5: `Features/<EntityPlural>/{Commands|Queries}/<VerbEntity>/` with a command/query, handler, and (for Create/Update, and most Delete) a validator.
- Use `IReadRepository<T,TKey>` / `IWriteRepository<T,TKey>` from `HRMS.BuildingBlocks` — never `IRepository<T,TKey>` (dead, unregistered), never `DbContext` directly in a handler.
- Match the module's existing mapping style (`IMapper` vs. manual `new TDto{...}`) and pagination style (`PagedResult<T>` vs. plain list) by checking sibling handlers first — these differ *by module*, and getting it wrong creates a new inconsistency rather than following one.
- New entities: add to `Domain/Entities/`, add EF configuration to `Infrastructure/Configurations/`, add the `DbSet<T>` line to `HRMS.Persistence/Context/ApplicationDbContext.cs`, and generate a migration in `HRMS.Persistence/Migrations/` (never a second DbContext or a per-module migrations folder).

## 5. Rules for Introducing New Modules

- Only add a new module (or fill in `Designation`, which is currently an empty stub) with explicit user approval.
- A new module must follow the exact three-project shape (`HRMS.Modules.<Name>.{Domain,Application,Infrastructure}`), be added to `HRMS.sln`, wired into `Program.cs` via its own `Add<Name>Application()`/`Add<Name>Infrastructure()` extension methods, and its entities picked up by `ApplicationDbContext` via an `ApplyConfigurationsFromAssembly` call plus explicit `DbSet<T>` properties — matching every existing module.
- Respect the existing dependency direction (`docs/MODULES.md` "Cross-Module Dependency Summary") — don't create a dependency cycle or an "upward" reference without discussing it first.

## 6. Rules for Missing or Ambiguous Patterns

Some things do **not** exist in this codebase yet, despite scaffolded folders suggesting they were planned: domain events, domain rule/invariant objects, a `Result<T>` return wrapper, a success-response envelope, structured logging, a caching layer for authorization checks, API versioning. **Do not implement any of these speculatively.** If a task seems to need one, stop and ask the user how they want it approached — see `docs/DEVELOPMENT_RULES.md` §9.

## 7. Mandatory Verification Process Before Modifying Code

1. Locate the module and layer using `docs/MODULES.md`.
2. Open at least two existing, comparable files in that module (or a structurally similar module) to confirm the exact pattern to follow — do not rely on memory of "typical .NET conventions."
3. Check that module's `DependencyInjection.cs` for what's actually registered (validation pipeline, AutoMapper) — do not assume every module registers everything (Foundation is a known exception).
4. Make the change, keeping it scoped to what was asked — do not opportunistically refactor, rename, or "fix" unrelated inconsistencies documented in `docs/ARCHITECTURE.md` §13 without calling them out first.
5. Build the solution. Run the relevant test project if the change touches Identity authorization.
6. Only report the task complete after the build (and any relevant tests) actually pass — never assert completion without verifying.

## 8. Explicit Prohibition

**Do not invent new architecture, patterns, or module structures without approval.** Every rule above exists because it was verified against actual source code, not assumed from `.NET` conventions in general — when this repository's actual pattern differs from a "typical" or "best practice" approach (e.g. centralized persistence instead of per-module DbContexts, direct module-to-module Domain references instead of an anti-corruption layer, hand-mapping instead of universal AutoMapper usage), **follow what is actually here**, and treat any change to that as a proposal requiring explicit user sign-off, not a default action.
