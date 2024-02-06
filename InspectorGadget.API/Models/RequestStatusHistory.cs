namespace InspectorGadget.Models;

public sealed class RequestStatusHistory
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int RepairRequestId { get; set; }

    public string RequestStatus { get; set; } = null!;

    public RepairRequest RepairRequest { get; set; } = null!;
}