using Domain.Entities.Basic;
using Domain.Entities.Composite;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    #region Sets

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

    #endregion

    #region Stored Procedures

    // Authentication
    public IQueryable<IntResult> AuthenticateUser(string inputLogin, string inputPasswordHash);

    public IQueryable<StringResult> GetUserLoginByTelephone(string inputTelephone,
        string inputSecretKey);

    public IQueryable<IntResult> ChangePassword(string inputLogin, string inputOldPasswordHash,
        string inputNewPasswordHash);

    // Client
    public IQueryable<IntResult> CreateClient(string inputFirstName,
        string inputSecondName,
        string inputTelephoneNumber,
        int discountPercentage,
        string inputLogin,
        string inputPasswordHash,
        string secretKey);

    // Employee
    public IQueryable<IntResult> CreateEmployee(string inputFirstName,
        string inputSecondName,
        string inputTelephoneNumber,
        int experienceYears,
        int yearsInCompany,
        int rating,
        string inputLogin,
        string inputPasswordHash,
        string secretKey,
        string inputRole);
    public IQueryable<BoolResult> UpdateEmployeeRole(int inputEmployeeId, string inputRole);
    public IQueryable<MasterRankingResult> GetMasterRanking(DateTime? inputPeriodStart, DateTime? inputPeriodEnd);

    // Parts
    public IQueryable<IntResult> GetPartsCount(int inputPartId);
    public IQueryable<PartsInfoResult> GetPartsLessMinCountInfo();
    public IQueryable<PartsInfoResult> GetPartsMoreMinCountInfo();

    // Repair Request
    public IQueryable<BoolResult> ChangeRepairRequestStatus(int inputRepairRequestId, string inputStatus,
        DateTime inputDate);
    public IQueryable<IntResult> CalculateRepairCost(int? inputRequestId);
    public IQueryable<IntResult> CalculateRepairTime(int? inputRequestId);
    public IQueryable<AcceptRequestResult> AcceptRequest(int? inputRequestId);
    public IQueryable<BoolResult> IsAvailableAllParts(int? inputRequestId);
    public IQueryable<RepairRequestInfoResult> GetRequestInfo(int? inputRequestId);

    // Device
    public IQueryable<RepairTypeInfoResult> GetRepairTypesInfo(int? inputDeviceId);

    #endregion
}