# HRMS — Module Catalog

See `docs/ARCHITECTURE.md` for the shared architecture pattern all modules follow. This catalog lists each module's responsibility, entities, and status. All information below is **[V] Verified** from source unless marked **[I]** (inferred) or **[R]** (recommendation).

Legend for **Implementation Status**: `Implemented` (CRUD + entities exist), `Partial` (some pieces missing), `Not Implemented` (scaffolding only).

**Not attributed to any module below**: `src/HRMS.API/Controllers/TestController.cs` is a standalone diagnostic/scratch controller, intentionally excluded from every module's controller list. `src/Document/` holds supplementary PDFs and text notes (module technical-documentation exports) and is not part of the buildable solution — see `docs/ARCHITECTURE.md` §2.

---

## Foundation
**Responsibility**: Shared reference/lookup data used by other modules (countries, states, address types, genders, marital statuses, document types, languages, skills, certifications, relationships, identifier types).

- **Projects**: `HRMS.Modules.Foundation.{Domain,Application,Infrastructure}`
- **Entities**: `AddressType`, `Certification`, `Country`, `DocumentType`, `Gender`, `IdentifierType`, `Language`, `MaritalStatus`, `Relationship`, `Skill`, `State` — all simple lookup entities.
- **API Controllers**: `AddressTypesController`, `CountriesController`, `DocumentTypesController`, `GendersController`, `IdentifierTypesController`, `LanguagesController`, `MaritalStatusesController`, `RelationshipsController`, `SkillsController`, `StatesController` (all at `HRMS.API/Controllers/`, not module-namespaced subfolder).
- **Dependencies on other modules**: none (leaf module).
- **Depended on by**: Employee (`Employee.Application` references `Foundation.Domain`).
- **Status**: Implemented (CRUD present). **Caveat**: Foundation's `Application/DependencyInjection/DependencyInjection.cs` does not register the `ValidationBehavior<,>` pipeline step — validators may be registered but not actually invoked (see `docs/ARCHITECTURE.md` §5.2). **No Infrastructure `DependencyInjection.cs`** — persistence config is picked up only via `ApplicationDbContext`'s `ApplyConfigurationsFromAssembly` scan.

---

## Department
**Responsibility**: Organizational department master data.

- **Projects**: `HRMS.Modules.Department.{Domain,Application,Infrastructure}`
- **Entities**: `Department` (single entity module).
- **API Controllers**: `DepartmentController`.
- **Dependencies on other modules**: none (leaf module, Application only references its own Domain).
- **Depended on by**: Employee.
- **Status**: Implemented (CRUD present), but list queries return unpaged `IReadOnlyList<TDto>` rather than `PagedResult<TDto>` — inconsistent with Employee/Leave/Attendance/Foundation. **`DepartmentController` has no authorization attributes at all** — a verified, current access-control gap (see `docs/ARCHITECTURE.md` §9.1).
- **[CRITICAL, verified] Department's Infrastructure/persistence wiring is broken**: `HRMS.Persistence.csproj` — unlike for every other module — never references `HRMS.Modules.Department.Infrastructure.csproj` (only `Department.Domain.csproj`). `DepartmentConfiguration.cs` (including its `HasQueryFilter(x => !x.IsDeleted)` soft-delete filter) is therefore never loaded by `ApplicationDbContext`; the `Department` entity is mapped purely by EF Core conventions with no fluent configuration applied at all. See `docs/ARCHITECTURE.md` §3.3 and §13 item 11. This is a currently-shipped defect, not a documentation gap — do not attempt to fix it as part of an unrelated change; it requires explicit user approval to address.

---

## Employee
**Responsibility**: Core employee master data and all employee-owned sub-records — addresses, contacts, emergency contacts, bank accounts, government identifiers, dependents, nominees, documents, education, experience, skills, languages, certifications, employment type/status lookups.

- **Projects**: `HRMS.Modules.Employee.{Domain,Application,Infrastructure}`
- **Main entities**: `Employee`, `EmployeeAddress`, `EmployeeContact`, `EmergencyContact`, `BankAccount`, `EmployeeGovernmentIdentifier`, `EmployeeDependent`, `EmployeeNominee`, `EmployeeDocument`, `EmployeeEducation`, `EmployeeExperience`, `EmployeeSkill`, `EmployeeLanguage`, `EmployeeCertification`, `EmploymentType`, `EmploymentStatus`.
- **API Controllers**: `EmployeeController` (root) plus module subfolder `Controllers/Employee/`: `BankAccountsController`, `EmergencyContactsController`, `EmployeeAddressesController`, `EmployeeCertificationsController`, `EmployeeContactsController`, `EmployeeDependentsController`, `EmployeeDocumentsController`, `EmployeeEducationsController`, `EmployeeExperiencesController`, `EmployeeLanguagesController`, `EmployeeNomineesController`, `EmployeeSkillsController`, `EmploymentStatusesController`, `EmploymentTypesController`, `GovernmentIdentifiersController`.
- **Dependencies on other modules**: `Foundation.Domain`, `Department.Domain` (an employee references department and various Foundation lookups).
- **Depended on by**: Identity, Leave, Attendance (all reference `Employee.Domain`).
- **Status**: Implemented — the richest module by entity/controller count, and the heaviest real user of AutoMapper (`IMapper` actually used in ~25 handlers, more than any other module). **Caveat (verified defect)**: Application-layer namespaces are rooted at `HRMS.Application.Features.*` instead of the `HRMS.Modules.Employee.Application.Features.*` pattern every other module uses — a naming inconsistency, not an intentional convention. **Caveat (authorization gap)**: `EmployeeController`'s mutating actions (Create/Update/Delete) and its class-level `[Authorize(Roles = "Employee")]` are commented out in source; only `GetAll`/`GetById` are permission-guarded.

---

## Identity
**Responsibility**: Authentication (JWT issuance), users, roles, permissions, and the permission-based authorization system for the whole application. See `docs/ARCHITECTURE.md` §9 for the full authorization flow.

- **Projects**: `HRMS.Modules.Identity.{Domain,Application,Infrastructure}`
- **Entities**: `User`, `Role`, `UserRole` (join), `Permission`, `RolePermission` (join), `Module` (the permission-catalog's module-code lookup, distinct from the solution's project modules).
- **Key Application pieces**: `Application/Authorization/` — `PermissionNames.cs` (centralized permission-code constants), `PermissionCatalog.cs` + `PermissionCatalogEntry.cs` (constant → module-code mapping), `PermissionAuthorizeAttribute.cs`, `IPermissionCatalogSynchronizer.cs`, `PermissionSynchronizationResult.cs`. `Application/Abstractions/Security/` — `IJwtTokenService`, `IPasswordHasher`, `JwtOptions`.
- **Key Infrastructure pieces**: `Infrastructure/Authorization/` — `PermissionAuthorizationPolicyProvider.cs`, `PermissionAuthorizationHandler` (or equivalent requirement handler), `AuthorizationConfiguration.cs` (`AddIdentityAuthorization()`), `PermissionCatalogSynchronizer.cs`. `Infrastructure/Security/` — `JwtTokenService.cs`, password hasher implementation.
- **API Controllers**: `AuthController` (root), and `Controllers/Identity/`: `PermissionCatalogController`, `PermissionController`, `RoleController`, `RolePermissionsController`, `UserController`, `UserRolesController`.
- **Dependencies on other modules**: `Employee.Domain` (a `User` links to an `Employee`).
- **Depended on by**: none currently (top of the dependency graph among the modules examined; Attendance references Companies/Employee/Leave, not Identity).
- **Status**: Implemented, and the only module with automated test coverage (`tests/HRMS.Modules.Identity.Authorization.Tests`). **Caveats (verified access-control gaps)**: `UserController` only guards `ResetPassword`; `PermissionCatalogController` uses role-based `[Authorize(Roles="Admin")]` instead of the module's own `PermissionAuthorize` convention; `RolePermissionsController` has no authorization attributes at all; `PermissionCatalog.Entries` currently only declares Employee/Department/one Identity permission — most of Identity's own endpoints (Role, Permission, UserRoles, RolePermissions controllers) have no permission codes defined yet. Permission checks perform live DB round-trips with **no caching**.

---

## Companies
**Responsibility**: Company master data (the tenant/organization entity referenced by Attendance's company holidays, etc.).

- **Projects**: `HRMS.Modules.Companies.{Domain,Application,Infrastructure}`
- **Entities**: `Company` (single entity module).
- **API Controllers**: `CompaniesController`.
- **Dependencies on other modules**: none (leaf module).
- **Depended on by**: Attendance (`Attendance.Application` references `Companies.Domain`).
- **Status**: Implemented (full Create/Update/Delete/GetById/GetAll CQRS slice, AutoMapper `CompanyProfile`, `CompanyBusinessRules`). List query (`GetCompaniesQuery`) returns unpaged `IReadOnlyList<CompanyDto>`, not `PagedResult<T>` — inconsistent with Employee/Leave/Attendance/Foundation. `DeleteCompanyCommand` has no validator class, unlike most other modules' Delete commands. No Infrastructure `DependencyInjection.cs`.

---

## Leave
**Responsibility**: Leave management — leave years/types/policies/policy rules, per-employee leave entitlements (with carry-forward), leave requests (create/approve/reject/cancel workflow), company holidays, leave day parts.

- **Projects**: `HRMS.Modules.Leave.{Domain,Application,Infrastructure}`
- **Entities**: `LeaveYear`, `LeaveYearStatus`, `LeaveType`, `LeavePolicy`, `LeavePolicyRule`, `LeaveRequestStatus`, `EmployeeLeaveEntitlement`, `LeaveDayPart`, `LeaveRequest`, `CompanyHoliday`.
- **Key Application pieces**: `Application/Abstractions/Persistence/ILeaveBalanceTransaction.cs` (module-specific transactional service: `ApproveLeaveAsync`, `CancelLeaveAsync`), implemented by `HRMS.Persistence.Transactions.LeaveBalanceTransaction`.
- **API Controllers**: `LeaveYearController`, `LeaveYearStatusController`, `LeaveTypeController`, `LeavePolicyController`, `LeavePolicyRuleController`, `LeaveRequestStatusesController`, `LeaveDayPartsController`, `EmployeeLeaveEntitlementController`, and module subfolder `Controllers/Leave/`: `CompanyHolidaysController`, `LeaveRequestsController`.
- **Dependencies on other modules**: `Employee.Domain` (leave requests/entitlements are per-employee).
- **Depended on by**: Attendance (`Attendance.Application` references `Leave.Domain` — likely for holiday/leave-aware attendance day-status calculation).
- **Status**: Implemented — has the richest `LeaveRequestBusinessRules` (a documented 10-step validation pipeline per the Application-layer research) and is the heaviest real user of AutoMapper after Employee (`IMapper` actually used, e.g. in `GetLeaveRequestsQueryHandler`). **Caveat**: places AutoMapper profile classes directly at the feature root (no `Mappings/` subfolder), unlike Employee/Companies/Attendance. Uniquely validates at the query level too (`GetLeaveRequestsQueryValidator`) — not seen elsewhere.

---

## Attendance
**Responsibility**: Attendance tracking — shifts, shift assignments, attendance policies, raw device logs, attendance sources/devices, computed attendance records, day-status computation, and regularization (correction) workflow.

- **Projects**: `HRMS.Modules.Attendance.{Domain,Application,Infrastructure}`
- **Entities**: `AttendanceShift`, `EmployeeShiftAssignment`, `AttendancePolicy`, `AttendanceSource`, `AttendanceDevice`, `AttendanceRawLog`, `AttendanceRecord`, `AttendanceRegularization`, `AttendanceRegularizationStatus`, `AttendanceDayStatus` (this last one, plus its controller, is a currently in-progress/untracked addition per git status).
- **Key Application pieces**: `Application/Services/` — `AttendanceCalculationService` and `AttendanceDayStatusService` (registered directly in `Program.cs`, not via the module's own DI extension — an outlier compared to every other cross-cutting service, which is registered inside the module's `AddXApplication()`/`AddXInfrastructure()` extension methods).
- **API Controllers**: module subfolder `Controllers/Attendance/`: `AttendanceDayStatusesController`, `AttendanceDevicesController`, `AttendancePoliciesController`, `AttendanceRawLogsController`, `AttendanceRecordsController`, `AttendanceRegularizationsController`, `AttendanceShiftsController`, `AttendanceSourcesController`, `EmployeeShiftAssignmentsController`.
- **Dependencies on other modules**: `Companies.Domain`, `Employee.Domain`, `Leave.Domain` — the most cross-module-coupled module in the solution.
- **Depended on by**: none currently.
- **Status**: Implemented but the most actively-in-progress module (per uncommitted git changes to `AttendanceDevice.cs`, several controllers, and the new `AttendanceDayStatuses` feature). **Caveat (authorization gap)**: `AttendanceRecordsController` has no authorization attributes; the permission catalog defines no Attendance permission codes at all yet, so no Attendance endpoint currently participates in the permission system.

---

## Designation — NOT IMPLEMENTED
`src/Modules/Designation/Domain/Entities/` exists as an empty directory with **no tracked files**, and the module is **absent from `HRMS.sln`**. Treat this as a placeholder for future work, not an existing pattern. Do not build against it or assume any Designation entity/API exists without first confirming with the user what, if anything, was intended here.

---

## Dead / Orphaned Projects — do not use

- **`HRMS.Application`** (`src/HRMS.Application/`): contains only a stale `obj/` folder (build artifacts from a deleted project), no `.cs` or `.csproj` files, and is not referenced in `HRMS.sln`. This is leftover from an earlier structure. Never add files here or reference it.

---

## Cross-Module Dependency Summary

```
Foundation, Department, Companies   (leaf modules — no outbound module deps)
        │
        ▼
    Employee   (depends on Foundation, Department)
        │
   ┌────┴────┐
   ▼         ▼
Identity   Leave   (both depend on Employee)
              │
              ▼
        Attendance   (depends on Companies, Employee, Leave)
```

This shows the **Application-layer** Domain dependency graph. Each module's **Infrastructure** project independently carries matching cross-module Domain references too (same directions, no contradictions) — see `docs/ARCHITECTURE.md` §3.2 for the exact list. `docs/ARCHITECTURE.md` §3.3 also documents that `HRMS.API`'s real compiled closure includes Companies and Foundation's Application projects transitively via `HRMS.Persistence`, even though neither is a *direct* dependency shown above or in `HRMS.API.csproj`.

When adding a feature to a module, only depend "downward" in this graph (an existing convention, not a hard-enforced rule at the tooling level) — e.g. it is normal for Attendance to reference Employee/Leave/Companies, but a new Companies or Foundation feature should not start referencing Leave or Attendance. If a task seems to require an "upward" dependency, stop and ask the user rather than adding the project reference.
