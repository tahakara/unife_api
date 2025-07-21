using Domain.Entities.LogEntities;
using Domain.Entities.LogEntities.AcademicModulLogEntities;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.PermissionsLog;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.RolesLog;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.SecurityEventsLog;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.SuspensionsLog;
using Domain.Entities.LogEntities.UniversityModulLogEntities;
using Domain.Entities.MainEntities;
using Domain.Entities.MainEntities.AcademicModulEntities;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Permissions;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Roles;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Suspensions;
using Domain.Entities.MainEntities.UniversityModul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security;

namespace Core.Database.Context
{
    public class UnifeContext : DbContext
    {
        public UnifeContext(DbContextOptions<UnifeContext> options) : base(options)
        {
        }

        public UnifeContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Database connection string is not configured. Please configure the connection string in the DbContext options.");
                //optionsBuilder.UseNpgsql("Server=localhost;Database=unife_fallback;Port=5432;");
            }
        }

        #region DbSets - Lookup Tables
        public DbSet<Region> Regions { get; set; }
        public DbSet<UniversityType> UniversityTypes { get; set; }
        public DbSet<CommunicationCategory> CommunicationCategories { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<AcademicDepartmentType> AcademicDepartmentTypes { get; set; }
        public DbSet<AcademicianTitle> AcademicianTitles { get; set; }
        public DbSet<AuditLogType> AuditLogTypes { get; set; }
        #endregion

        #region DbSets - Main Entity Tables
        public DbSet<University> Universities { get; set; }
        public DbSet<UniversityCommunication> UniversityCommunications { get; set; }
        public DbSet<UniversityAddress> UniversityAddresses { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<AcademicDepartment> AcademicDepartments { get; set; }
        public DbSet<UniversityFacultyDepartment> UniversityFacultyDepartments { get; set; }
        public DbSet<Academician> Academicians { get; set; }
        public DbSet<Rector> Rectors { get; set; }
        #endregion

        #region DbSets - Permission Tables
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<SecurityEventType> SecurityEventTypes { get; set; }
        public DbSet<SecurityEvent> SecurityEvents { get; set; }
        public DbSet<StaffSuspension> StaffSuspensions { get; set; }
        public DbSet<StudentSuspension> StudentSuspensions { get; set; }

        // Junction Tables
        public DbSet<AdminRole> AdminRoles { get; set; }
        public DbSet<StaffRole> StaffRoles { get; set; }
        public DbSet<StudentRole> StudentRoles { get; set; }
        public DbSet<AdminPermission> AdminPermissions { get; set; }
        public DbSet<StaffPermission> StaffPermissions { get; set; }
        public DbSet<StudentPermission> StudentPermissions { get; set; }
        #endregion

        #region DbSets - Audit Log Tables

        #region Main Entity Audit Logs
        public DbSet<UniversityLog> UniversityLogs { get; set; }
        public DbSet<FacultyLog> FacultyLogs { get; set; }
        public DbSet<AcademicianLog> AcademicianLogs { get; set; }
        public DbSet<RectorLog> RectorLogs { get; set; }
        public DbSet<AcademicDepartmentLog> AcademicDepartmentLogs { get; set; }
        public DbSet<UniversityCommunicationLog> UniversityCommunicationLogs { get; set; }
        public DbSet<UniversityAddressLog> UniversityAddressLogs { get; set; }
        public DbSet<UniversityFacultyDepartmentLog> UniversityFacultyDepartmentLogs { get; set; }
        #endregion

        #region Permission Audit Logs
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<StaffLog> StaffLogs { get; set; }
        public DbSet<StudentLog> StudentLogs { get; set; }
        public DbSet<RoleLog> RoleLogs { get; set; }
        public DbSet<PermissionLog> PermissionLogs { get; set; }
        public DbSet<SecurityEventLog> SecurityEventLogs { get; set; }
        public DbSet<StaffSuspensionLog> StaffSuspensionLogs { get; set; }
        public DbSet<StudentSuspensionLog> StudentSuspensionLogs { get; set; }
        public DbSet<AdminRoleLog> AdminRoleLogs { get; set; }
        public DbSet<StaffRoleLog> StaffRoleLogs { get; set; }
        public DbSet<StudentRoleLog> StudentRoleLogs { get; set; }
        public DbSet<AdminPermissionLog> AdminPermissionLogs { get; set; }
        public DbSet<StaffPermissionLog> StaffPermissionLogs { get; set; }
        public DbSet<StudentPermissionLog> StudentPermissionLogs { get; set; }
        public DbSet<SecurityEventTypeLog> SecurityEventTypeLogs { get; set; }
        #endregion
        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PostgreSQL specific configuration
            modelBuilder.HasDefaultSchema("public");

            // Configure lookup tables
            ConfigureRegion(modelBuilder);
            ConfigureUniversityType(modelBuilder);
            ConfigureCommunicationCategory(modelBuilder);
            ConfigureCommunicationType(modelBuilder);
            ConfigureAddressType(modelBuilder);
            ConfigureAcademicDepartmentType(modelBuilder);
            ConfigureAcademicianTitle(modelBuilder);
            ConfigureAuditLogType(modelBuilder);

            // Configure main entity tables
            ConfigureUniversity(modelBuilder);
            ConfigureUniversityCommunication(modelBuilder);
            ConfigureUniversityAddress(modelBuilder);
            ConfigureFaculty(modelBuilder);
            ConfigureAcademicDepartment(modelBuilder);
            ConfigureUniversityFacultyDepartment(modelBuilder);
            ConfigureAcademician(modelBuilder);
            ConfigureRector(modelBuilder);

            // *** ADD MISSING PERMISSION TABLES CONFIGURATION ***
            ConfigureAdmin(modelBuilder);
            ConfigureStaff(modelBuilder);
            ConfigureStudent(modelBuilder);
            ConfigureRole(modelBuilder);
            ConfigurePermission(modelBuilder);
            ConfigureSecurityEventType(modelBuilder);
            ConfigureSecurityEvent(modelBuilder);
            ConfigureStaffSuspension(modelBuilder);
            ConfigureStudentSuspension(modelBuilder);

            // Junction Tables
            ConfigureAdminRole(modelBuilder);
            ConfigureStaffRole(modelBuilder);
            ConfigureStudentRole(modelBuilder);
            ConfigureAdminPermission(modelBuilder);
            ConfigureStaffPermission(modelBuilder);
            ConfigureStudentPermission(modelBuilder);

            // Configure audit log tables
            ConfigureUniversityLog(modelBuilder);
            ConfigureFacultyLog(modelBuilder);
            ConfigureAcademicianLog(modelBuilder);
            ConfigureRectorLog(modelBuilder);
            ConfigureAcademicDepartmentLog(modelBuilder);
            ConfigureUniversityCommunicationLog(modelBuilder);
            ConfigureUniversityAddressLog(modelBuilder);
            ConfigureUniversityFacultyDepartmentLog(modelBuilder);

            // *** PERMISSION LOG CONFIGURATION CALLS ***
            ConfigureAdminLog(modelBuilder);
            ConfigureStaffLog(modelBuilder);
            ConfigureStudentLog(modelBuilder);
            ConfigureRoleLog(modelBuilder);
            ConfigurePermissionLog(modelBuilder);
            ConfigureSecurityEventLog(modelBuilder);
            ConfigureStaffSuspensionLog(modelBuilder);
            ConfigureStudentSuspensionLog(modelBuilder);
            ConfigureSecurityEventTypeLog(modelBuilder);

            // Junction Table Logs
            ConfigureAdminRoleLog(modelBuilder);
            ConfigureStaffRoleLog(modelBuilder);
            ConfigureStudentRoleLog(modelBuilder);
            ConfigureAdminPermissionLog(modelBuilder);
            ConfigureStaffPermissionLog(modelBuilder);
            ConfigureStudentPermissionLog(modelBuilder);
        }

        #region Lookup Tables Configuration

        private void ConfigureRegion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("regions");
                entity.HasKey(e => e.RegionId);
                entity.Property(e => e.RegionId).HasColumnName("region_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.RegionName).HasColumnName("region_name").HasMaxLength(250).IsRequired();
                entity.Property(e => e.RegionCodeAlpha2).HasColumnName("region_code_alpha_2").HasMaxLength(2).IsRequired();
                entity.Property(e => e.RegionCodeAlpha3).HasColumnName("region_code_alpha_3").HasMaxLength(3).IsRequired();
                entity.Property(e => e.RegionCodeNumeric).HasColumnName("region_code_numeric").HasMaxLength(4).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                // Unique constraints
                entity.HasIndex(e => e.RegionCodeAlpha2).IsUnique();
                entity.HasIndex(e => e.RegionCodeAlpha3).IsUnique();
                entity.HasIndex(e => e.RegionCodeNumeric).IsUnique();
            });
        }

        private void ConfigureUniversityType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityType>(entity =>
            {
                entity.ToTable("university_types");
                entity.HasKey(e => e.TypeId);
                entity.Property(e => e.TypeId).HasColumnName("type_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.TypeName).HasColumnName("type_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.TypeDescription).HasColumnName("type_description").HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => e.TypeName).IsUnique();
            });
        }

        private void ConfigureCommunicationCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunicationCategory>(entity =>
            {
                entity.ToTable("communication_categories");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).HasColumnName("category_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.CategoryName).HasColumnName("category_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.CategoryDescription).HasColumnName("category_description").HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => e.CategoryName).IsUnique();
            });
        }

        private void ConfigureCommunicationType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunicationType>(entity =>
            {
                entity.ToTable("communication_types");
                entity.HasKey(e => e.TypeId);
                entity.Property(e => e.TypeId).HasColumnName("type_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.CategoryId).HasColumnName("category_id").IsRequired();
                entity.Property(e => e.TypeName).HasColumnName("type_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.TypeDescription).HasColumnName("type_description").HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => new { e.CategoryId, e.TypeName }).IsUnique();

                // Foreign key
                entity.HasOne(ct => ct.Category).WithMany(ct => ct.CommunicationTypes).HasForeignKey(ct => ct.CategoryId).HasConstraintName("fk_communication_types_category");
            });
        }

        private void ConfigureAddressType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.ToTable("address_types");
                entity.HasKey(e => e.TypeId);
                entity.Property(e => e.TypeId).HasColumnName("type_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.TypeName).HasColumnName("type_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.TypeDescription).HasColumnName("type_description").HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => e.TypeName).IsUnique();
            });
        }

        private void ConfigureAcademicDepartmentType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicDepartmentType>(entity =>
            {
                entity.ToTable("academic_department_types");
                entity.HasKey(e => e.TypeId);
                entity.Property(e => e.TypeId).HasColumnName("type_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.TypeShortName).HasColumnName("type_short_name").HasMaxLength(50).IsRequired();
                entity.Property(e => e.TypeName).HasColumnName("type_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.TypeDescription).HasColumnName("type_description").HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => e.TypeShortName).IsUnique();
                entity.HasIndex(e => e.TypeName).IsUnique();
            });
        }

        private void ConfigureAcademicianTitle(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicianTitle>(entity =>
            {
                entity.ToTable("academician_titles");
                entity.HasKey(e => e.TitleId);
                entity.Property(e => e.TitleId).HasColumnName("title_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.TitleShortName).HasColumnName("title_short_name").HasMaxLength(50).IsRequired();
                entity.Property(e => e.TitleName).HasColumnName("title_name").HasMaxLength(200).IsRequired();
                entity.Property(e => e.TitleDescription).HasColumnName("title_description").HasColumnType("text");
                entity.Property(e => e.TitleOrder).HasColumnName("title_order").HasDefaultValue(0).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => e.TitleShortName).IsUnique();
                entity.HasIndex(e => e.TitleName).IsUnique();
            });
        }

        private void ConfigureAuditLogType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLogType>(entity =>
            {
                entity.ToTable("audit_log_types");
                entity.HasKey(e => e.LogTypeId);
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeName).HasColumnName("log_type_name").HasMaxLength(50).IsRequired();
                entity.Property(e => e.LogTypeDescription).HasColumnName("log_type_description").HasColumnType("text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => e.LogTypeName).IsUnique();
            });
        }

        #endregion

        #region Main Entity Tables Configuration

        private void ConfigureUniversity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("universities");
                entity.HasKey(e => e.UniversityUuid);
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityName).HasColumnName("university_name").HasMaxLength(250).IsRequired();
                entity.Property(e => e.UniversityCode).HasColumnName("university_code").HasMaxLength(50);
                entity.Property(e => e.RegionId).HasColumnName("region_id");
                entity.Property(e => e.UniversityTypeId).HasColumnName("university_type_id");

                // University Main Address FK - Many-to-One ilişki
                entity.Property(e => e.AddressUuid).HasColumnName("address_uuid");

                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.EstablishedYear).HasColumnName("established_year");
                entity.Property(e => e.WebsiteUrl).HasColumnName("website_url").HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.UniversityCode).IsUnique().HasDatabaseName("idx_universities_code");
                entity.HasIndex(e => e.UniversityName).HasDatabaseName("idx_universities_name");
                entity.HasIndex(e => e.RegionId).HasDatabaseName("idx_universities_region");
                entity.HasIndex(e => e.UniversityTypeId).HasDatabaseName("idx_universities_type");
                entity.HasIndex(e => e.AddressUuid).HasDatabaseName("idx_universities_address"); // Yeni index
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_universities_active").HasFilter("is_active = true");

                // Foreign keys - Lookup relationships
                entity.HasOne(u => u.Region)
                    .WithMany(r => r.Universities)
                    .HasForeignKey(u => u.RegionId)
                    .HasConstraintName("fk_universities_region");

                entity.HasOne(u => u.UniversityType)
                    .WithMany(ut => ut.Universities)
                    .HasForeignKey(u => u.UniversityTypeId)
                    .HasConstraintName("fk_universities_type");

                // *** DÜZELTME: University -> UniversityAddress Many-to-One İlişki ***
                // Birden fazla üniversite aynı ana adresi kullanabilir
                entity.HasOne(u => u.MainAddress)
                      .WithMany(ua => ua.UniversitiesAsMainAddress)
                      .HasForeignKey(u => u.AddressUuid)
                      .HasConstraintName("fk_universities_main_address")
                      .OnDelete(DeleteBehavior.SetNull);

                // *** ONE-TO-ONE RELATIONSHIP: University -> Rector ***
                entity.HasOne(u => u.Rector)
                      .WithOne(r => r.University)
                      .HasForeignKey<Rector>(r => r.UniversityUuid)
                      .HasConstraintName("fk_rectors_university")
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureUniversityCommunication(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityCommunication>(entity =>
            {
                entity.ToTable("university_communications");
                entity.HasKey(e => e.CommunicationUuid);
                entity.Property(e => e.CommunicationUuid).HasColumnName("communication_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").IsRequired();
                entity.Property(e => e.TypeId).HasColumnName("type_id").IsRequired();
                entity.Property(e => e.CommunicationValue).HasColumnName("communication_value").HasMaxLength(500).IsRequired();
                entity.Property(e => e.Priority).HasColumnName("priority").HasDefaultValue(0);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                // Indexes
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_university_communications_university");
                entity.HasIndex(e => e.TypeId).HasDatabaseName("idx_university_communications_type");
                // Foreign keys
                entity.HasOne(uc => uc.University).WithMany(ct => ct.UniversityCommunications).HasForeignKey(uc => uc.UniversityUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_university_communications_university");
                entity.HasOne(uc => uc.Type).WithMany(ct => ct.UniversityCommunications).HasForeignKey(uc => uc.TypeId).HasConstraintName("fk_university_communications_type");
            });
        }

        private void ConfigureUniversityAddress(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityAddress>(entity =>
            {
                entity.ToTable("university_addresses");
                entity.HasKey(e => e.AddressUuid);
                entity.Property(e => e.AddressUuid).HasColumnName("address_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").IsRequired();
                entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id").IsRequired();
                entity.Property(e => e.AddressTitle).HasColumnName("address_title").HasMaxLength(100);
                entity.Property(e => e.AddressLine1).HasColumnName("address_line_1").HasColumnType("text").IsRequired();
                entity.Property(e => e.AddressLine2).HasColumnName("address_line_2").HasColumnType("text");
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
                entity.Property(e => e.StateProvince).HasColumnName("state_province").HasMaxLength(100);
                entity.Property(e => e.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
                entity.Property(e => e.Country).HasColumnName("country").HasMaxLength(100);
                entity.Property(e => e.Latitude).HasColumnName("latitude").HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitude).HasColumnName("longitude").HasColumnType("decimal(11,8)");
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.Priority).HasColumnName("priority").HasDefaultValue(0);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_university_addresses_university");
                entity.HasIndex(e => e.AddressTypeId).HasDatabaseName("idx_university_addresses_type");
                entity.HasIndex(e => e.City).HasDatabaseName("idx_university_addresses_city");

                // Foreign keys
                entity.HasOne(ua => ua.University)
                    .WithMany(u => u.UniversityAddresses)
                    .HasForeignKey(ua => ua.UniversityUuid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_university_addresses_university");

                entity.HasOne(ua => ua.AddressType)
                    .WithMany(at => at.UniversityAddresses)
                    .HasForeignKey(ua => ua.AddressTypeId)
                    .HasConstraintName("fk_university_addresses_type");
            });
        }

        private void ConfigureFaculty(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("faculties");
                entity.HasKey(e => e.FacultyUuid);
                entity.Property(e => e.FacultyUuid).HasColumnName("faculty_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").IsRequired();
                entity.Property(e => e.FacultyName).HasColumnName("faculty_name").HasMaxLength(200).IsRequired();
                entity.Property(e => e.FacultyCode).HasColumnName("faculty_code").HasMaxLength(50);
                entity.Property(e => e.FacultyDescription).HasColumnName("faculty_description").HasColumnType("text");
                entity.Property(e => e.DeanUuid).HasColumnName("dean_uuid");
                entity.Property(e => e.AddressUuid).HasColumnName("address_uuid");
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_faculties_university");
                entity.HasIndex(e => e.FacultyName).HasDatabaseName("idx_faculties_name");
                entity.HasIndex(e => e.DeanUuid).HasDatabaseName("idx_faculties_dean");
                entity.HasIndex(e => e.AddressUuid).HasDatabaseName("idx_faculties_address");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_faculties_active").HasFilter("is_active = true");

                // Foreign keys
                entity.HasOne(f => f.University)
                    .WithMany(u => u.Faculties) // Corrected to reference the collection of Faculties
                    .HasForeignKey(f => f.UniversityUuid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_faculties_university");

                // Dean relationship
                entity.HasOne(f => f.Dean)
                      .WithMany(a => a.DeanOfFaculties)
                      .HasForeignKey(f => f.DeanUuid)
                      .HasConstraintName("fk_faculties_dean")
                      .OnDelete(DeleteBehavior.SetNull);

                // Address relationship
                entity.HasOne(f => f.Address)
                      .WithMany(ua => ua.Faculties)
                      .HasForeignKey(f => f.AddressUuid)
                      .HasConstraintName("fk_faculties_address")
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void ConfigureAcademicDepartment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicDepartment>(entity =>
            {
                entity.ToTable("academic_departments");
                entity.HasKey(e => e.DepartmentUuid);
                entity.Property(e => e.DepartmentUuid).HasColumnName("department_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.DepartmentTypeId).HasColumnName("department_type_id").IsRequired();
                entity.Property(e => e.DepartmentName).HasColumnName("department_name").HasMaxLength(200).IsRequired();
                entity.Property(e => e.DepartmentCode).HasColumnName("department_code").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DepartmentDescription).HasColumnName("department_description").HasColumnType("text");
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                entity.HasIndex(e => e.DepartmentCode).IsUnique();
                // Foreign keys
                entity.HasOne(ad => ad.DepartmentType).WithMany(ad => ad.AcademicDepartments).HasForeignKey(ad => ad.DepartmentTypeId).HasConstraintName("fk_academic_departments_type");
            });
        }

        private void ConfigureUniversityFacultyDepartment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityFacultyDepartment>(entity =>
            {
                entity.ToTable("university_faculty_departments");
                entity.HasKey(e => e.UniversityDepartmentUuid);
                entity.Property(e => e.UniversityDepartmentUuid).HasColumnName("university_department_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").IsRequired();
                entity.Property(e => e.FacultyUuid).HasColumnName("faculty_uuid").IsRequired();
                entity.Property(e => e.DepartmentUuid).HasColumnName("department_uuid").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");
                // Indexes
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_ufd_university");
                entity.HasIndex(e => e.FacultyUuid).HasDatabaseName("idx_ufd_faculty");
                entity.HasIndex(e => e.DepartmentUuid).HasDatabaseName("idx_ufd_department");
                entity.HasIndex(e => new { e.UniversityUuid, e.FacultyUuid, e.DepartmentUuid }).HasDatabaseName("idx_ufd_composite");
                // Foreign keys
                entity.HasOne(ufd => ufd.University).WithMany(f => f.UniversityFacultyDepartments).HasForeignKey(ufd => ufd.UniversityUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_ufd_university");
                entity.HasOne(ufd => ufd.Faculty).WithMany(f => f.UniversityFacultyDepartments).HasForeignKey(ufd => ufd.FacultyUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_ufd_faculty");
                entity.HasOne(ufd => ufd.Department).WithMany(f => f.UniversityFacultyDepartments).HasForeignKey(ufd => ufd.DepartmentUuid).HasConstraintName("fk_ufd_department");
            });
        }

        private void ConfigureAcademician(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Academician>(entity =>
            {
                entity.ToTable("academicians");

                // Primary Key
                entity.HasKey(e => e.AcademicianUuid);

                // Properties
                entity.Property(e => e.AcademicianUuid)
                      .HasColumnName("academician_uuid")
                      .HasColumnType("uuid")
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.FacultyUuid).HasColumnName("faculty_uuid");
                entity.Property(e => e.DepartmentUuid).HasColumnName("department_uuid");
                entity.Property(e => e.TitleId).HasColumnName("title_id");

                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(5);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.PersonalWebsiteUrl).HasColumnName("personal_website_url").HasMaxLength(500);
                entity.Property(e => e.OfficeAddress).HasColumnName("office_address").HasColumnType("text");
                entity.Property(e => e.Bio).HasColumnName("bio").HasColumnType("text");

                entity.Property(e => e.YokId).HasColumnName("yok_id").HasMaxLength(100);
                entity.Property(e => e.GoogleScholarId).HasColumnName("google_scholar_id").HasMaxLength(100);
                entity.Property(e => e.ScopusId).HasColumnName("scopus_id").HasMaxLength(100);
                entity.Property(e => e.WebOfScienceId).HasColumnName("web_of_science_id").HasMaxLength(100);
                entity.Property(e => e.OrcidId).HasColumnName("orcid_id").HasMaxLength(100);
                entity.Property(e => e.ResearchGateId).HasColumnName("research_gate_id").HasMaxLength(100);
                entity.Property(e => e.LinkedinUsername).HasColumnName("linkedin_username").HasMaxLength(100);
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);

                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_academicians_university");
                entity.HasIndex(e => e.FacultyUuid).HasDatabaseName("idx_academicians_faculty");
                entity.HasIndex(e => e.DepartmentUuid).HasDatabaseName("idx_academicians_department");
                entity.HasIndex(e => e.TitleId).HasDatabaseName("idx_academicians_title");
                entity.HasIndex(e => new { e.FirstName, e.LastName }).HasDatabaseName("idx_academicians_name");
                entity.HasIndex(e => e.YokId).HasDatabaseName("idx_academicians_yok_id").HasFilter("yok_id IS NOT NULL");
                entity.HasIndex(e => e.GoogleScholarId).HasDatabaseName("idx_academicians_google_scholar").HasFilter("google_scholar_id IS NOT NULL");
                entity.HasIndex(e => e.OrcidId).HasDatabaseName("idx_academicians_orcid").HasFilter("orcid_id IS NOT NULL");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_academicians_active").HasFilter("is_active = true");

                // Unique constraints for academic identifiers
                entity.HasIndex(e => e.YokId).IsUnique();
                entity.HasIndex(e => e.GoogleScholarId).IsUnique();
                entity.HasIndex(e => e.ScopusId).IsUnique();
                entity.HasIndex(e => e.WebOfScienceId).IsUnique();
                entity.HasIndex(e => e.OrcidId).IsUnique();
                entity.HasIndex(e => e.ResearchGateId).IsUnique();
                entity.HasIndex(e => e.LinkedinUsername).IsUnique();

                // Foreign key relationships
                entity.HasOne(a => a.University)
                      .WithMany(u => u.Academicians) // Corrected to reference the collection of Academicians
                      .HasForeignKey(a => a.UniversityUuid)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("fk_academicians_university");

                entity.HasOne(a => a.Faculty)
                      .WithMany(f => f.Academicians)
                      .HasForeignKey(a => a.FacultyUuid)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("fk_academicians_faculty");

                entity.HasOne(a => a.Department)
                      .WithMany(u => u.Academicians)
                      .HasForeignKey(a => a.DepartmentUuid)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("fk_academicians_department");

                entity.HasOne(a => a.Title)
                      .WithMany(t => t.Academicians)
                      .HasForeignKey(a => a.TitleId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("fk_academicians_title");

                // Dean relationship (one-to-many)
                entity.HasMany(a => a.DeanOfFaculties)
                      .WithOne(f => f.Dean)
                      .HasForeignKey(f => f.DeanUuid)
                      .HasConstraintName("fk_faculties_dean")
                      .OnDelete(DeleteBehavior.SetNull);

                // One-to-One relationship: Academician -> Rector
                entity.HasOne(a => a.Rector)
                      .WithOne(r => r.Academician)
                      .HasForeignKey<Rector>(r => r.AcademicianUuid)
                      .HasConstraintName("fk_rectors_academician")
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }


        private void ConfigureRector(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rector>(entity =>
            {
                entity.ToTable("rectors");
                entity.HasKey(e => e.RectorUuid);
                entity.Property(e => e.RectorUuid).HasColumnName("rector_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").IsRequired();
                entity.Property(e => e.AcademicianUuid).HasColumnName("academician_uuid").IsRequired();
                entity.Property(e => e.StartDate).HasColumnName("start_date").HasColumnType("date").HasDefaultValueSql("CURRENT_DATE").IsRequired();
                entity.Property(e => e.EndDate).HasColumnName("end_date").HasColumnType("date");
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.UniversityUuid).IsUnique().HasDatabaseName("idx_rectors_university");
                entity.HasIndex(e => e.AcademicianUuid).HasDatabaseName("idx_rectors_academician");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_rectors_active").HasFilter("is_active = true");

                // *** ONE-TO-ONE RELATIONSHIP: Rector -> University ***
                entity.HasOne(r => r.University)
                      .WithOne(u => u.Rector)
                      .HasForeignKey<Rector>(r => r.UniversityUuid)
                      .HasConstraintName("fk_rectors_university")
                      .OnDelete(DeleteBehavior.Cascade);

                // *** ONE-TO-ONE RELATIONSHIP: Rector -> Academician ***
                entity.HasOne(r => r.Academician)
                      .WithOne(a => a.Rector)
                      .HasForeignKey<Rector>(r => r.AcademicianUuid)
                      .HasConstraintName("fk_rectors_academician")
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        #endregion

        #region Permission Tables Configuration


        private static void ConfigureAdmin(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admins");
                entity.HasKey(e => e.AdminUuid);

                entity.Property(e => e.AdminUuid).HasColumnName("admin_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified").HasDefaultValue(false);
                entity.Property(e => e.IsPhoneNumberVerified).HasColumnName("is_phone_number_verified").HasDefaultValue(false);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt").IsRequired();
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(false);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.IsSystemPermission).HasColumnName("is_system_permission").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.Email).HasDatabaseName("idx_admins_email");
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_admins_university");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_admins_active").HasFilter("is_active = true");
                entity.HasIndex(e => e.IsDeleted).HasDatabaseName("idx_admins_deleted").HasFilter("is_deleted = false");

                // Foreign keys
                entity.HasOne(a => a.University).WithOne(u => u.Admin).HasForeignKey<Admin>(a => a.UniversityUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_admins_university");
            });
        }

        private static void ConfigureStaff(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("staff");
                entity.HasKey(e => e.StaffUuid);

                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified").HasDefaultValue(false);
                entity.Property(e => e.IsPhoneNumberVerified).HasColumnName("is_phone_number_verified").HasDefaultValue(false);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt").IsRequired();
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(false);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.Email).HasDatabaseName("idx_staff_email");
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_staff_university");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_staff_active").HasFilter("is_active = true");

                // Foreign keys
                entity.HasOne(s => s.University).WithMany(u => u.StaffMembers).HasForeignKey(s => s.UniversityUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_staff_university");
            });
        }

        private static void ConfigureStudent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students");
                entity.HasKey(e => e.StudentUuid);

                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified").HasDefaultValue(false);
                entity.Property(e => e.IsPhoneNumberVerified).HasColumnName("is_phone_number_verified").HasDefaultValue(false);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt").IsRequired();
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(false);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.Email).HasDatabaseName("idx_students_email");
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_students_university");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_students_active").HasFilter("is_active = true");
                entity.HasIndex(e => e.IsDeleted).HasDatabaseName("idx_students_deleted").HasFilter("is_deleted = false");

                // Foreign keys
                entity.HasOne(s => s.University).WithMany(u => u.Students).HasForeignKey(s => s.UniversityUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_students_university");
            });
        }

        private static void ConfigureRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.RoleUuid);

                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.IsEditable).HasColumnName("is_editable").HasDefaultValue(true);
                entity.Property(e => e.IsSystemRole).HasColumnName("is_system_role").HasDefaultValue(false);
                entity.Property(e => e.IsAssignedToUniversity).HasColumnName("is_assigned_to_university").HasDefaultValue(false);
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorAdminUuid).HasColumnName("creator_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStaffUuid).HasColumnName("creator_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStudentUuid).HasColumnName("creator_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.Name).HasDatabaseName("idx_roles_name");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_roles_active").HasFilter("is_active = true");
                entity.HasIndex(e => e.IsDeleted).HasDatabaseName("idx_roles_deleted").HasFilter("is_deleted = false");
                entity.HasIndex(e => e.IsSystemRole).HasDatabaseName("idx_roles_system").HasFilter("is_system_role = true");

                // Foreign keys
                entity.HasOne(r => r.University).WithMany(u => u.Roles)
                    .HasForeignKey(r => r.UniversityUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_roles_university");
                entity.HasOne(r => r.CreatorAdmin).WithMany(a => a.CreatedRoles)
                    .HasForeignKey(r => r.CreatorAdminUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_roles_creator_admin");
                entity.HasOne(r => r.CreatorStaff).WithMany(s => s.CreatedRoles)
                    .HasForeignKey(r => r.CreatorStaffUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_roles_creator_staff");
                entity.HasOne(r => r.CreatorStudent).WithMany(st => st.CreatedRoles)
                    .HasForeignKey(r => r.CreatorStudentUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_roles_creator_student");
            });
        }

        private static void ConfigurePermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");
                entity.HasKey(e => e.PermissionUuid);

                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.IsEditable).HasColumnName("is_editable").HasDefaultValue(true);
                entity.Property(e => e.IsSystemPermission).HasColumnName("is_system_permission").HasDefaultValue(false);
                entity.Property(e => e.IsAssignedToUniversity).HasColumnName("is_assigned_to_university").HasDefaultValue(false);
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorAdminUuid).HasColumnName("creator_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStaffUuid).HasColumnName("creator_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStudentUuid).HasColumnName("creator_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.Name).HasDatabaseName("idx_permissions_name");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_permissions_active").HasFilter("is_active = true");
                entity.HasIndex(e => e.IsDeleted).HasDatabaseName("idx_permissions_deleted").HasFilter("is_deleted = false");
                entity.HasIndex(e => e.IsSystemPermission).HasDatabaseName("idx_permissions_system").HasFilter("is_system_permission = true");

                // Foreign keys
                entity.HasOne(p => p.University).WithMany(u => u.Permissions)
                    .HasForeignKey(p => p.UniversityUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_permissions_university");
                entity.HasOne(p => p.CreatorAdmin)
                    .WithMany(a => a.CreatedPermissions)
                    .HasForeignKey(p => p.CreatorAdminUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_permissions_creator_admin");
                entity.HasOne(p => p.CreatorStaff)
                    .WithMany(s => s.CreatedPermissions)
                    .HasForeignKey(p => p.CreatorStaffUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_permissions_creator_staff");
                entity.HasOne(p => p.CreatorStudent).WithMany(st => st.CreatedPermissions)
                    .HasForeignKey(p => p.CreatorStudentUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_permissions_creator_student");
            });
        }

        private static void ConfigureSecurityEventType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityEventType>(entity =>
            {
                entity.ToTable("security_event_types");
                entity.HasKey(e => e.SecurityEventTypeUuid);

                entity.Property(e => e.SecurityEventTypeUuid).HasColumnName("security_event_type_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.IsSystemEvent).HasColumnName("is_system_event").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.Name).HasDatabaseName("idx_security_event_types_name");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("idx_security_event_types_active").HasFilter("is_active = true");
                entity.HasIndex(e => e.IsSystemEvent).HasDatabaseName("idx_security_event_types_system").HasFilter("is_system_event = true");
            });
        }

        private static void ConfigureSecurityEvent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityEvent>(entity =>
            {
                entity.ToTable("security_events");
                entity.HasKey(e => e.SecurityEventUuid);

                entity.Property(e => e.SecurityEventUuid).HasColumnName("security_event_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.EventTypeUuid).HasColumnName("event_type_uuid").HasColumnType("uuid");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.EventedByAdminUuid).HasColumnName("evented_by_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.EventedByStaffUuid).HasColumnName("evented_by_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.EventedByStudentUuid).HasColumnName("evented_by_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(1000).IsRequired();
                entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(45).IsRequired();
                entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
                entity.Property(e => e.AdditionalData).HasColumnName("additional_data").HasColumnType("jsonb");
                entity.Property(e => e.EventTime).HasColumnName("event_time").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.EventTypeUuid).HasDatabaseName("idx_security_events_type");
                entity.HasIndex(e => e.EventedByAdminUuid).HasDatabaseName("idx_security_events_admin");
                entity.HasIndex(e => e.EventedByStaffUuid).HasDatabaseName("idx_security_events_staff");
                entity.HasIndex(e => e.EventedByStudentUuid).HasDatabaseName("idx_security_events_student");
                entity.HasIndex(e => e.EventTime).HasDatabaseName("idx_security_events_time");
                entity.HasIndex(e => e.IpAddress).HasDatabaseName("idx_security_events_ip");

                // Foreign keys
                entity.HasOne(se => se.EventType).WithMany(set => set.SecurityEvents).HasForeignKey(se => se.EventTypeUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_security_events_type");
                entity.HasOne(se => se.University).WithMany(u => u.SecurityEvents).HasForeignKey(se => se.UniversityUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_security_events_university");
                entity.HasOne(se => se.EventedByAdmin).WithMany(a => a.SecurityEvents).HasForeignKey(se => se.EventedByAdminUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_security_events_admin");
                entity.HasOne(se => se.EventedByStaff).WithMany(s => s.SecurityEvents).HasForeignKey(se => se.EventedByStaffUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_security_events_staff");
                entity.HasOne(se => se.EventedByStudent).WithMany(st => st.SecurityEvents).HasForeignKey(se => se.EventedByStudentUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_security_events_student");
            });
        }

        private static void ConfigureStaffSuspension(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffSuspension>(entity =>
            {
                entity.ToTable("staff_suspensions");
                entity.HasKey(e => e.StaffSuspensionUuid);

                entity.Property(e => e.StaffSuspensionUuid).HasColumnName("staff_suspension_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.SuspendedByAdminUuid).HasColumnName("suspended_by_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspendedByStaffUuid).HasColumnName("suspended_by_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspensionStartDate).HasColumnName("suspension_start_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.SuspensionEndDate).HasColumnName("suspension_end_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.Reason).HasColumnName("reason").HasMaxLength(1000).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.StaffUuid).HasDatabaseName("idx_staff_suspensions_staff");
                entity.HasIndex(e => e.SuspendedByAdminUuid).HasDatabaseName("idx_staff_suspensions_admin");
                entity.HasIndex(e => e.SuspendedByStaffUuid).HasDatabaseName("idx_staff_suspensions_staff_by");
                entity.HasIndex(e => e.SuspensionStartDate).HasDatabaseName("idx_staff_suspensions_start");
                entity.HasIndex(e => e.SuspensionEndDate).HasDatabaseName("idx_staff_suspensions_end");

                // Foreign keys
                entity.HasOne(ss => ss.Staff).WithMany(s => s.StaffSuspensions).HasForeignKey(ss => ss.StaffUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_staff_suspensions_staff");
                entity.HasOne(ss => ss.SuspendedByAdmin).WithMany(a => a.SuspendedStaffSuspensions).HasForeignKey(ss => ss.SuspendedByAdminUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_staff_suspensions_admin");
                entity.HasOne(ss => ss.SuspendedByStaff).WithMany(s => s.SuspendedStaffSuspensions).HasForeignKey(ss => ss.SuspendedByStaffUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_staff_suspensions_staff_by");
            });
        }

        private static void ConfigureStudentSuspension(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentSuspension>(entity =>
            {
                entity.ToTable("student_suspensions");
                entity.HasKey(e => e.StudentSuspensionUuid);

                entity.Property(e => e.StudentSuspensionUuid).HasColumnName("student_suspension_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.SuspendedByAdminUuid).HasColumnName("suspended_by_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspendedByStaffUuid).HasColumnName("suspended_by_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspendedByStudentUuid).HasColumnName("suspended_by_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspensionStartDate).HasColumnName("suspension_start_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.SuspensionEndDate).HasColumnName("suspension_end_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.Reason).HasColumnName("reason").HasMaxLength(1000).IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Indexes
                entity.HasIndex(e => e.StudentUuid).HasDatabaseName("idx_student_suspensions_student");
                entity.HasIndex(e => e.SuspendedByAdminUuid).HasDatabaseName("idx_student_suspensions_admin");
                entity.HasIndex(e => e.SuspendedByStaffUuid).HasDatabaseName("idx_student_suspensions_staff");
                entity.HasIndex(e => e.SuspendedByStudentUuid).HasDatabaseName("idx_student_suspensions_student_by");
                entity.HasIndex(e => e.SuspensionStartDate).HasDatabaseName("idx_student_suspensions_start");
                entity.HasIndex(e => e.SuspensionEndDate).HasDatabaseName("idx_student_suspensions_end");

                // Foreign keys
                entity.HasOne(ss => ss.Student).WithMany(s => s.StudentSuspensions).HasForeignKey(ss => ss.StudentUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_student_suspensions_student");
                entity.HasOne(ss => ss.SuspendedByAdmin).WithMany(a => a.SuspendedStudentSuspensions).HasForeignKey(ss => ss.SuspendedByAdminUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_student_suspensions_admin");
                entity.HasOne(ss => ss.SuspendedByStaff).WithMany(s => s.SuspendedStudentSuspensions).HasForeignKey(ss => ss.SuspendedByStaffUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_student_suspensions_staff");
                entity.HasOne(ss => ss.SuspendedByStudent).WithMany(s => s.SuspendedStudentSuspensions).HasForeignKey(ss => ss.SuspendedByStudentUuid).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_student_suspensions_student_by");
            });
        }

        // Junction Tables
        private static void ConfigureAdminRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRole>(entity =>
            {
                entity.ToTable("admin_roles");
                entity.HasKey(e => e.AdminRoleUuid);

                entity.Property(e => e.AdminRoleUuid).HasColumnName("admin_role_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.AdminUuid).HasColumnName("admin_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Unique constraint
                entity.HasIndex(e => new { e.AdminUuid, e.RoleUuid }).IsUnique();

                // Indexes
                entity.HasIndex(e => e.AdminUuid).HasDatabaseName("idx_admin_roles_admin");
                entity.HasIndex(e => e.RoleUuid).HasDatabaseName("idx_admin_roles_role");

                // Foreign keys - Configure with navigation properties
                entity.HasOne(ar => ar.Admin).WithMany(a => a.AdminRoles).HasForeignKey(ar => ar.AdminUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_admin_roles_admin");
                entity.HasOne(ar => ar.Role).WithMany(r => r.AdminRoles).HasForeignKey(ar => ar.RoleUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_admin_roles_role");
            });
        }

        private static void ConfigureStaffRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffRole>(entity =>
            {
                entity.ToTable("staff_roles");
                entity.HasKey(e => e.StaffRoleUuid);

                entity.Property(e => e.StaffRoleUuid).HasColumnName("staff_role_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Unique constraint
                entity.HasIndex(e => new { e.StaffUuid, e.RoleUuid }).IsUnique();

                // Indexes
                entity.HasIndex(e => e.StaffUuid).HasDatabaseName("idx_staff_roles_staff");
                entity.HasIndex(e => e.RoleUuid).HasDatabaseName("idx_staff_roles_role");

                // Foreign keys
                entity.HasOne(sr => sr.Staff).WithMany(s => s.StaffRoles).HasForeignKey(sr => sr.StaffUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_staff_roles_staff");
                entity.HasOne(sr => sr.Role).WithMany(r => r.StaffRoles).HasForeignKey(sr => sr.RoleUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_staff_roles_role");
            });
        }

        private static void ConfigureStudentRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRole>(entity =>
            {
                entity.ToTable("student_roles");
                entity.HasKey(e => e.StudentRoleUuid);

                entity.Property(e => e.StudentRoleUuid).HasColumnName("student_role_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Unique constraint
                entity.HasIndex(e => new { e.StudentUuid, e.RoleUuid }).IsUnique();

                // Indexes
                entity.HasIndex(e => e.StudentUuid).HasDatabaseName("idx_student_roles_student");
                entity.HasIndex(e => e.RoleUuid).HasDatabaseName("idx_student_roles_role");

                // Foreign keys
                entity.HasOne(sr => sr.Student).WithMany(s => s.StudentRoles).HasForeignKey(sr => sr.StudentUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_student_roles_student");
                entity.HasOne(sr => sr.Role).WithMany(r => r.StudentRoles).HasForeignKey(sr => sr.RoleUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_student_roles_role");
            });
        }

        private static void ConfigureAdminPermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminPermission>(entity =>
            {
                entity.ToTable("admin_permissions");
                entity.HasKey(e => e.AdminPermissionUuid);

                entity.Property(e => e.AdminPermissionUuid).HasColumnName("admin_permission_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.AdminUuid).HasColumnName("admin_uuid").HasColumnType("uuid").IsRequired(); // Explicitly map to admin_uuid
                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").IsRequired(); // Explicitly map to permission_uuid
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Unique constraint
                entity.HasIndex(e => new { e.AdminUuid, e.PermissionUuid }).IsUnique();

                // Indexes
                entity.HasIndex(e => e.AdminUuid).HasDatabaseName("idx_admin_permissions_admin");
                entity.HasIndex(e => e.PermissionUuid).HasDatabaseName("idx_admin_permissions_permission");

                // Foreign keys - Configure with navigation properties
                entity.HasOne(ap => ap.Admin).WithMany(a => a.AdminPermissions).HasForeignKey(ap => ap.AdminUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_admin_permissions_admin");
                entity.HasOne(ap => ap.Permission).WithMany(p => p.AdminPermissions).HasForeignKey(ap => ap.PermissionUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_admin_permissions_permission");
            });
        }

        private static void ConfigureStaffPermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffPermission>(entity =>
            {
                entity.ToTable("staff_permissions");
                entity.HasKey(e => e.StaffPermissionUuid);

                entity.Property(e => e.StaffPermissionUuid).HasColumnName("staff_permission_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Unique constraint
                entity.HasIndex(e => new { e.StaffUuid, e.PermissionUuid }).IsUnique();

                // Indexes
                entity.HasIndex(e => e.StaffUuid).HasDatabaseName("idx_staff_permissions_staff");
                entity.HasIndex(e => e.PermissionUuid).HasDatabaseName("idx_staff_permissions_permission");

                // Foreign keys
                entity.HasOne(sp => sp.Staff).WithMany(s => s.StaffPermissions).HasForeignKey(sp => sp.StaffUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_staff_permissions_staff");
                entity.HasOne(sp => sp.Permission).WithMany(p => p.StaffPermissions).HasForeignKey(sp => sp.PermissionUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_staff_permissions_permission");
            });
        }

        private static void ConfigureStudentPermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentPermission>(entity =>
            {
                entity.ToTable("student_permissions");
                entity.HasKey(e => e.StudentPermissionUuid);

                entity.Property(e => e.StudentPermissionUuid).HasColumnName("student_permission_uuid").HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

                // Unique constraint
                entity.HasIndex(e => new { e.StudentUuid, e.PermissionUuid }).IsUnique();

                // Indexes
                entity.HasIndex(e => e.StudentUuid).HasDatabaseName("idx_student_permissions_student");
                entity.HasIndex(e => e.PermissionUuid).HasDatabaseName("idx_student_permissions_permission");

                // Foreign keys
                entity.HasOne(sp => sp.Student).WithMany(s => s.StudentPermissions).HasForeignKey(sp => sp.StudentUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_student_permissions_student");
                entity.HasOne(sp => sp.Permission).WithMany(p => p.StudentPermissions).HasForeignKey(sp => sp.PermissionUuid).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_student_permissions_permission");
            });
        }

        #endregion

        #region Audit Log Tables Configuration

        #region Main Entities Logs And Junction Logs Tables
        private void ConfigureUniversityLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityLog>(entity =>
            {
                entity.ToTable("university_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.UniversityName).HasColumnName("university_name").HasMaxLength(250);
                entity.Property(e => e.UniversityCode).HasColumnName("university_code").HasMaxLength(50);
                entity.Property(e => e.RegionId).HasColumnName("region_id");
                entity.Property(e => e.UniversityTypeId).HasColumnName("university_type_id");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.EstablishedYear).HasColumnName("established_year");
                entity.Property(e => e.WebsiteUrl).HasColumnName("website_url").HasMaxLength(500);
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_university_logs_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_university_logs_logged_at");
                // Foreign key
                entity.HasOne(ul => ul.LogType).WithMany(au => au.UniversityLogs).HasForeignKey(ul => ul.LogTypeId).HasConstraintName("fk_university_logs_type");
            });
        }

        private void ConfigureFacultyLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacultyLog>(entity =>
            {
                entity.ToTable("faculty_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.FacultyUuid).HasColumnName("faculty_uuid");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.FacultyName).HasColumnName("faculty_name").HasMaxLength(200);
                entity.Property(e => e.FacultyCode).HasColumnName("faculty_code").HasMaxLength(50);
                entity.Property(e => e.FacultyDescription).HasColumnName("faculty_description").HasColumnType("text");
                entity.Property(e => e.DeanUuid).HasColumnName("dean_uuid");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.FacultyUuid).HasDatabaseName("idx_faculty_logs_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_faculty_logs_logged_at");
                // Foreign key
                entity.HasOne(fl => fl.LogType).WithMany(ua => ua.FacultyLogs).HasForeignKey(fl => fl.LogTypeId).HasConstraintName("fk_faculty_logs_type");
            });
        }

        private void ConfigureAcademicianLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicianLog>(entity =>
            {
                entity.ToTable("academician_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.AcademicianUuid).HasColumnName("academician_uuid");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.FacultyUuid).HasColumnName("faculty_uuid");
                entity.Property(e => e.DepartmentUuid).HasColumnName("department_uuid");
                entity.Property(e => e.TitleId).HasColumnName("title_id");


                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(5);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.PersonalWebsiteUrl).HasColumnName("personal_website_url").HasMaxLength(500);
                entity.Property(e => e.OfficeAddress).HasColumnName("office_address").HasColumnType("text");
                entity.Property(e => e.Bio).HasColumnName("bio").HasColumnType("text");

                entity.Property(e => e.YokId).HasColumnName("yok_id").HasMaxLength(100);
                entity.Property(e => e.GoogleScholarId).HasColumnName("google_scholar_id").HasMaxLength(100);
                entity.Property(e => e.ScopusId).HasColumnName("scopus_id").HasMaxLength(100);
                entity.Property(e => e.WebOfScienceId).HasColumnName("web_of_science_id").HasMaxLength(100);
                entity.Property(e => e.OrcidId).HasColumnName("orcid_id").HasMaxLength(100);
                entity.Property(e => e.ResearchGateId).HasColumnName("research_gate_id").HasMaxLength(100);
                entity.Property(e => e.LinkedinUsername).HasColumnName("linkedin_username").HasMaxLength(100);
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);

                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.AcademicianUuid).HasDatabaseName("idx_academician_logs_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_academician_logs_logged_at");
                // Foreign key
                entity.HasOne(al => al.LogType).WithMany(ua => ua.AcademicianLogs).HasForeignKey(al => al.LogTypeId).HasConstraintName("fk_academician_logs_type");
            });
        }

        private void ConfigureRectorLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RectorLog>(entity =>
            {
                entity.ToTable("rector_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.RectorUuid).HasColumnName("rector_uuid");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.AcademicianUuid).HasColumnName("academician_uuid");
                entity.Property(e => e.StartDate).HasColumnName("start_date").HasColumnType("date");
                entity.Property(e => e.EndDate).HasColumnName("end_date").HasColumnType("date");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.RectorUuid).HasDatabaseName("idx_rector_logs_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_rector_logs_logged_at");
                // Foreign key
                entity.HasOne(rl => rl.LogType).WithMany(uar => uar.RectorLogs).HasForeignKey(rl => rl.LogTypeId).HasConstraintName("fk_rector_logs_type");
            });
        }

        private void ConfigureAcademicDepartmentLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicDepartmentLog>(entity =>
            {
                entity.ToTable("academic_department_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.DepartmentUuid).HasColumnName("department_uuid");
                entity.Property(e => e.DepartmentTypeId).HasColumnName("department_type_id");
                entity.Property(e => e.DepartmentName).HasColumnName("department_name").HasMaxLength(200);
                entity.Property(e => e.DepartmentCode).HasColumnName("department_code").HasMaxLength(50);
                entity.Property(e => e.DepartmentDescription).HasColumnName("department_description").HasColumnType("text");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.DepartmentUuid).HasDatabaseName("idx_academic_department_logs_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_academic_department_logs_logged_at");
                // Foreign key
                entity.HasOne(adl => adl.LogType).WithMany(ad => ad.AcademicDepartmentLogs).HasForeignKey(adl => adl.LogTypeId).HasConstraintName("fk_academic_department_logs_type");
            });
        }

        private void ConfigureUniversityCommunicationLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityCommunicationLog>(entity =>
            {
                entity.ToTable("university_communication_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.CommunicationUuid).HasColumnName("communication_uuid");
                entity.Property(e => e.TypeId).HasColumnName("type_id");
                entity.Property(e => e.CommunicationValue).HasColumnName("communication_value").HasMaxLength(500);
                entity.Property(e => e.Priority).HasColumnName("priority").HasDefaultValue(0);
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_university_communication_logs_university");
                entity.HasIndex(e => e.CommunicationUuid).HasDatabaseName("idx_university_communication_logs_communication");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_university_communication_logs_logged_at");
                // Foreign key
                entity.HasOne(ucl => ucl.LogType).WithMany(ua => ua.UniversityCommunicationLogs).HasForeignKey(ucl => ucl.LogTypeId).HasConstraintName("fk_university_communication_logs_type");
            });
        }

        private void ConfigureUniversityAddressLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityAddressLog>(entity =>
            {
                entity.ToTable("university_address_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.AddressUuid).HasColumnName("address_uuid");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id");
                entity.Property(e => e.AddressTitle).HasColumnName("address_title").HasMaxLength(100);
                entity.Property(e => e.AddressLine1).HasColumnName("address_line_1").HasColumnType("text").IsRequired();
                entity.Property(e => e.AddressLine2).HasColumnName("address_line_2").HasColumnType("text");
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
                entity.Property(e => e.StateProvince).HasColumnName("state_province").HasMaxLength(100);
                entity.Property(e => e.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
                entity.Property(e => e.Country).HasColumnName("country").HasMaxLength(100);
                entity.Property(e => e.Latitude).HasColumnName("latitude").HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitude).HasColumnName("longitude").HasColumnType("decimal(11,8)");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.Priority).HasColumnName("priority").HasDefaultValue(0);
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.AddressUuid).HasDatabaseName("idx_university_address_logs_address");
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_university_address_logs_university");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_university_address_logs_logged_at");
                // Foreign key
                entity.HasOne(ual => ual.LogType)
                    .WithMany(ua => ua.UniversityAddressLogs)
                    .HasForeignKey(ual => ual.LogTypeId)
                    .HasConstraintName("fk_university_address_logs_type");
            });
        }

        private void ConfigureUniversityFacultyDepartmentLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityFacultyDepartmentLog>(entity =>
            {
                entity.ToTable("university_faculty_department_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.UniversityDepartmentUuid).HasColumnName("university_department_uuid");
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid");
                entity.Property(e => e.FacultyUuid).HasColumnName("faculty_uuid");
                entity.Property(e => e.DepartmentUuid).HasColumnName("department_uuid");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValueSql("CURRENT_USER");
                // Indexes
                entity.HasIndex(e => e.UniversityDepartmentUuid).HasDatabaseName("idx_university_faculty_department_logs_uuid");
                entity.HasIndex(e => e.UniversityUuid).HasDatabaseName("idx_university_faculty_department_logs_university");
                entity.HasIndex(e => e.FacultyUuid).HasDatabaseName("idx_university_faculty_department_logs_faculty");
                entity.HasIndex(e => e.DepartmentUuid).HasDatabaseName("idx_university_faculty_department_logs_department");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_university_faculty_department_logs_logged_at");
                // Foreign key
                entity.HasOne(ufdl => ufdl.LogType)
                    .WithMany(ua => ua.UniversityFacultyDepartmentLogs)
                    .HasForeignKey(ufdl => ufdl.LogTypeId)
                    .HasConstraintName("fk_university_faculty_department_logs_type");
            });
        }
        #endregion

        #region Permission Logs And Junction Logs Tables

        private static void ConfigureAdminLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminLog>(entity =>
            {
                entity.ToTable("admin_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.AdminUuid).HasColumnName("admin_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified");
                entity.Property(e => e.IsPhoneNumberVerified).HasColumnName("is_phone_number_verified");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.IsSystemPermission).HasColumnName("is_system_permission");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.AdminUuid).HasDatabaseName("idx_admin_logs_admin_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_admin_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_admin_logs_log_type");

                // Foreign keys
                entity.HasOne(al => al.LogType).WithMany(lt => lt.AdminLogs).HasForeignKey(al => al.LogTypeId).HasConstraintName("fk_admin_logs_log_type");
            });
        }

        private static void ConfigureStaffLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffLog>(entity =>
            {
                entity.ToTable("staff_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified");
                entity.Property(e => e.IsPhoneNumberVerified).HasColumnName("is_phone_number_verified");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StaffUuid).HasDatabaseName("idx_staff_logs_staff_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_staff_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_staff_logs_log_type");

                // Foreign keys
                entity.HasOne(sl => sl.LogType).WithMany(lt => lt.StaffLogs).HasForeignKey(sl => sl.LogTypeId).HasConstraintName("fk_staff_logs_log_type");
            });
        }

        private static void ConfigureStudentLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentLog>(entity =>
            {
                entity.ToTable("student_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.UniversityUuid).HasColumnName("university_uuid").HasColumnType("uuid");
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code").HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified");
                entity.Property(e => e.IsPhoneNumberVerified).HasColumnName("is_phone_number_verified");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
                entity.Property(e => e.ProfileImageUrl).HasColumnName("profile_image_url").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StudentUuid).HasDatabaseName("idx_student_logs_student_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_student_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_student_logs_log_type");

                // Foreign keys
                entity.HasOne(sl => sl.LogType).WithMany(lt => lt.StudentLogs).HasForeignKey(sl => sl.LogTypeId).HasConstraintName("fk_student_logs_log_type");
            });
        }

        private static void ConfigureRoleLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleLog>(entity =>
            {
                entity.ToTable("role_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.IsEditable).HasColumnName("is_editable");
                entity.Property(e => e.IsSystemRole).HasColumnName("is_system_role");
                entity.Property(e => e.CreatorAdminUuid).HasColumnName("creator_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStaffUuid).HasColumnName("creator_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStudentUuid).HasColumnName("creator_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.RoleUuid).HasDatabaseName("idx_role_logs_role_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_role_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_role_logs_log_type");

                // Foreign keys
                entity.HasOne(rl => rl.LogType).WithMany(lt => lt.RoleLogs).HasForeignKey(rl => rl.LogTypeId).HasConstraintName("fk_role_logs_log_type");
            });
        }

        private static void ConfigurePermissionLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionLog>(entity =>
            {
                entity.ToTable("permission_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.IsEditable).HasColumnName("is_editable");
                entity.Property(e => e.IsSystemPermission).HasColumnName("is_system_permission");
                entity.Property(e => e.CreatorAdminUuid).HasColumnName("creator_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStaffUuid).HasColumnName("creator_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.CreatorStudentUuid).HasColumnName("creator_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.PermissionUuid).HasDatabaseName("idx_permission_logs_permission_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_permission_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_permission_logs_log_type");

                // Foreign keys
                entity.HasOne(pl => pl.LogType).WithMany(lt => lt.PermissionLogs).HasForeignKey(pl => pl.LogTypeId).HasConstraintName("fk_permission_logs_log_type");
            });
        }

        private static void ConfigureSecurityEventLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityEventLog>(entity =>
            {
                entity.ToTable("security_event_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.SecurityEventUuid).HasColumnName("security_event_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.EventTypeUuid).HasColumnName("event_type_uuid").HasColumnType("uuid");
                entity.Property(e => e.EventedByAdminUuid).HasColumnName("evented_by_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.EventedByStaffUuid).HasColumnName("evented_by_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.EventedByStudentUuid).HasColumnName("evented_by_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(1000).IsRequired();
                entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(45).IsRequired();
                entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
                entity.Property(e => e.AdditionalData).HasColumnName("additional_data").HasColumnType("jsonb");
                entity.Property(e => e.EventTime).HasColumnName("event_time").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.SecurityEventUuid).HasDatabaseName("idx_security_event_logs_event_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_security_event_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_security_event_logs_log_type");

                // Foreign keys
                entity.HasOne(sel => sel.LogType).WithMany(lt => lt.SecurityEventLogs).HasForeignKey(sel => sel.LogTypeId).HasConstraintName("fk_security_event_logs_log_type");
            });
        }

        private static void ConfigureStaffSuspensionLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffSuspensionLog>(entity =>
            {
                entity.ToTable("staff_suspension_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StaffSuspensionUuid).HasColumnName("staff_suspension_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.SuspendedByAdminUuid).HasColumnName("suspended_by_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspendedByStaffUuid).HasColumnName("suspended_by_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspensionStartDate).HasColumnName("suspension_start_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.SuspensionEndDate).HasColumnName("suspension_end_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.Reason).HasColumnName("reason").HasMaxLength(1000).IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StaffSuspensionUuid).HasDatabaseName("idx_staff_suspension_logs_suspension_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_staff_suspension_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_staff_suspension_logs_log_type");

                // Foreign keys
                entity.HasOne(ssl => ssl.LogType).WithMany(lt => lt.StaffSuspensionLogs).HasForeignKey(ssl => ssl.LogTypeId).HasConstraintName("fk_staff_suspension_logs_log_type");
            });
        }

        private static void ConfigureStudentSuspensionLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentSuspensionLog>(entity =>
            {
                entity.ToTable("student_suspension_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StudentSuspensionUuid).HasColumnName("student_suspension_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.SuspendedByAdminUuid).HasColumnName("suspended_by_admin_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspendedByStaffUuid).HasColumnName("suspended_by_staff_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspendedByStudentUuid).HasColumnName("suspended_by_student_uuid").HasColumnType("uuid");
                entity.Property(e => e.SuspensionStartDate).HasColumnName("suspension_start_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.SuspensionEndDate).HasColumnName("suspension_end_date").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.Reason).HasColumnName("reason").HasMaxLength(1000).IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StudentSuspensionUuid).HasDatabaseName("idx_student_suspension_logs_suspension_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_student_suspension_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_student_suspension_logs_log_type");

                // Foreign keys
                entity.HasOne(ssl => ssl.LogType).WithMany(lt => lt.StudentSuspensionLogs).HasForeignKey(ssl => ssl.LogTypeId).HasConstraintName("fk_student_suspension_logs_log_type");
            });
        }

        private static void ConfigureSecurityEventTypeLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityEventTypeLog>(entity =>
            {
                entity.ToTable("security_event_type_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.SecurityEventTypeUuid).HasColumnName("security_event_type_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.IsSystemEvent).HasColumnName("is_system_event");
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.SecurityEventTypeUuid).HasDatabaseName("idx_security_event_type_logs_type_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_security_event_type_logs_logged_at");
                entity.HasIndex(e => e.LogTypeId).HasDatabaseName("idx_security_event_type_logs_log_type");

                // Foreign keys
                entity.HasOne(setl => setl.LogType).WithMany(lt => lt.SecurityEventTypeLogs).HasForeignKey(setl => setl.LogTypeId).HasConstraintName("fk_security_event_type_logs_log_type");
            });
        }

        // Junction Table Logs
        private static void ConfigureAdminRoleLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRoleLog>(entity =>
            {
                entity.ToTable("admin_role_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.AdminRoleUuid).HasColumnName("admin_role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.AdminUuid).HasColumnName("admin_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.AdminRoleUuid).HasDatabaseName("idx_admin_role_logs_admin_role_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_admin_role_logs_logged_at");

                // Foreign keys
                entity.HasOne(arl => arl.LogType).WithMany(lt => lt.AdminRoleLogs).HasForeignKey(arl => arl.LogTypeId).HasConstraintName("fk_admin_role_logs_log_type");
            });
        }

        private static void ConfigureStaffRoleLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffRoleLog>(entity =>
            {
                entity.ToTable("staff_role_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StaffRoleUuid).HasColumnName("staff_role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StaffRoleUuid).HasDatabaseName("idx_staff_role_logs_staff_role_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_staff_role_logs_logged_at");

                // Foreign keys
                entity.HasOne(srl => srl.LogType).WithMany(lt => lt.StaffRoleLogs).HasForeignKey(srl => srl.LogTypeId).HasConstraintName("fk_staff_role_logs_log_type");
            });
        }

        private static void ConfigureStudentRoleLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRoleLog>(entity =>
            {
                entity.ToTable("student_role_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StudentRoleUuid).HasColumnName("student_role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.RoleUuid).HasColumnName("role_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StudentRoleUuid).HasDatabaseName("idx_student_role_logs_student_role_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_student_role_logs_logged_at");

                // Foreign keys
                entity.HasOne(srl => srl.LogType).WithMany(lt => lt.StudentRoleLogs).HasForeignKey(srl => srl.LogTypeId).HasConstraintName("fk_student_role_logs_log_type");
            });
        }

        private static void ConfigureAdminPermissionLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminPermissionLog>(entity =>
            {
                entity.ToTable("admin_permission_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.AdminPermissionUuid).HasColumnName("admin_permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.AdminUuid).HasColumnName("admin_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.AdminPermissionUuid).HasDatabaseName("idx_admin_permission_logs_admin_permission_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_admin_permission_logs_logged_at");

                // Foreign keys
                entity.HasOne(apl => apl.LogType).WithMany(lt => lt.AdminPermissionLogs).HasForeignKey(apl => apl.LogTypeId).HasConstraintName("fk_admin_permission_logs_log_type");
            });
        }

        private static void ConfigureStaffPermissionLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffPermissionLog>(entity =>
            {
                entity.ToTable("staff_permission_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StaffPermissionUuid).HasColumnName("staff_permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.StaffUuid).HasColumnName("staff_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StaffPermissionUuid).HasDatabaseName("idx_staff_permission_logs_staff_permission_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_staff_permission_logs_logged_at");

                // Foreign keys
                entity.HasOne(spl => spl.LogType).WithMany(lt => lt.StaffPermissionLogs).HasForeignKey(spl => spl.LogTypeId).HasConstraintName("fk_staff_permission_logs_log_type");
            });
        }

        private static void ConfigureStudentPermissionLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentPermissionLog>(entity =>
            {
                entity.ToTable("student_permission_logs");
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("log_id").ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.LogTypeId).HasColumnName("log_type_id").IsRequired();
                entity.Property(e => e.LoggedAt).HasColumnName("logged_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");
                entity.Property(e => e.LoggedBy).HasColumnName("logged_by").HasMaxLength(100).HasDefaultValue("System");

                // Original entity properties
                entity.Property(e => e.StudentPermissionUuid).HasColumnName("student_permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.StudentUuid).HasColumnName("student_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.PermissionUuid).HasColumnName("permission_uuid").HasColumnType("uuid").IsRequired();
                entity.Property(e => e.OriginalCreatedAt).HasColumnName("original_created_at").HasColumnType("timestamp with time zone");
                entity.Property(e => e.OriginalUpdatedAt).HasColumnName("original_updated_at").HasColumnType("timestamp with time zone");

                // Indexes
                entity.HasIndex(e => e.StudentPermissionUuid).HasDatabaseName("idx_student_permission_logs_student_permission_uuid");
                entity.HasIndex(e => e.LoggedAt).HasDatabaseName("idx_student_permission_logs_logged_at");

                // Foreign keys
                entity.HasOne(spl => spl.LogType).WithMany(lt => lt.StudentPermissionLogs).HasForeignKey(spl => spl.LogTypeId).HasConstraintName("fk_student_permission_logs_log_type");
            });
        }
        #endregion

        #endregion

    }
}

