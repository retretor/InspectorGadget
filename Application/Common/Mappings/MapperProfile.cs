using AutoMapper;
using Domain.Entities.Basic;

namespace Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // TODO: Add mappings here
        // CreateMap<Source, Destination>();

        CreateMap<Actions.DbUser.CreateDbUserQuery, DbUser>();
        CreateMap<Actions.DbUser.UpdateDbUserQuery, DbUser>();

        CreateMap<Actions.DbUser.CreateDbUserQuery, DbUser>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<Actions.DbUser.UpdateDbUserQuery, DbUser>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        
        CreateMap<Actions.AllowedRepairTypesForEmployee.CreateAllowedRepairTypesForEmployeeQuery, AllowedRepairTypesForEmployee>();
        CreateMap<Actions.AllowedRepairTypesForEmployee.UpdateAllowedRepairTypesForEmployeeQuery, AllowedRepairTypesForEmployee>();
    }
}