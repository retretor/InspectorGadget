using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DbResults;

public class BoolResult
{
    [Column("result")] public bool Result { get; set; }
}