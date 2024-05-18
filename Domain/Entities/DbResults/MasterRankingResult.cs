using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DbResults;

public class MasterRankingResult
{
    [Column("master_id")] public int MasterId { get; set; }

    [Column("repairs_rank")] public int RepairsRank { get; set; }

    [Column("cost_rank")] public int CostRank { get; set; }

    [Column("repairs_period_rank")] public int RepairsPeriodRank { get; set; }

    [Column("cost_period_rank")] public int CostPeriodRank { get; set; }
}