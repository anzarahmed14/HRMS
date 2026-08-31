using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.BuildingBlocks.Domain.Entities;
using HRMS.Modules.Attendance.Domain.Entities;
using HRMS.Modules.Attendance.Infrastructure.Configurations;
using HRMS.Modules.Companies.Infrastructure.Configurations;
using HRMS.Modules.Department.Domain.Entities;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Employee.Infrastructure.Configurations;
using HRMS.Modules.Foundation.Domain.Entities;
using HRMS.Modules.Foundation.Infrastructure.Configurations;
using HRMS.Modules.Identity.Domain.Entities;
using HRMS.Modules.Identity.Infrastructure.Configurations;
using HRMS.Modules.Leave.Domain.Entities;
using HRMS.Modules.Leave.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IUserContext _userContext;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IUserContext userContext)
        : base(options)
    {
        _userContext = userContext;
    }

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<EmploymentType> EmploymentTypes => Set<EmploymentType>();

    public DbSet<AddressType> AddressTypes => Set<AddressType>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<State> States => Set<State>();
    public DbSet<EmployeeAddress> EmployeeAddresses => Set<EmployeeAddress>();
    public DbSet<EmploymentStatus> EmploymentStatuses => Set<EmploymentStatus>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<LeaveYear> LeaveYears => Set<LeaveYear>();

    public DbSet<LeaveYearStatus> LeaveYearStatuses => Set<LeaveYearStatus>();

    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

    public DbSet<LeavePolicy> LeavePolicies => Set<LeavePolicy>();

    public DbSet<LeavePolicyRule> LeavePolicyRules => Set<LeavePolicyRule>();

    public DbSet<LeaveRequestStatus> LeaveRequestStatuses
        => Set<LeaveRequestStatus>();

    public DbSet<EmployeeLeaveEntitlement> EmployeeLeaveEntitlements
        => Set<EmployeeLeaveEntitlement>();

    public DbSet<LeaveDayPart> LeaveDayParts
        => Set<LeaveDayPart>();

    public DbSet<LeaveRequest> LeaveRequests
        => Set<LeaveRequest>();

    public DbSet<AttendanceShift> AttendanceShifts
        => Set<AttendanceShift>();

    public DbSet<CompanyHoliday> CompanyHolidays
        => Set<CompanyHoliday>();

    public DbSet<AttendancePolicy> AttendancePolicies
        => Set<AttendancePolicy>();

    public DbSet<EmployeeShiftAssignment> EmployeeShiftAssignments
        => Set<EmployeeShiftAssignment>();

    public DbSet<AttendanceSource> AttendanceSources
        => Set<AttendanceSource>();

    public DbSet<AttendanceDevice> AttendanceDevices
        => Set<AttendanceDevice>();

    public DbSet<AttendanceRawLog> AttendanceRawLogs
        => Set<AttendanceRawLog>();

    public DbSet<AttendanceRecord> AttendanceRecords
        => Set<AttendanceRecord>();

    public DbSet<AttendanceRegularization> AttendanceRegularizations
        => Set<AttendanceRegularization>();

    public DbSet<AttendanceRegularizationStatus> AttendanceRegularizationStatuses
        => Set<AttendanceRegularizationStatus>();

    public DbSet<AttendanceDayStatus> AttendanceDayStatuses
        => Set<AttendanceDayStatus>();

    public DbSet<EmployeeContact> EmployeeContacts => Set<EmployeeContact>();
    public DbSet<Relationship> Relationships => Set<Relationship>();

    public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<IdentifierType> IdentifierTypes => Set<IdentifierType>();

    public DbSet<Gender> Genders => Set<Gender>();
    public DbSet<MaritalStatus> MaritalStatuses => Set<MaritalStatus>();
    public DbSet<EmployeeGovernmentIdentifier> EmployeeGovernmentIdentifiers => Set<EmployeeGovernmentIdentifier>();

    public DbSet<EmployeeDependent> EmployeeDependents => Set<EmployeeDependent>();
    public DbSet<EmployeeNominee> EmployeeNominees => Set<EmployeeNominee>();
    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
    public DbSet<EmployeeDocument> EmployeeDocuments => Set<EmployeeDocument>();
    public DbSet<EmployeeEducation> EmployeeEducations => Set<EmployeeEducation>();

    public DbSet<EmployeeExperience> EmployeeExperiences => Set<EmployeeExperience>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<EmployeeSkill> EmployeeSkills => Set<EmployeeSkill>();
    public DbSet<Certification> Certifications => Set<Certification>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<EmployeeLanguage> EmployeeLanguages => Set<EmployeeLanguage>();
    public DbSet<EmployeeCertification> EmployeeCertifications => Set<EmployeeCertification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Persistence configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        // Employee configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EmployeeConfiguration).Assembly);

        // Identity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RolePermissionConfiguration).Assembly);

        // Company configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CompanyConfiguration).Assembly);

        // Leave configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LeaveYearConfiguration).Assembly);

        // Attendance configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AttendancePolicyConfiguration).Assembly);

        // Foundation configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AddressTypeConfiguration).Assembly);

        modelBuilder.ApplyConfigurationsFromAssembly(
         typeof(EmployeeConfiguration).Assembly);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<AuditableEntity<Guid>>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = _userContext.UserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTimeOffset.UtcNow;
                    entry.Entity.ModifiedBy = _userContext.UserId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;

                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedOn = DateTimeOffset.UtcNow;
                    entry.Entity.DeletedBy = _userContext.UserId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
