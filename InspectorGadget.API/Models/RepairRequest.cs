﻿namespace InspectorGadget.Models;

public sealed class RepairRequest
{
    public int Id { get; set; }

    public int DeviceId { get; set; }

    public int ClientId { get; set; }

    public int? EmployeeId { get; set; }

    public string SerialNumber { get; set; } = null!;

    public Client Client { get; set; } = null!;

    public Device Device { get; set; } = null!;

    public Employee? Employee { get; set; }

    public ICollection<RepairTypesList> RepairTypesLists { get; set; } = new List<RepairTypesList>();

    public ICollection<RequestStatusHistory> RequestStatusHistories { get; set; } = new List<RequestStatusHistory>();
}