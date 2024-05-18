namespace Domain.Entities.Responses;

public class PartForDeviceResult
{
    public int PartId { get; set; }
    public string PartName { get; set; } = null!;
    public string PartCondition { get; set; } = null!;
    public string PartSpecification { get; set; } = null!;
    public int PartCurrentCount { get; set; }
    public int PartMinAllowedCount { get; set; }
    public int PartCost { get; set; }
}