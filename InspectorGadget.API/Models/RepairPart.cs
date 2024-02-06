﻿namespace InspectorGadget.Models;

public sealed class RepairPart
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Specification { get; set; } = null!;

    public int CurrentCount { get; set; }

    public int MinAllowedCount { get; set; }

    public int Cost { get; set; }

    public string Condition { get; set; } = null!;

    public ICollection<PartForRepairPart> PartForRepairParts { get; set; } = new List<PartForRepairPart>();
}