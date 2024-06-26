﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DbResults;

public class RepairRequestInfoResult
{
    [Column("repair_request_id")] public int RepairRequestId { get; set; }
    [Column("status")] public string Status { get; set; } = null!;
    [Column("description")] public string Description { get; set; } = null!;
    [Column("cost")] public int? Cost { get; set; }
    [Column("start_date")] public DateTime? StartDate { get; set; } = null!;
    [Column("expected_date")] public DateTime? ExpectedDate { get; set; }
}