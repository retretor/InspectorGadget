using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Composite;

public class BoolResult
{
    [Column("result")] public bool Result { get; set; }
}