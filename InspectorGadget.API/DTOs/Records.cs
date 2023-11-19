using InspectorGadget.Models;

namespace InspectorGadget.DTOs;

public record struct DbUserAuthDto(int Id, string FirstName, string SecondName, string Login, string PasswordHash,
    UserRole Role);

public record struct Records(int RepairTypeForDeviceId, int EmployeeId);

public record struct ClientDto(string FirstName, string SecondName, string TelephoneNumber, string Login,
    string PasswordHash, int DiscountPercentage);

public record struct DeviceDto(string Name);

public record struct EmployeeDto(string FirstName, string SecondName, string TelephoneNumber);

public record struct PartForRepairPartDto(int PartCount, int RepairTypeForDeviceId, int RepairPartId);

public record struct RepairPartDto(string Name, string Specification, int CurrentCount, int MinAllowedCount, int Cost);

public record struct RepairRequestDto(int DeviceId, int ClientId, int? EmployeeId, string SerialNumber);

public record struct RepairTypeDto(string Name);

public record struct RepairTypeForDeviceDto(int Cost, int Time, int RepairTypeId, int DeviceId);

public record struct RepairTypesListDto(int RepairTypeForDeviceId, int RepairRequestId);

public record struct RequestStatusHistoryDto(DateTime Date, int RepairRequestId);