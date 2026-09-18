# HRMS — Development Rules

This document defines how to implement changes in the HRMS codebase so they match existing conventions. It complements `docs/ARCHITECTURE.md` (what the architecture *is*) and `docs/MODULES.md` (what each module *contains*). Read both before making changes; this document is the "how to add to it" companion.

These rules apply to human developers and to AI coding assistants (see root `CLAUDE.md` for the enforcement-oriented version of the same rules).

## 1. Feature Development Workflow

Before writing any code:
1. Identify the correct **module** (see `docs/MODULES.md`). If the feature spans modules, identify which module should own the new command/query based on the existing dependency graph (§ "Cross-Module Dependency Summary" in `docs/MODULES.md`) — don't create a new upward dependency without asking.
2. Open at least **two existing features in that module** (or, if the module is new/sparse, two features in a sibling module of similar shape — e.g. use Companies as a reference for another single-entity CRUD module) and follow their folder structure, naming, and handler style exactly.
3. Confirm whether the module registers `ValidationBehavior<,>` and `AutoMapper` in its `DependencyInjection.cs` before assuming validators/mapping profiles will actually run (Foundation is a known exception — see `docs/ARCHITECTURE.md` §5.2).
4. Implement Domain changes (if any) → Application (command/query/handler/validator/DTO) → Infrastructure (EF configuration, if a new entity) → API (controller action) → migration, in that order.
5. Build the solution and, where a test project exists for the area you touched (currently only Identity authorization), run/extend its tests.

## 2. Identifying the Correct Module and Layer

- A new **entity** belongs in `Modules/<Module>/Domain/Entities/`, inheriting `AuditableEntity<TKey>` (standard) unless the entity is a very simple lookup table, where some modules use plain patterns closer to `BaseEntity<TKey>` — check a sibling entity in the same module first.
- A new **use case** (command or query) belongs in `Modules/<Module>/Application/Features/<EntityPlural>/{Commands|Queries}/<VerbEntity>/`.
- A new **EF Core mapping** belongs in `Modules/<Module>/Infrastructure/Configurations/<Entity>Configuration.cs` implementing `IEntityTypeConfiguration<TEntity>`. Because `ApplicationDbContext.OnModelCreating` uses `ApplyConfigurationsFromAssembly` per module, a new configuration class in an existing module's Infrastructure assembly is picked up automatically — but still add the matching `DbSet<TEntity>` property to `ApplicationDbContext` for discoverability, matching the existing convention (every current entity has one).
- A new **controller action** belongs in the existing controller for that entity if one exists, in `HRMS.API/Controllers/` (root, or the module's subfolder if one is established — e.g. `Controllers/Employee/`, `Controllers/Leave/`, `Controllers/Attendance/`, `Controllers/Identity/`). Do not invent a new subfolder convention without checking whether the module already has (or lacks) one.

## 3. Extending Entities, Commands, Queries, Handlers, Validators, Repositories, Endpoints

- **Entity**: add properties directly to the existing entity class; do not introduce value objects or domain events unless the user explicitly asks — none exist anywhere in the codebase today (the scaffolded `BuildingBlocks/Domain/Events` and `Domain/Rules` folders are empty), so adding one would be a new pattern, not an extension of an existing one.
- **Command/Query**: use a `record` implementing `IRequest<TResponse>`, matching the existing style in that module.
- **Handler**: implement `IRequestHandler<TCommand/TQuery, TResponse>`; inject `IReadRepository<TEntity,TKey>` for reads and `IWriteRepository<TEntity,TKey>` for writes from `HRMS.BuildingBlocks.Application.Abstractions.Persistence`. **Do not** inject `ApplicationDbContext` or `HRMS.Persistence` types directly into a handler, and do not use `IRepository<TEntity,TKey>`/`Repository<T,TKey>` — that combined interface exists in BuildingBlocks but is not registered in DI and is dead code; using it in a new handler will fail at resolution time.
- **Validator**: add a `<VerbEntity>CommandValidator : AbstractValidator<TCommand>` sibling file for every Create/Update command, matching the pattern used elsewhere in the module. Most Delete commands also have one — add one for a new Delete command even though a couple of existing ones (e.g. `DeleteCompanyCommand`) lack it; don't propagate that gap.
- **Business rules**: put reusable validation/invariant logic that spans multiple handlers into a `BusinessRules/<Entity>BusinessRules.cs` class registered as scoped in the module's DI, matching the existing pattern (e.g. `CompanyBusinessRules`, `LeaveRequestBusinessRules`), rather than duplicating logic across handlers or pushing it into the entity itself.
- **Mapping**: check whether the module's existing handlers for that entity actually use `IMapper` or hand-map with `new TDto{...}`. Match whichever the *sibling handlers in the same feature area* do — do not assume a registered AutoMapper `Profile` means mapping is wired in; verify by reading a nearby handler.
- **Pagination**: for list queries, use `PagedRequest`/`PagedResult<TDto>` via `IReadRepository<T,TKey>.GetPagedAsync(...)` if the module's other list queries already do (Employee, Leave, Attendance, Foundation) — but if the module currently returns unpaged lists (Companies, Department, Identity), match that instead of unilaterally introducing paging; raise it with the user if paging is actually needed there.
- **Repository**: never create a bespoke per-module repository interface for plain CRUD — the generic `IReadRepository`/`IWriteRepository` cover that. A module-specific abstraction (like Leave's `ILeaveBalanceTransaction`) is only justified when the operation is a genuine cross-entity transactional workflow that doesn't reduce to CRUD on one entity — confirm this reasoning with the user before adding a new one.
- **Endpoint**: controllers inject `IMediator` (see the MediatR Controller Standard in `docs/ARCHITECTURE.md` §9 — `ISender` should not be introduced into a new controller) and return `Task<IActionResult>` using `Ok()`/`CreatedAtAction()`/`NoContent()`/`NotFound()`/`BadRequest()`. There is no `Result<T>` wrapper or `ActionResult<T>` generic usage to introduce.

## 4. Avoiding Unnecessary Abstractions

- Do not introduce a new base class, generic pattern, service-locator, or cross-module messaging mechanism (event bus, mediator-based module integration, etc.) that does not already exist in the codebase. The current cross-module integration mechanism is **direct project references to another module's Domain project** (see `docs/ARCHITECTURE.md` §3) — that is the established pattern, even though it is a known architectural compromise. Do not replace it with an anti-corruption layer, domain events, or an integration-events bus without an explicit user decision — this is a deliberate architectural boundary the user owns, not a defect to silently correct.
- Do not add a `Result<T>` return-type wrapper, a success-response envelope, structured logging, or a caching layer to the permission-authorization handler as a "cleanup" — these are real gaps (documented in `docs/ARCHITECTURE.md` §13) but fixing them is an architectural decision for the user to make, not an incidental improvement to bundle into an unrelated task.
- Three similar lines of hand-mapping code in a handler is consistent with the codebase's actual habits (most handlers hand-map) — do not "fix" it by forcing `IMapper` usage into a handler that doesn't already use it, unless asked.

## 5. Database Migrations

- All EF Core migrations live in the single, centralized `HRMS.Persistence` project (`src/HRMS.Persistence/Migrations/`) — never create a per-module migrations folder or a second `DbContext`.
- After adding/changing an entity or `IEntityTypeConfiguration`, generate a migration from `HRMS.Persistence` targeting `ApplicationDbContext` (e.g. `dotnet ef migrations add <Name> --project src/HRMS.Persistence --startup-project src/HRMS.API`), matching the existing naming convention seen in `Migrations/` (verb + entity, e.g. `AddCompanyHoliday`, `AddAttendanceDayStatuses`).
- Do not hand-edit a previously-applied migration. Do not delete or squash existing migrations without explicit user approval — this is a destructive, hard-to-reverse operation.
- The relationship between `HRMS.Persistence/MigrationSQL/*.sql` scripts and the EF Core migrations was not fully confirmed during architecture discovery (see `docs/ARCHITECTURE.md` §7) — do not assume a required "also write a .sql file" step; ask the user if unsure whether a new migration needs a companion script.

## 6. Authorization & Permission Changes

- To guard a new endpoint, use `[PermissionAuthorize(PermissionNames.<Module>.<Action>)]` — the same pattern used by `EmployeeController`'s guarded actions. Do not use `[Authorize(Policy = "...")]` with a hand-built string, and do not use `[Authorize(Roles = "...")]` for new endpoints even though `PermissionCatalogController` currently does — that is a documented inconsistency, not the convention to follow.
- Every new permission code must be added as a constant to `PermissionNames.cs` (never a raw string literal in a controller attribute) and registered in `PermissionCatalog.cs` mapped to its owning module code, so `PermissionCatalogSynchronizer` will create the corresponding `Permission` row. Remember the synchronizer only runs when `POST api/PermissionCatalog/synchronize` is called — it does not run automatically, so a newly added permission code will not exist in the database until that endpoint is invoked.
- Never assign permissions to a user through any path other than `User → UserRole → Role → RolePermission → Permission`. Never embed permission claims in the JWT — this is a standing, deliberate project rule (see `CLAUDE.md`).
- Do not silently add authorization to a currently-unguarded endpoint (e.g. `DepartmentController`, `UserController`'s unguarded actions) as a side effect of an unrelated change — this changes runtime access behavior and must be a deliberate, user-approved change, even though the current state is arguably a gap.
- Authorization logic must never live in a controller body or a repository — it belongs solely in the `PermissionAuthorize` attribute + policy provider + handler chain described in `docs/ARCHITECTURE.md` §9.

## 7. Testing and Validation

- Only one test project currently exists (`tests/HRMS.Modules.Identity.Authorization.Tests`). If you add tests for another module, create a new test project following that project's structure and naming (`tests/HRMS.Modules.<Module>.Tests` or similarly scoped), rather than adding ad-hoc test files into a non-test project.
- Build the full solution (`dotnet build HRMS.sln`) after every change. Run the existing test project if you touched anything in Identity's authorization pipeline.
- There is no established integration-test or in-memory-database convention in this repository yet — do not invent one (e.g. Testcontainers, WebApplicationFactory) without confirming with the user, since it would be a new, cross-cutting decision.

## 8. Backward Compatibility & Existing Functionality

- Preserve existing controller routes, DTO shapes, and permission code strings — these are consumed by a frontend (a `ReactApp` CORS policy is configured in `Program.cs`) that is outside this repository. Do not rename an existing DTO property, permission code, or route without explicit approval, since it is a breaking API change for a consumer you cannot see or verify here.
- Preserve the soft-delete and audit-stamping behavior centralized in `ApplicationDbContext.SaveChangesAsync` — never bypass it by calling raw SQL deletes or reimplementing audit logic locally in a handler.
- When a task appears to require fixing one of the documented gaps in `docs/ARCHITECTURE.md` §13 (e.g. the missing `BadRequestException` mapping, Foundation's missing validation pipeline registration, an unguarded controller), do not fix it opportunistically while doing something else — call it out to the user explicitly and let them decide whether it's in scope.

## 9. When Architecture Is Ambiguous or Missing

If a task requires something this repository has no established pattern for (e.g. domain events, a `Result<T>` wrapper, cross-module integration events, structured logging, a caching layer for authorization, API versioning, a success-response envelope):
1. **Stop before writing implementation code.**
2. State plainly that no existing convention covers this.
3. Present the trade-off briefly and ask the user how they want it done, rather than picking a design and proceeding.

This mirrors the standing rule already established for the Identity/Authorization refactor work (see the pinned session memory on authorization architecture principles) and is generalized here to the whole codebase: **never invent architecture silently.**
