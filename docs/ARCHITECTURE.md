# HRMS — Architecture Reference

Status legend used throughout this document:
- **[V] Verified** — confirmed directly from source code in this repository.
- **[I] Inferred** — a reasonable reading of the code whose intent was not fully confirmed (e.g. no comment/test proves the "why"); treat as likely, not certain.
- **[R] Recommendation** — not current behavior. A suggestion for the future, never to be treated as existing convention.

This document is a factual snapshot as of the `main` branch on 2026-09-17. It reflects a codebase mid-refactor (see `docs/MODULES.md` for per-module status and the git status noted in `CLAUDE.md`).

---

## 1. Architecture Overview [V]

HRMS is a **.NET 9 modular monolith**: one deployable ASP.NET Core Web API (`HRMS.API`) composed of seven business **modules**, each split into its own **Domain**, **Application**, and **Infrastructure** class library, plus two shared cross-cutting libraries (`HRMS.BuildingBlocks`, `HRMS.Persistence`). It is *not* a set of independently deployable microservices, and it is *not* a single-project layered app — module boundaries exist at the project/assembly level, but persistence is centralized (see §7).

Per-module internal layering follows Clean Architecture direction (Domain has no outward dependencies; Application depends only on Domain + BuildingBlocks; Infrastructure implements Application's contracts and depends on Persistence/Domain). The Application layer within each module is organized around **CQRS with MediatR** (commands and queries as discrete request/handler pairs), not a service-layer or repository-per-feature style.

Modules are **not fully isolated** from each other: several modules' Application/Domain projects directly reference other modules' Domain projects (see §3 dependency graph). There is no anti-corruption layer, no module-to-module messaging bus, and no event-driven integration between modules today — cross-module reads happen via direct project references and repository calls.

## 2. Solution & Project Structure [V]

```
HRMS.sln
src/
  HRMS.API/                      Web API host — ASP.NET Core, controllers, composition root (Program.cs)
  HRMS.BuildingBlocks/           Shared kernel: base entities, abstractions, pipeline behaviors, exceptions, pagination
  HRMS.Persistence/               Centralized EF Core DbContext, generic repositories, migrations (single project for ALL modules)
  HRMS.Application/               DEAD/ORPHANED — obj/ only, no .cs files, not in HRMS.sln. Do not use or resurrect without asking.
  Modules/
    Employee/    {Domain, Application, Infrastructure}
    Department/  {Domain, Application, Infrastructure}
    Identity/    {Domain, Application, Infrastructure}
    Companies/   {Domain, Application, Infrastructure}
    Leave/       {Domain, Application, Infrastructure}
    Attendance/  {Domain, Application, Infrastructure}
    Foundation/  {Domain, Application, Infrastructure}
    Designation/ {Domain/Entities}  — EMPTY placeholder, no files exist, not in HRMS.sln
tests/
  HRMS.Modules.Identity.Authorization.Tests/   Only test project in the solution; covers Identity's permission/authorization pieces
docs/                              This governance documentation (new)
```

Not part of the buildable solution but present on disk, not previously called out: `src/Document/` holds supplementary PDFs/notes (module technical-documentation exports) — non-code, no bearing on any architecture claim. `src/HRMS.API/Controllers/TestController.cs` exists as a standalone diagnostic/scratch controller not attributable to any module; it is intentionally excluded from the per-module controller inventory in `docs/MODULES.md`.

Each module follows the same three-project shape and naming convention: `HRMS.Modules.<ModuleName>.Domain`, `HRMS.Modules.<ModuleName>.Application`, `HRMS.Modules.<ModuleName>.Infrastructure`. `Designation` deviates — it has an empty `Domain/Entities` folder with no tracked files and is absent from `HRMS.sln`; treat it as **not implemented**, not as an existing pattern to copy.

## 3. Module Dependency Graph [V]

### 3.1 Application-layer Domain dependencies

Derived from actual `<ProjectReference>` entries in each module's `.Application.csproj` (Domain-to-Domain references):

```
Foundation.Domain   ← Employee.Application
Department.Domain   ← Employee.Application
Employee.Domain     ← Identity.Application, Leave.Application, Attendance.Application
Leave.Domain        ← Attendance.Application
Companies.Domain    ← Attendance.Application
```

Leaf modules with no outbound cross-module Domain dependency: **Companies, Department, Foundation** (Application layer only references its own Domain).

Implied dependency order (lower modules should not depend on higher ones): `Foundation, Department, Companies` → `Employee` → `Identity, Leave` → `Attendance`.

### 3.2 Infrastructure-layer cross-module dependencies [V]

The Application-layer graph above is not the whole picture — each module's Infrastructure project independently carries its own cross-module Domain references (same directions as §3.1, no contradictions, but not visible if you only inspect Application `.csproj` files):

```
Department.Domain, Foundation.Domain  ← Employee.Infrastructure
Employee.Domain                       ← Leave.Infrastructure
Companies.Domain                      ← Attendance.Infrastructure
```

### 3.3 HRMS.API's real build closure — verified via `project.assets.json`, not just `HRMS.API.csproj` [V]

`HRMS.API.csproj` itself lists **direct** `<ProjectReference>` entries only to: `HRMS.BuildingBlocks`, `HRMS.Persistence`, and the **Application** projects of Attendance, Department, Employee, Identity, Leave — plus the **Infrastructure** projects of Identity and Leave only. Reading only this file (as an earlier pass of this document did) gives the false impression that Companies and Foundation are excluded from the compiled API.

They are not. `HRMS.Persistence.csproj` itself references the **Infrastructure** project of every module except Department (see §13 item 11 — this Department omission is a separate, critical defect, not an intentional exception), and each module's Infrastructure project references its own Application project (standard Infrastructure→Application direction). This produces a transitive chain:

```
HRMS.API → HRMS.Persistence → {Attendance, Employee, Foundation, Identity, Companies, Leave}.Infrastructure → that module's own Application project
```

Confirmed by inspecting `src/HRMS.API/obj/project.assets.json`'s resolved `targets` closure (which includes `HRMS.Modules.Companies.Application` and `HRMS.Modules.Foundation.Application` as transitive project dependencies, with `HRMS.Modules.Companies.Infrastructure`/`HRMS.Modules.Foundation.Infrastructure` as the direct link) and by a live `dotnet build src/HRMS.API/HRMS.API.csproj`, which compiled `CompaniesController.cs`/`AddressTypesController.cs` (both of which `using` the Companies/Foundation Application namespaces) with **zero compiler errors** — the only failures observed were output-file-copy locks from a separately running `HRMS.API` process, not a missing-reference/compile failure. **Companies and Foundation are fully part of the compiled solution; do not treat their absence from `HRMS.API.csproj`'s own reference list as evidence they are unused or unwired.**

Companies and Foundation still have **no** Infrastructure `DependencyInjection.cs` at all [V] — meaning nothing beyond EF configuration discovery flows through this transitive path for those two modules; their Application-layer DI extension methods (`AddCompaniesApplication()`, `AddFoundationApplication()`) are still called explicitly from `Program.cs`, same as every other module.

### 3.4 A scoped, non-standard reference: `HRMS.Persistence → Leave.Application` [V]

`HRMS.Persistence.csproj` also carries a direct `<ProjectReference>` to `HRMS.Modules.Leave.Application.csproj` — not just `Leave.Infrastructure`, unlike any other module. This is a deliberate-looking, narrow exception to the normal Infrastructure→Application direction (here, the shared Persistence project itself depends on one module's Application layer), needed because `HRMS.Persistence.Transactions.LeaveBalanceTransaction` implements `ILeaveBalanceTransaction`, an interface declared in `Leave.Application.Abstractions.Persistence`. **Treat this as a one-off, scoped exception tied specifically to that interface — not a precedent for referencing an Application project from Persistence or BuildingBlocks in general.**

**[R]** If a true modular-monolith isolation guarantee is ever desired, this direct Domain-to-Domain coupling (and the Persistence→Leave.Application reference above) would need to be replaced with contracts/events. That is a future architectural decision, not something to introduce unilaterally — flag it to the user rather than refactoring it as a side effect of an unrelated task.

## 4. Layer Responsibilities [V]

- **Domain** (`Modules/<M>/Domain`): entities only (`Domain/Entities/*.cs`). Entities inherit `AuditableEntity<TKey>` (from BuildingBlocks) for most business entities, or plain `BaseEntity<TKey>` for simpler lookup-style entities. No value objects, no domain events, and no domain-level business-rule/invariant classes were found in any module's Domain project — business rules live in the **Application** layer instead (see `BusinessRules/` folders, §5). `HRMS.BuildingBlocks/Domain/Events` and `HRMS.BuildingBlocks/Domain/Rules` exist as **empty scaffold folders** [V] — this indicates domain events/domain rules were planned but are not implemented anywhere; do not assume they exist, and do not invent an implementation without confirming with the user first.
- **Application** (`Modules/<M>/Application`): CQRS commands/queries + MediatR handlers, FluentValidation validators, DTOs, AutoMapper profiles (where used), per-entity `BusinessRules` classes, and the module's DI composition (`DependencyInjection/DependencyInjection.cs`). Depends on Domain and `HRMS.BuildingBlocks` only — never on Infrastructure or `HRMS.Persistence`.
- **Infrastructure** (`Modules/<M>/Infrastructure`): EF Core `IEntityTypeConfiguration<T>` classes under `Configurations/`, and (for Identity only, richly) module-specific service implementations under `Security/`/`Authorization/`. Does **not** contain a module-specific `DbContext` — see §7.
- **HRMS.Persistence**: the single, centralized EF Core `ApplicationDbContext`, generic repository implementations, and **all** EF Core migrations for **every module**, in one project.
- **HRMS.BuildingBlocks**: shared kernel referenced by every module — base entities, generic repository interfaces, MediatR `ValidationBehavior`, typed exceptions, pagination types, `IUserContext`.
- **HRMS.API**: composition root (`Program.cs`), controllers (grouped by module under `Controllers/<Module>/`), middleware, and the `CurrentUserContext` implementation of `IUserContext`.

## 5. Application Layer / CQRS Pattern [V]

Standard feature shape (verified across Employee, Leave, Attendance, Companies):

```
Application/Features/<EntityPlural>/
  Commands/<VerbEntity>/
    <VerbEntity>Command.cs            record : IRequest<TResponse>
    <VerbEntity>CommandHandler.cs     class : IRequestHandler<TCommand, TResponse>
    <VerbEntity>CommandValidator.cs   class : AbstractValidator<TCommand>   (usually present; not 100% universal, see below)
  Queries/<VerbEntity>/
    <VerbEntity>Query.cs              record : IRequest<TResponse | PagedResult<TDto>>
    <VerbEntity>QueryHandler.cs
  DTOs/
    <Entity>Dto.cs
  Mappings/                            (present in most, not all, modules — see §5.2)
    <Entity>Profile.cs                 AutoMapper Profile
  BusinessRules/
    <Entity>BusinessRules.cs           plain class, injected into handlers, holds validation/invariant logic
```

Handlers access the database exclusively through the generic repository abstractions in `HRMS.BuildingBlocks.Application.Abstractions.Persistence`:
- `IReadRepository<TEntity, TKey>` — queries, injected into query handlers. Includes `GetPagedAsync(PagedRequest, predicate, orderBy, includes)` for paged list queries.
- `IWriteRepository<TEntity, TKey>` — commands, injected into command handlers.
- `IRepository<TEntity, TKey>` — a combined interface exists in BuildingBlocks with an implementation (`HRMS.Persistence.Repositories.Repository<T,TKey>`), but it is **not registered in DI** (`AddPersistence` only registers `IReadRepository`/`IWriteRepository`). **[V] Treat `IRepository<T,TKey>` as dead/legacy — do not build new features against it**; use the split read/write interfaces that match the rest of the codebase.

Handlers do **not** use `DbContext` directly, and do not call `HRMS.Persistence` types outside the repository interfaces — this boundary is consistently respected.

### 5.1 Module-specific Application abstractions
Only two modules define their own `Application/Abstractions/` folder, and each for a different reason — this is not a generic pattern to copy without a real need:
- **Leave**: `Application/Abstractions/Persistence/ILeaveBalanceTransaction.cs` — a domain-transaction service (`ApproveLeaveAsync`, `CancelLeaveAsync`) for operations that don't reduce to a single CRUD call.
- **Identity**: `Application/Abstractions/Security/{IJwtTokenService, IPasswordHasher, JwtOptions}` — security service contracts, not data abstractions.

### 5.2 Known inconsistencies in the Application layer [V]
- **Foundation's `DependencyInjection.cs` omits the `ValidationBehavior<,>` MediatR pipeline registration** that every other module registers — FluentValidation validators registered there are likely never invoked automatically. Verify before assuming Foundation's validators run.
- **The overwhelming majority of Employee's feature namespaces are rooted at `HRMS.Application.Features.*`** (missing the `Modules.Employee` segment) instead of `HRMS.Modules.Employee.Application.Features.*` used by every other module — confirmed in 261 of 264 files under `Modules/Employee/Application/Features/`. Two files already use the "correct" `HRMS.Modules.Employee.Application.Features.*` form (`Features/Employees/Queries/GetEmployees/GetEmployeesQuery.cs` and its handler), so this is not a hard 100%-universal rule, but it is the dominant, systemic pattern. This is almost certainly a naming slip, not an intentional convention — new Employee features should still be added under the existing (inconsistent) `HRMS.Application.Features.*` namespace to stay consistent with the surrounding sibling files in the same folder, unless the user asks for a rename.
- **AutoMapper is registered module-wide in every module except Identity, but genuinely used (`IMapper` injected and called) in a minority of handlers.** Most handlers hand-map with `new XDto { ... }` even when a matching `Profile` class exists. Do not assume a `Profile` class means mapping is actually wired into a given handler — check the handler itself.
- **Mapping profile file location varies**: Employee/Companies/Attendance mostly use a `Mappings/` subfolder; Leave places profile classes directly at the feature root (no `Mappings/` folder).
- **Pagination is not used everywhere**: Employee, Leave, Attendance, and Foundation queries generally return `PagedResult<TDto>` via `GetPagedAsync`; Companies and Department list queries currently return unpaged `IReadOnlyList<TDto>` via `GetAllAsync`. Follow the paging convention already used by the module you are extending — don't silently introduce paging into Companies/Department, or drop it from Employee/Leave, without discussing it.
- **Validator coverage on Delete commands is inconsistent** (e.g. `DeleteCompanyCommand` has no validator, while most Delete commands elsewhere do).

## 6. Domain Entities & Database Design [V]

- All auditable entities inherit `HRMS.BuildingBlocks.Domain.Entities.AuditableEntity<TKey>`: `Id`, `CreatedBy`, `CreatedOn`, `ModifiedBy`, `ModifiedOn`, `DeletedBy`, `DeletedOn`, `IsDeleted`.
- **Soft delete is enforced centrally**: `ApplicationDbContext.SaveChangesAsync` intercepts `EntityState.Deleted` and rewrites it to `EntityState.Modified` with `IsDeleted = true` plus deletion audit fields — a physical `DbSet.Remove()`/`DeleteAsync()` call never produces a hard delete for an `AuditableEntity<TKey>`. `BaseReadRepository` always filters `Where(x => !x.IsDeleted)` in its query methods.
- **Per-entity `HasQueryFilter(x => !x.IsDeleted)` coverage is real but inconsistent, not absent.** There is no single "global" filter applied by one central call — instead, 36 of the 55 `IEntityTypeConfiguration<T>` classes across all modules individually call `builder.HasQueryFilter(x => !x.IsDeleted)` (e.g. `EmployeeConfiguration.cs`, `UserRoleConfiguration.cs`, `AttendanceDeviceConfiguration.cs`, `DepartmentConfiguration.cs`). For those 36 entities, a query written directly against `DbSet<T>` (bypassing the repository) **still gets the soft-delete filter automatically**, because EF Core applies `HasQueryFilter` at the model level regardless of how the query is written. The remaining **19 entity configurations have no query filter at all**, meaning a raw `DbSet<T>` query against these *would* return soft-deleted rows: **all 10 Leave-module configurations** (`LeaveYear`, `LeaveYearStatus`, `LeaveType`, `LeavePolicy`, `LeavePolicyRule`, `LeaveRequest`, `LeaveRequestStatus`, `LeaveDayPart`, `CompanyHoliday`, `EmployeeLeaveEntitlement`), 6 of Attendance's 10 (`AttendancePolicy`, `AttendanceShift`, `AttendanceRecord`, `AttendanceRegularization`, `AttendanceRegularizationStatus`, `AttendanceDayStatus`), `CompanyConfiguration` (Companies' only entity), and `RoleConfiguration`/`UserConfiguration` (Identity). This inconsistent coverage — not a total absence — is the real risk when writing a query that bypasses `IReadRepository`.
- Audit fields (`CreatedBy`/`ModifiedBy`/`DeletedBy`) are populated centrally in the same `SaveChangesAsync` override, sourced from `IUserContext.UserId`.
- EF Core entity configurations live in each module's `Infrastructure/Configurations/*Configuration.cs` (`IEntityTypeConfiguration<T>`), one file per entity — this convention is consistent across modules.

## 7. Persistence — Centralized, Not Per-Module [V]

There is **one** `DbContext` for the entire solution: `HRMS.Persistence.Context.ApplicationDbContext`. It declares a `DbSet<T>` property for every entity across every module, and its `OnModelCreating` calls `modelBuilder.ApplyConfigurationsFromAssembly(typeof(<SomeConfig>).Assembly)` once per module to pull in that module's `IEntityTypeConfiguration<T>` classes by assembly scan. **This is the established pattern — new entities in an existing module are picked up automatically by the existing `ApplyConfigurationsFromAssembly` call for that module's assembly; you do not need to add a new `DbSet` line for the context to work, but the codebase's existing convention is to also add an explicit `DbSet<T>` property for discoverability. Follow that convention.**

All EF Core migrations for all modules live in **one** folder: `src/HRMS.Persistence/Migrations/`, organized into per-feature subfolders (e.g. `Migrations/EmployeeAddress/`, `Migrations/AddLeaveModule/`) but sharing one migration history against one database. `src/HRMS.Persistence/MigrationSQL/` additionally holds hand-authored `.sql` scripts mirrored per entity — their exact relationship to the EF migrations (generated companion vs. manual seed/patch scripts) was not further verified in this pass; **[R]** confirm intent with the user before writing new files there.

`SqlServer` is the configured EF Core provider (`UseSqlServer`, `AddPersistence` in `HRMS.Persistence/DependencyInjection/DependencyInjection.cs`).

## 8. Request/Response Flow [V]

```
HTTP request
  → ASP.NET Core routing → Controller action (api/[controller])
  → [PermissionAuthorize("...")] / [Authorize(Roles="...")] / (often: nothing — see §9)
  → MediatR IMediator.Send(command or query)
  → ValidationBehavior<TRequest,TResponse> pipeline step (FluentValidation, if a validator is registered AND the module registered the behavior; throws FluentValidation's own ValidationException, not the BuildingBlocks one — see §10)
  → Handler (IRequestHandler) — loads via IReadRepository, applies BusinessRules, mutates/creates entity, saves via IWriteRepository
  → ApplicationDbContext.SaveChangesAsync (soft-delete + audit interception)
  → Handler returns a DTO / PagedResult<TDto> / Guid id
  → Controller wraps result in Ok(...)/CreatedAtAction(...)/NoContent() — IActionResult, no generic Result<T> wrapper
  → ExceptionHandlingMiddleware catches any thrown exception and maps it to a JSON ErrorResponse + status code
```

There is **no** standard success-response envelope — controllers return raw DTOs via `Ok(dto)`. There **is** a standard error envelope: `HRMS.API.Responses.ErrorResponse { StatusCode, Message, Timestamp, TraceId, Errors }`, produced only by `ExceptionHandlingMiddleware`.

### 8.1 MediatR Controller Standard [V]

All `HRMS.API` controllers must use `IMediator` — not `ISender` — for sending commands and queries. Historically both patterns existed side by side across the Attendance module's controllers (`ISender`) and controllers like `EmployeeController` (`IMediator`); the Attendance controllers have since been converted to `IMediator` so the codebase is consistent, and this is now the standing rule going forward, not just a description of the majority pattern.

Constructor injection with a consistent private field name:

```csharp
private readonly IMediator _mediator;

public ExampleController(IMediator mediator)
{
    _mediator = mediator;
}
```

Rules:

- New API controllers must inject `IMediator`, not `ISender`.
- Existing controllers already using `IMediator` are unaffected — no action needed.
- `ISender` should not be introduced into a new controller without an explicit, user-approved architectural exception.
- `IMediator` and `ISender` are both MediatR abstractions (`IMediator` extends `ISender` and `IPublisher`) — this is a project-wide consistency rule, not a functional or architectural change. `Send(...)` behavior, the `ValidationBehavior<,>` pipeline, handler resolution, and everything downstream of the controller are unaffected.
- This rule applies to the **API presentation layer only**. It does not require touching commands, queries, handlers, DTOs, validators, repositories, business rules, or any MediatR usage inside the Application/Infrastructure layers.

## 9. Authentication & Authorization Flow [V]

Full chain, verified end to end:

1. **Login** issues a JWT (`Modules/Identity/Infrastructure/Security/JwtTokenService.cs`) carrying `sub`/`userId`, `employeeId`, `ClaimTypes.Name`, `jti`, and one `ClaimTypes.Role` claim per role name. **Permission codes are never embedded in the JWT** — this matches the standing project rule captured in `CLAUDE.md`.
2. `CurrentUserContext` (`HRMS.API/Services/CurrentUserContext.cs`, implementing `HRMS.BuildingBlocks.Application.Abstractions.IUserContext`) reads `userId`/`employeeId`/name claims from `HttpContext.User` per request.
3. Permission model (chain, exactly as intended): **`User` → `UserRole` → `Role` → `RolePermission` → `Permission`** (`Permission` also belongs to a `Module`). All entities under `Modules/Identity/Domain/Entities/`.
4. **Declaring a required permission on an endpoint**: a custom `[PermissionAuthorize("Employee.View")]` attribute (`Modules/Identity/Application/Authorization/PermissionAuthorizeAttribute.cs`), which builds the ASP.NET Core policy name `"Permission:" + code`.
5. **Resolving the policy**: `PermissionAuthorizationPolicyProvider` (registered as `IAuthorizationPolicyProvider`) recognizes any policy name prefixed `"Permission:"` and constructs an `AuthorizationPolicy` with a `PermissionRequirement` on the fly — **there is no per-permission `AddPolicy(...)` registration**, satisfying the standing "don't create one ASP.NET Core policy per permission" rule.
6. **Evaluating the requirement**: `PermissionAuthorizationHandler` re-queries the database on every request (`User` → `UserRoles` → active `Role`s → `RolePermissions` → active `Permission`s) and checks for the required permission name. **No caching layer exists** — three repository round-trips per authorization check, a deliberate-but-unoptimized choice, worth a future performance review but not to be "fixed" silently.
7. **Permission catalog**: `PermissionNames.cs` centralizes permission code constants (satisfying the "no magic strings" rule) as nested static classes (e.g. `PermissionNames.Employee.View = "Employee.View"`). `PermissionCatalog.cs` maps each constant to a module code. `PermissionCatalogSynchronizer` (Infrastructure, behind `IPermissionCatalogSynchronizer`) one-way-syncs missing `Permission` rows into the database (insert-only, idempotent); it must be triggered manually via `POST api/PermissionCatalog/synchronize` (`PermissionCatalogController`) — it does **not** run automatically at startup.
8. Wiring: `AddIdentityAuthorization()` (policy provider + handler registration) and `AddIdentityInfrastructure(configuration)` (JWT options, token service, password hasher, catalog synchronizer) are both called explicitly from `Program.cs`.
9. **Exposing effective permissions to the frontend**: `GET api/auth/me` (`AuthController.Me`) returns the caller's effective roles and permissions alongside `UserId`/`EmployeeId`/`UserName`/`IsAuthenticated`, via `GetEffectivePermissionsQuery`/`GetEffectivePermissionsQueryHandler` (`Modules/Identity/Application/Features/Identity/Queries/GetEffectivePermissions/`). This handler intentionally re-implements the same `User` → `UserRoles` → active `Role`s → `RolePermissions` → active `Permission`s traversal as `PermissionAuthorizationHandler` (step 6 above), rather than sharing a common service, so it stays consistent with the existing per-feature handler convention (no new abstraction layer). Both code paths use the exact same active-`Role`/active-`Permission` filters and the same lack of a `User.IsActive` check, so `/api/auth/me` can never disagree with an actual `[PermissionAuthorize]` decision — but a future change to one must be mirrored in the other by hand, since the logic is duplicated, not shared.

### 9.1 Known authorization gaps — verified, not hypothetical [V]
These are real, currently-shipped states of the code, not speculative risks — confirm with the user before "fixing" any of them, since closing them changes runtime access behavior:
- `EmployeeController`: mutating actions (Create/Update/Delete) and the class-level `[Authorize(Roles = "Employee")]` are **commented out** in source; only `GetAll`/`GetById` carry `[PermissionAuthorize(...)]`.
- `DepartmentController`, `AttendanceRecordsController`, `RolePermissionsController`: **no authorization attributes at all**.
- `UserController`: only `ResetPassword` is guarded; Create/Update/Deactivate/ChangePassword/GetAll/GetById are not.
- `PermissionCatalogController` (the endpoint that seeds the permission system itself) uses `[Authorize(Roles = "Admin")]` — role-based, not the module's own `PermissionAuthorize` policy convention.
- `PermissionCatalog.Entries` only declares Employee, Department, and one Identity permission — Attendance, Leave, and most of Identity's own endpoints have no permission codes defined at all yet.

## 10. Cross-Cutting Concerns [V]

- **Exception handling**: `HRMS.API.Middleware.ExceptionHandlingMiddleware` maps typed exceptions from `HRMS.BuildingBlocks.Application.Exceptions` to HTTP status codes: `ValidationException`→400 (both FluentValidation's own and the BuildingBlocks one), `NotFoundException`→404, `ConflictException`→409 (plus SQL error 2601/2627 duplicate-key detection→409), `UnauthorizedException`→401, `BusinessException`→400, anything else→500. **`BadRequestException` is a plain `Exception` subclass (does not inherit `BusinessException`) and is not explicitly matched in the middleware's switch — it currently falls through to the generic 500 branch.** This looks like a real defect; do not silently "fix" it as a drive-by change — flag it and let the user decide.
- **Validation**: FluentValidation, run via the shared `ValidationBehavior<TRequest,TResponse>` MediatR pipeline behavior, registered per-module in each module's DI (except Foundation, see §5.2). **Correction**: the behavior throws `FluentValidation.ValidationException`, not `HRMS.BuildingBlocks.Application.Exceptions.ValidationException` — confirmed from `ValidationBehavior.cs`, which only imports `FluentValidation`/`MediatR` (no `using` for the BuildingBlocks exceptions namespace) and constructs `new ValidationException(failures)` where `failures` is a `List<FluentValidation.Results.ValidationFailure>`; that constructor shape only matches FluentValidation's own exception type (the BuildingBlocks `ValidationException` instead takes `IDictionary<string, string[]>` and cannot accept a `List<ValidationFailure>`). `ExceptionHandlingMiddleware` (§10) happens to map both exception types to HTTP 400, so this has no current runtime effect on the response — but code that catches the BuildingBlocks `ValidationException` type specifically, or reads its `.Errors` as an `IDictionary<string, string[]>`, will **not** catch what the pipeline actually throws. Do not write handling code assuming the pipeline throws the BuildingBlocks type.
- **Audit & soft delete**: centralized in `ApplicationDbContext.SaveChangesAsync` (§6) — never re-implement audit-stamping or delete-interception logic in a handler or repository.
- **CurrentUser**: `IUserContext` (BuildingBlocks) / `CurrentUserContext` (HRMS.API) — this is the *only* source of the acting user's identity for audit and authorization; never read `HttpContext` directly from Application-layer code.
- **Logging**: no structured/centralized logging framework (Serilog, etc.) was found wired into `Program.cs`; only default ASP.NET Core logging is present. **[I]** Treat this as not-yet-standardized; do not introduce a logging library without asking.
- **Transactions**: `IUnitOfWorkTransaction` (BuildingBlocks abstraction, `UnitOfWorkTransaction` impl in `HRMS.Persistence/Transactions/`) exists for explicit multi-repository transactions; `ILeaveBalanceTransaction`/`LeaveBalanceTransaction` is Leave's module-specific transactional service for leave balance operations.

## 11. Testing Approach [V]

Exactly one test project exists in the solution: `tests/HRMS.Modules.Identity.Authorization.Tests`, covering `JwtTokenService`, `PermissionAuthorizationHandler`, `PermissionAuthorizationPolicyProvider`, `PermissionAuthorizeAttribute`, `PermissionCatalogController` authorization, and `PermissionCatalogSynchronizer`. **No other module (Employee, Department, Leave, Attendance, Companies, Foundation) has any automated test coverage.** There is no established project-wide unit/integration test convention beyond what Identity's authorization slice demonstrates — do not assume a testing pattern exists for other modules; if you add tests elsewhere, mirror the structure of `tests/HRMS.Modules.Identity.Authorization.Tests` (one test project, xunit-style, testing concrete infrastructure classes) unless told otherwise.

## 12. Architectural Decisions & Constraints [V/I]

- **[V]** Persistence is intentionally centralized (one `DbContext`, one migration history) rather than per-module — this is the current, load-bearing convention; do not split it into per-module contexts without explicit approval, since it would be a major, cross-cutting change.
- **[V]** Permissions are deliberately excluded from the JWT (per the standing rule in `CLAUDE.md`) — this is a conscious, open architectural decision, not an oversight.
- **[I]** The empty `HRMS.BuildingBlocks/Domain/Events`, `Domain/Rules`, and `Application/Results` folders suggest domain events, domain invariant/rule objects, and a `Result<T>`-style return wrapper were planned but never built. Do not assume these exist; do not build them speculatively — ask first if a task seems to need one.
- **[V]** `HRMS.Application` (top-level, not under `Modules/`) and `Modules/Designation` are both dead/unimplemented scaffolding — not part of the working solution (`HRMS.Application` isn't even in `HRMS.sln`).

## 13. Known Technical Debt (consolidated) [V]

1. `IRepository<T,TKey>`/`Repository<T,TKey>` exist but are unused/unregistered — dead abstraction.
2. Foundation module's MediatR pipeline is missing the validation behavior.
3. Employee's Application namespace root (`HRMS.Application.*`) diverges from every other module (`HRMS.Modules.<M>.Application.*`).
4. AutoMapper profiles are registered broadly but exercised in only a minority of handlers — most mapping is manual and profiles risk silently drifting out of sync with DTOs.
5. Pagination (`PagedResult<T>`) is not used consistently across list queries (Companies/Department/Identity return unpaged lists).
6. `BadRequestException` is not handled by `ExceptionHandlingMiddleware`'s switch and currently surfaces as a 500.
7. Multiple controllers/endpoints ship with missing or commented-out authorization (§9.1) — a real, current access-control gap, not a hypothetical.
8. No automated test coverage outside Identity's authorization slice.
9. No structured logging framework configured.
10. `Program.cs` contains dead, commented-out duplicate DI calls (`AddApplication()`, `AddInfrastructure()`, a second `AddIdentityInfrastructure()`).
11. **[CRITICAL] `HRMS.Modules.Department.Infrastructure.csproj` is never referenced by `HRMS.Persistence.csproj`** — unlike every other module, whose Infrastructure project *is* referenced there (see §3.3). `HRMS.Persistence.csproj` references only `HRMS.Modules.Department.Domain.csproj`. Consequences, verified: `ApplicationDbContext.OnModelCreating` has no `ApplyConfigurationsFromAssembly` call reaching this assembly at all (it structurally cannot — the assembly isn't part of `HRMS.Persistence`'s compiled references, so `DepartmentConfiguration.cs` cannot be referenced by type from `ApplicationDbContext`); `Department` is mapped purely through EF Core conventions with **zero** fluent configuration applied, including `DepartmentConfiguration.cs`'s own `HasQueryFilter(x => !x.IsDeleted)` call (§6), which is defined in source but never loaded. This is a live, currently-shipped defect distinct from (and more severe than) item 5 above — it is not a missing-DI-registration inconsistency, it is an entire module's persistence configuration being unreachable. See `docs/MODULES.md`'s Department section.

These are documented for traceability, per TOGAF change-governance practice — **fixing any of them is a candidate future task requiring explicit user approval, not something to bundle into an unrelated feature change.**
