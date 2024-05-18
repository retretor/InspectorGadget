using Domain.Entities.DbResults;

namespace Domain.Entities.Responses;

public class AllPartsResult
{
    public List<PartResult> Parts { get; set; } = new();
}