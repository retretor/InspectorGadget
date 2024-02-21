using Domain.Entities.Basic;
using Domain.Entities.Composite;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<DbUser> DbUsers { get; set; }
    DbSet<Client> Clients { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; }
    DbSet<RepairType> RepairTypes { get; set; }
    DbSet<Device> Devices { get; set; }
    DbSet<PartForRepairType> PartForRepairTypes { get; set; }
    DbSet<RepairPart> RepairParts { get; set; }
    DbSet<RepairRequest> RepairRequests { get; set; }
    DbSet<RepairTypeForDevice> RepairTypeForDevices { get; set; }
    DbSet<RepairTypesList> RepairTypesLists { get; set; }
    DbSet<RequestStatusHistory> RequestStatusHistories { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);


    // SAVED PROCEDURES
    public IQueryable<GetUserLoginByTelephoneResult> GetUserLoginByTelephone(string inputTelephone, string inputSecretKey);

    public IQueryable<CreateClientResult> CreateClient(string inputFirstName, string inputSecondName, string inputTelephoneNumber,
        int discountPercentage,
        string inputLogin, string inputPasswordHash, string secretKey);

    public IQueryable<CreateEmployeeResult> CreateEmployee(string inputFirstName, string inputSecondName, string inputTelephoneNumber,
        int experienceYears, int yearsInCompany, int rating, string inputLogin, string inputPasswordHash,
        string secretKey, string inputRole);

    public IQueryable<MasterRankingResult> GetMasterRanking(DateTime? inputPeriodStart, DateTime? inputPeriodEnd);

    public IQueryable<PartsInfoResult> GetPartsLessMinCountInfo();

    public IQueryable<PartsInfoResult> GetPartsMoreMinCountInfo();

    public IQueryable<RepairCostResult> CalculateRepairCost(int? inputRequestId);
    public IQueryable<RepairRequestInfoResult> GetRequestInfo(int? inputRequestId);

    public IQueryable<RepairTypeInfoResult> GetRepairTypeInfoResult(int? inputDeviceId);
}