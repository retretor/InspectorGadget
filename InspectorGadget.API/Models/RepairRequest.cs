namespace InspectorGadget.Models;

public partial class RepairRequest
{
    public int Id { get; set; }

    public int DeviceId { get; set; }

    public int ClientId { get; set; }

    public int? EmployeeId { get; set; }

    public string SerialNumber { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Device Device { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<RepairTypesList> RepairTypesLists { get; set; } = new List<RepairTypesList>();

    public virtual ICollection<RequestStatusHistory> RequestStatusHistories { get; set; } = new List<RequestStatusHistory>();
}
