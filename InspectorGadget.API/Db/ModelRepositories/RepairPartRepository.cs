using InspectorGadget.DTOs;
using InspectorGadget.Models;
using Microsoft.EntityFrameworkCore;

namespace InspectorGadget.Db.ModelRepositories;

public class RepairPartRepository : BaseRepository<RepairPart>
{
    public override async Task<RepairPart?> CreateAsync(object inDto) 
    {
        var dto = (RepairPartDto)inDto;
        var entities = await Context.RepairParts.ToListAsync();
        if (entities.Any(e => e.Name == dto.Name))
        {
            Console.WriteLine($"RepairPart already exists: {dto.Name}");
            return null;
        }

        var entity = new RepairPart
        {
            Condition = dto.Condition,
            Cost = dto.Cost,
            CurrentCount = dto.CurrentCount,
            MinAllowedCount = dto.MinAllowedCount,
            Name = dto.Name,
            Specification = dto.Specification
        };
        Context.RepairParts.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }
}