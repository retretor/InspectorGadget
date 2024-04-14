using System.Text;
using Application.Common.Interfaces;
using Domain.Entities.Basic;
using Domain.Entities.Composite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class InspectorGadgetDbContext : DbContext, IApplicationDbContext
{
    public InspectorGadgetDbContext()
    {
    }

    public InspectorGadgetDbContext(DbContextOptions<InspectorGadgetDbContext> options)
        : base(options)
    {
    }

    public InspectorGadgetDbContext(string connectionString) : base(BuildDbContextOptions(connectionString))
    {
    }

    private static DbContextOptions<InspectorGadgetDbContext> BuildDbContextOptions(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InspectorGadgetDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return optionsBuilder.Options;
    }

    #region Sets

    public virtual DbSet<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } = null!;
    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<DbUser> DbUsers { get; set; } = null!;
    public virtual DbSet<Device> Devices { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;
    public virtual DbSet<PartForRepairType> PartForRepairTypes { get; set; } = null!;
    public virtual DbSet<RepairPart> RepairParts { get; set; } = null!;
    public virtual DbSet<RepairRequest> RepairRequests { get; set; } = null!;
    public virtual DbSet<RepairType> RepairTypes { get; set; } = null!;
    public virtual DbSet<RepairTypeForDevice> RepairTypeForDevices { get; set; } = null!;
    public virtual DbSet<RepairTypesList> RepairTypesLists { get; set; } = null!;
    public virtual DbSet<RequestStatusHistory> RequestStatusHistories { get; set; } = null!;

    #endregion

    #region Stored procedures

    public virtual IQueryable<IntResult> AuthenticateUser(string inputLogin,
        string inputPasswordHash) =>
        FromExpression(() => AuthenticateUser(inputLogin, inputPasswordHash));

    public virtual IQueryable<StringResult> GetUserLoginByTelephone(string inputTelephone,
        string inputSecretKey) =>
        FromExpression(() => GetUserLoginByTelephone(inputTelephone, inputSecretKey));

    public virtual IQueryable<IntResult> ChangePassword(string inputLogin, string inputOldPasswordHash,
        string inputNewPasswordHash) =>
        FromExpression(() => ChangePassword(inputLogin, inputOldPasswordHash, inputNewPasswordHash));

    public virtual IQueryable<IntResult> CreateClient(string inputFirstName, string inputSecondName,
        string inputTelephoneNumber, int discountPercentage, string inputLogin, string inputPasswordHash,
        string secretKey) =>
        FromExpression(() => CreateClient(inputFirstName, inputSecondName, inputTelephoneNumber, discountPercentage,
            inputLogin, inputPasswordHash, secretKey));

    public virtual IQueryable<IntResult> CreateEmployee(string inputFirstName, string inputSecondName,
        string inputTelephoneNumber, int experienceYears, int yearsInCompany, int rating, string inputLogin,
        string inputPasswordHash, string secretKey, string inputRole) =>
        FromExpression(() => CreateEmployee(inputFirstName, inputSecondName, inputTelephoneNumber, experienceYears,
            yearsInCompany, rating, inputLogin, inputPasswordHash, secretKey, inputRole));

    public IQueryable<BoolResult> UpdateEmployeeRole(int inputEmployeeId, string inputRole) =>
        FromExpression(() => UpdateEmployeeRole(inputEmployeeId, inputRole));

    public IQueryable<IntResult> GetPartsCount(int inputPartId) =>
        FromExpression(() => GetPartsCount(inputPartId));

    public IQueryable<BoolResult> ChangeRepairRequestStatus(int inputRepairRequestId, string inputStatus,
        DateTime inputDate)
        => FromExpression(() => ChangeRepairRequestStatus(inputRepairRequestId, inputStatus, inputDate));

    public virtual IQueryable<MasterRankingResult> GetMasterRanking(DateTime? inputPeriodStart,
        DateTime? inputPeriodEnd) =>
        FromExpression(() => GetMasterRanking(inputPeriodStart, inputPeriodEnd));

    public virtual IQueryable<PartsInfoResult> GetPartsLessMinCountInfo() =>
        FromExpression(() => GetPartsLessMinCountInfo());

    public virtual IQueryable<PartsInfoResult> GetPartsMoreMinCountInfo() =>
        FromExpression(() => GetPartsMoreMinCountInfo());

    public virtual IQueryable<IntResult> CalculateRepairCost(int? inputRequestId) =>
        FromExpression(() => CalculateRepairCost(inputRequestId));

    public IQueryable<IntResult> CalculateRepairTime(int? inputRequestId) =>
        FromExpression(() => CalculateRepairTime(inputRequestId));

    public virtual IQueryable<RepairRequestInfoResult> GetRequestInfo(int? inputRequestId) =>
        FromExpression(() => GetRequestInfo(inputRequestId));

    public virtual IQueryable<RepairTypeInfoResult> GetRepairTypesInfo(int? inputDeviceId) =>
        FromExpression(() => GetRepairTypesInfo(inputDeviceId));

    public IQueryable<AcceptRequestResult> AcceptRequest(int? inputRequestId) =>
        FromExpression(() => AcceptRequest(inputRequestId));

    public IQueryable<BoolResult> IsAvailableAllParts(int? inputRequestId) =>
        FromExpression(() => IsAvailableAllParts(inputRequestId));

    #endregion

    private static void RegisterFunctions(ModelBuilder modelBuilder)
    {
        // STORED PROCEDURES
        RegisterDbFunction(modelBuilder, nameof(AuthenticateUser), "authenticate_user", typeof(IntResult));
        RegisterDbFunction(modelBuilder, nameof(GetUserLoginByTelephone), "get_user_login_by_telephone",
            typeof(StringResult));
        RegisterDbFunction(modelBuilder, nameof(ChangePassword), "change_password", typeof(IntResult));
        RegisterDbFunction(modelBuilder, nameof(CreateClient), "create_client", typeof(IntResult));
        RegisterDbFunction(modelBuilder, nameof(CreateEmployee), "create_employee", typeof(IntResult));
        RegisterDbFunction(modelBuilder, nameof(GetMasterRanking), "get_master_ranking",
            typeof(MasterRankingResult));
        RegisterDbFunction(modelBuilder, nameof(GetPartsLessMinCountInfo), "get_parts_less_min_count_info",
            typeof(PartsInfoResult));
        RegisterDbFunction(modelBuilder, nameof(GetPartsMoreMinCountInfo), "get_parts_more_min_count_info",
            typeof(PartsInfoResult));
        RegisterDbFunction(modelBuilder, nameof(CalculateRepairCost), "calculate_repair_cost",
            typeof(IntResult));
        RegisterDbFunction(modelBuilder, nameof(GetRequestInfo), "get_request_info",
            typeof(RepairRequestInfoResult));
        RegisterDbFunction(modelBuilder, nameof(GetRepairTypesInfo), "get_repair_type_info_result",
            typeof(RepairTypeInfoResult));
        RegisterDbFunction(modelBuilder, nameof(AcceptRequest), "accept_request", typeof(AcceptRequestResult));
        RegisterDbFunction(modelBuilder, nameof(IsAvailableAllParts), "is_available_all_parts", typeof(BoolResult));
        RegisterDbFunction(modelBuilder, nameof(UpdateEmployeeRole), "update_employee_role", typeof(BoolResult));
        RegisterDbFunction(modelBuilder, nameof(GetPartsCount), "get_parts_count", typeof(IntResult));
        RegisterDbFunction(modelBuilder, nameof(ChangeRepairRequestStatus), "change_repair_request_status",
            typeof(BoolResult));
        RegisterDbFunction(modelBuilder, nameof(CalculateRepairTime), "calculate_repair_time", typeof(IntResult));
    }
    private static void RegisterDbFunction(ModelBuilder modelBuilder, string methodName, string functionName,
        Type? returnType)
    {
        modelBuilder.HasDbFunction(typeof(InspectorGadgetDbContext).GetMethod(methodName)!).HasName(functionName);
        if (returnType != null) modelBuilder.Entity(returnType).HasNoKey();
    }

    private static string ConvertToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var builder = new StringBuilder();
        builder.Append(char.ToLowerInvariant(input[0]));

        for (var i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                builder.Append('_');
                builder.Append(char.ToLowerInvariant(input[i]));
            }
            else
            {
                builder.Append(input[i]);
            }
        }

        return builder.ToString();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Register stored functions
        RegisterFunctions(modelBuilder);

        // Stored entity configurations
        modelBuilder.Entity<AllowedRepairTypesForEmployee>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("allowed_repair_types_for_employee_pkey");

            entity.ToTable("allowed_repair_types_for_employee");

            entity.HasIndex(e => new { e.RepairTypeForDeviceId, e.EmployeeId },
                "allowed_repair_types_for_empl_repair_type_for_device_id_emp_key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
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
            entity.HasKey(e => e.EntityId).HasName("client_pkey");

            entity.ToTable("client");

            entity.HasIndex(e => e.DbUserId, "client_db_user_id_key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.DbUserId).HasColumnName("db_user_id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.SecondName)
                .HasMaxLength(255)
                .HasColumnName("second_name");
            entity.Property(e => e.TelephoneNumber)
                .HasMaxLength(255)
                .HasColumnName("telephone_number");

            entity.HasOne(d => d.DbUser).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.DbUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("client_db_user_id_fkey");
        });

        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("db_user_pkey");

            entity.ToTable("db_user");

            entity.HasIndex(e => e.Login, "db_user_login_key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .HasColumnName("role");
            entity.Property(e => e.SecretKey)
                .HasMaxLength(255)
                .HasColumnName("secret_key");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("device_pkey");

            entity.ToTable("device");

            entity.HasIndex(e => e.Name, "device_name_key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(255)
                .HasColumnName("brand");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(255)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Series)
                .HasMaxLength(255)
                .HasColumnName("series");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.DbUserId, "employee_db_user_id_key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.DbUserId).HasColumnName("db_user_id");
            entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.SecondName)
                .HasMaxLength(255)
                .HasColumnName("second_name");
            entity.Property(e => e.TelephoneNumber)
                .HasMaxLength(255)
                .HasColumnName("telephone_number");
            entity.Property(e => e.YearsInCompany).HasColumnName("years_in_company");

            entity.HasOne(d => d.DbUser).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.DbUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("employee_db_user_id_fkey");
        });

        modelBuilder.Entity<PartForRepairType>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("part_for_repair_type_pkey");

            entity.ToTable("part_for_repair_type");

            entity.HasIndex(e => new { e.RepairTypeForDeviceId, e.RepairPartId },
                "part_for_repair_type_repair_type_for_device_id_repair_part__key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.PartCount).HasColumnName("part_count");
            entity.Property(e => e.RepairPartId).HasColumnName("repair_part_id");
            entity.Property(e => e.RepairTypeForDeviceId).HasColumnName("repair_type_for_device_id");

            entity.HasOne(d => d.RepairPart).WithMany(p => p.PartForRepairTypes)
                .HasForeignKey(d => d.RepairPartId)
                .HasConstraintName("part_for_repair_type_repair_part_id_fkey");

            entity.HasOne(d => d.RepairTypeForDevice).WithMany(p => p.PartForRepairTypes)
                .HasForeignKey(d => d.RepairTypeForDeviceId)
                .HasConstraintName("part_for_repair_type_repair_type_for_device_id_fkey");
        });

        modelBuilder.Entity<RepairPart>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("repair_part_pkey");

            entity.ToTable("repair_part");

            entity.HasIndex(e => e.Name, "repair_part_name_key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.Condition)
                .HasMaxLength(255)
                .HasColumnName("condition");
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
            entity.HasKey(e => e.EntityId).HasName("repair_request_pkey");

            entity.ToTable("repair_request");

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Description).HasColumnName("description");
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
            entity.HasKey(e => e.EntityId).HasName("repair_type_pkey");

            entity.ToTable("repair_type");

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RepairTypeForDevice>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("repair_type_for_device_pkey");

            entity.ToTable("repair_type_for_device");

            entity.HasIndex(e => new { e.RepairTypeId, e.DeviceId },
                "repair_type_for_device_repair_type_id_device_id_key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.DaysToComplete).HasColumnName("days_to_complete");
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.RepairTypeId).HasColumnName("repair_type_id");

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
            entity.HasKey(e => e.EntityId).HasName("repair_types_list_pkey");

            entity.ToTable("repair_types_list");

            entity.HasIndex(e => new { e.RepairRequestId, e.RepairTypeForDeviceId },
                "repair_types_list_repair_request_id_repair_type_for_device__key").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("id");
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
            entity.HasKey(e => e.EntityId).HasName("request_status_history_pkey");

            entity.ToTable("request_status_history");

            entity.Property(e => e.EntityId).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("date");
            entity.Property(e => e.RepairRequestId).HasColumnName("repair_request_id");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");

            entity.HasOne(d => d.RepairRequest).WithMany(p => p.RequestStatusHistories)
                .HasForeignKey(d => d.RepairRequestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("request_status_history_repair_request_id_fkey");
        });

        base.OnModelCreating(modelBuilder);
    }
}