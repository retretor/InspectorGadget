using InspectorGadget.Models;

namespace InspectorGadget.DTOs;

// PEOPLE
public record struct DbUserAuthDto(int Id, string FirstName, string SecondName, string TelephoneNumber, string Login,
    string PasswordHash, string Role);

public record struct UpdateClientDto(string FirstName, string SecondName, string TelephoneNumber,
    int DiscountPercentage);

public record struct CreateClientDto(int DiscountPercentage, int DbUserId);

public record struct ClientGetDto(int Id, string FirstName, string SecondName, string TelephoneNumber, string Login,
    int DiscountPercentage, DbUser DbUser, ICollection<RepairRequest> RepairRequests);

public record struct UpdateEmployeeDto(string FirstName, string SecondName, string TelephoneNumber,
    ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees);

public record struct EmployeeGetDto(int Id, string FirstName, string SecondName, string TelephoneNumber, string Login,
    DbUser DbUser, ICollection<RepairRequest> RepairRequests, ICollection<AllowedRepairTypesForEmployee>
        AllowedRepairTypesForEmployees);

public record struct CreateEmployeeDto(int DbUserId);

// TODO: Add DTOs for other foreign entities!!!!!!!!!

public record struct DeviceDto(string Name, string Type, string Brand, string Series, string Manufacturer);

public record struct PartForRepairPartDto(int PartCount, int RepairTypeForDeviceId, int RepairPartId);

public record struct RepairPartDto(string Name, string Specification, int CurrentCount, int MinAllowedCount, int Cost,
    string Condition);

public record struct RepairRequestDto(int DeviceId, int ClientId, int? EmployeeId, string SerialNumber);

public record struct RepairTypeDto(string Name);

public record struct RepairTypeForDeviceDto(int Cost, int Time, int RepairTypeId, int DeviceId);

public record struct RepairTypesListDto(int RepairTypeForDeviceId, int RepairRequestId);

public record struct RequestStatusHistoryDto(DateTime Date, int RepairRequestId);

public record struct AllowedRepairTypesForEmployeeDto(int RepairTypeForDeviceId, int EmployeeId);