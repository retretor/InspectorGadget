using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Composite;

public class IntResult
{
    [Column("result")] public int Result { get; set; }
}