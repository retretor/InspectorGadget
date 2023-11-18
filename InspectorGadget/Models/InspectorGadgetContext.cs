using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Models;

public partial class InspectorGadgetContext : DbContext
{
    public InspectorGadgetContext(DbContextOptions<InspectorGadgetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } = null!;

    public virtual DbSet<Client> Clients { get; set; } = null!;

    public virtual DbSet<Device> Devices { get; set; } = null!;

    public virtual DbSet<Employee> Employees { get; set; } = null!;

    public virtual DbSet<PartForRepairPart> PartForRepairParts { get; set; } = null!;

    public virtual DbSet<RepairPart> RepairParts { get; set; } = null!;

    public virtual DbSet<RepairRequest> RepairRequests { get; set; } = null!;

    public virtual DbSet<RepairType> RepairTypes { get; set; } = null!;

    public virtual DbSet<RepairTypeForDevice> RepairTypeForDevices { get; set; } = null!;

    public virtual DbSet<RepairTypesList> RepairTypesLists { get; set; } = null!;

    public virtual DbSet<RequestStatusHistory> RequestStatusHistories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("employee_position", new[] { "ADMIN", "RECEPTIONIST", "MASTER" })
            .HasPostgresEnum("repair_part_condition", new[] { "NEW", "USED" })
            .HasPostgresEnum("request_status", new[] { "NEW", "IN_PROCESSING", "REJECTED", "PAID", "RETURNED", "ACCEPTED", "COMPLETED" });

        modelBuilder.Entity<AllowedRepairTypesForEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("allowed_repair_types_for_employee_pkey");

            entity.ToTable("allowed_repair_types_for_employee");

            entity.HasIndex(e => new { e.RepairTypeForDeviceId, e.EmployeeId }, "allowed_repair_types_for_empl_repair_type_for_device_id_emp_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.RepairTypeForDeviceId).HasColumnName("repair_type_for_device_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.AllowedRepairTypesForEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("allowed_repair_types_for_employee_employee_id_fkey");

            entity.HasOne(d => d.RepairTypeForDevice).WithMany(p => p.AllowedRepairTypesForEmployees)
                .HasForeignKey(d => d.RepairTypeForDeviceId)
                .HasConstraintName("allowed_repair_types_for_employe_repair_type_for_device_id_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pkey");

            entity.ToTable("client");

            entity.HasIndex(e => e.Login, "client_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.SecondName)
                .HasMaxLength(255)
                .HasColumnName("second_name");
            entity.Property(e => e.TelephoneNumber)
                .HasMaxLength(255)
                .HasColumnName("telephone_number");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("device_pkey");

            entity.ToTable("device");

            entity.HasIndex(e => e.Name, "device_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.SecondName)
                .HasMaxLength(255)
                .HasColumnName("second_name");
            entity.Property(e => e.TelephoneNumber)
                .HasMaxLength(255)
                .HasColumnName("telephone_number");
        });

        modelBuilder.Entity<PartForRepairPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("part_for_repair_part_pkey");

            entity.ToTable("part_for_repair_part");

            entity.HasIndex(e => new { e.RepairTypeForDeviceId, e.RepairPartId }, "part_for_repair_part_repair_type_for_device_id_repair_part__key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartCount).HasColumnName("part_count");
            entity.Property(e => e.RepairPartId).HasColumnName("repair_part_id");
            entity.Property(e => e.RepairTypeForDeviceId).HasColumnName("repair_type_for_device_id");

            entity.HasOne(d => d.RepairPart).WithMany(p => p.PartForRepairParts)
                .HasForeignKey(d => d.RepairPartId)
                .HasConstraintName("part_for_repair_part_repair_part_id_fkey");

            entity.HasOne(d => d.RepairTypeForDevice).WithMany(p => p.PartForRepairParts)
                .HasForeignKey(d => d.RepairTypeForDeviceId)
                .HasConstraintName("part_for_repair_part_repair_type_for_device_id_fkey");
        });

        modelBuilder.Entity<RepairPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_part_pkey");

            entity.ToTable("repair_part");

            entity.HasIndex(e => e.Name, "repair_part_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.CurrentCount).HasColumnName("current_count");
            entity.Property(e => e.MinAllowedCount).HasColumnName("min_allowed_count");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Specification)
                .HasMaxLength(255)
                .HasColumnName("specification");
        });

        modelBuilder.Entity<RepairRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_request_pkey");

            entity.ToTable("repair_request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(20)
                .HasColumnName("serial_number");

            entity.HasOne(d => d.Client).WithMany(p => p.RepairRequests)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("repair_request_client_id_fkey");

            entity.HasOne(d => d.Device).WithMany(p => p.RepairRequests)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("repair_request_device_id_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.RepairRequests)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("repair_request_employee_id_fkey");
        });

        modelBuilder.Entity<RepairType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_type_pkey");

            entity.ToTable("repair_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RepairTypeForDevice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_type_for_device_pkey");

            entity.ToTable("repair_type_for_device");

            entity.HasIndex(e => new { e.RepairTypeId, e.DeviceId }, "repair_type_for_device_repair_type_id_device_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.RepairTypeId).HasColumnName("repair_type_id");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.Device).WithMany(p => p.RepairTypeForDevices)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("repair_type_for_device_device_id_fkey");

            entity.HasOne(d => d.RepairType).WithMany(p => p.RepairTypeForDevices)
                .HasForeignKey(d => d.RepairTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("repair_type_for_device_repair_type_id_fkey");
        });

        modelBuilder.Entity<RepairTypesList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_types_list_pkey");

            entity.ToTable("repair_types_list");

            entity.HasIndex(e => new { e.RepairRequestId, e.RepairTypeForDeviceId }, "repair_types_list_repair_request_id_repair_type_for_device__key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RepairRequestId).HasColumnName("repair_request_id");
            entity.Property(e => e.RepairTypeForDeviceId).HasColumnName("repair_type_for_device_id");

            entity.HasOne(d => d.RepairRequest).WithMany(p => p.RepairTypesLists)
                .HasForeignKey(d => d.RepairRequestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("repair_types_list_repair_request_id_fkey");

            entity.HasOne(d => d.RepairTypeForDevice).WithMany(p => p.RepairTypesLists)
                .HasForeignKey(d => d.RepairTypeForDeviceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("repair_types_list_repair_type_for_device_id_fkey");
        });

        modelBuilder.Entity<RequestStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("request_status_history_pkey");

            entity.ToTable("request_status_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.RepairRequestId).HasColumnName("repair_request_id");

            entity.HasOne(d => d.RepairRequest).WithMany(p => p.RequestStatusHistories)
                .HasForeignKey(d => d.RepairRequestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("request_status_history_repair_request_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
