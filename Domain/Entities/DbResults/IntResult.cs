using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DbResults;

public class IntResult
{
    [Column("result")] public int Result { get; set; }
}