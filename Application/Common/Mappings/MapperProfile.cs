using AutoMapper;
using Domain.Entities.Basic;

namespace Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Actions.DbUser.UpdateDbUserCommand, DbUser>();

        CreateMap<Actions.AllowedRepairTypesForEmployee.CreateAllowedRepairTypesForEmployeeCommand,
            AllowedRepairTypesForEmployee>();
        CreateMap<Actions.AllowedRepairTypesForEmployee.UpdateAllowedRepairTypesForEmployeeCommand,
            AllowedRepairTypesForEmployee>();

        CreateMap<Actions.Client.CreateClientCommand, Client>();
        CreateMap<Actions.Client.UpdateClientCommand, Client>();

        CreateMap<Actions.Employee.CreateEmployeeCommand, Employee>();
        CreateMap<Actions.Employee.UpdateEmployeeCommand, Employee>();

        CreateMap<Actions.Device.CreateDeviceCommand, Device>();
        CreateMap<Actions.Device.UpdateDeviceCommand, Device>();

        CreateMap<Actions.PartForRepairType.CreatePartForRepairTypeCommand, PartForRepairType>();
        CreateMap<Actions.PartForRepairType.UpdatePartForRepairTypeCommand, PartForRepairType>();

        CreateMap<Actions.RepairPart.CreateRepairPartCommand, RepairPart>();
        CreateMap<Actions.RepairPart.UpdateRepairPartCommand, RepairPart>();

        CreateMap<Actions.RepairRequest.CreateRepairRequestCommand, RepairRequest>();
        CreateMap<Actions.RepairRequest.UpdateRepairRequestCommand, RepairRequest>();

        CreateMap<Actions.RepairType.CreateRepairTypeCommand, RepairType>();
        CreateMap<Actions.RepairType.UpdateRepairTypeCommand, RepairType>();

        CreateMap<Actions.RepairTypeForDevice.CreateRepairTypeForDeviceCommand, RepairTypeForDevice>();
        CreateMap<Actions.RepairTypeForDevice.UpdateRepairTypeForDeviceCommand, RepairTypeForDevice>();

        CreateMap<Actions.RepairTypesList.CreateRepairTypesListCommand, RepairTypesList>();
        CreateMap<Actions.RepairTypesList.UpdateRepairTypesListCommand, RepairTypesList>();

        CreateMap<Actions.RequestStatusHistory.CreateRequestStatusHistoryCommand, RequestStatusHistory>();
        CreateMap<Actions.RequestStatusHistory.UpdateRequestStatusHistoryCommand, RequestStatusHistory>();
    }
}