namespace InspectorGadget.Models;

public sealed partial class RequestStatusHistory
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int RepairRequestId { get; set; }

    public RepairRequest RepairRequest { get; set; } = null!;
}
