using Domain.Entities.Base.Concrete;
using Domain.Entities.LogEntities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database.Context
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

        #region DbSets - Audit Log Tables
        public DbSet<UniversityLog> UniversityLogs { get; set; }
        public DbSet<FacultyLog> FacultyLogs { get; set; }
        public DbSet<AcademicianLog> AcademicianLogs { get; set; }
        public DbSet<RectorLog> RectorLogs { get; set; }
        public DbSet<AcademicDepartmentLog> AcademicDepartmentLogs { get; set; }
        public DbSet<UniversityCommunicationLog> UniversityCommunicationLogs { get; set; }
        public DbSet<UniversityAddressLog> UniversityAddressLogs { get; set; }
        public DbSet<UniversityFacultyDepartmentLog> UniversityFacultyDepartmentLogs { get; set; }
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

            // Configure audit log tables
            ConfigureUniversityLog(modelBuilder);
            ConfigureFacultyLog(modelBuilder);
            ConfigureAcademicianLog(modelBuilder);
            ConfigureRectorLog(modelBuilder);
            ConfigureAcademicDepartmentLog(modelBuilder);
            ConfigureUniversityCommunicationLog(modelBuilder);
            ConfigureUniversityAddressLog(modelBuilder);
            ConfigureUniversityFacultyDepartmentLog(modelBuilder);
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

        #region Audit Log Tables Configuration

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
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100);
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(320);
                // ... other properties would follow the same pattern

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

    }
}

