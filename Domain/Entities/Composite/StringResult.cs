using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Composite;

public class StringResult
{
    [Column("result")] public string Result { get; set; } = string.Empty;
}