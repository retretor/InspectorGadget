using Domain.Entities.Basic;
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
}