using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb_2_databaser.Models2;

public partial class Ordrar
{
    [Key]
    public int OrderId { get; set; }

    public int? KundId { get; set; }

    public DateOnly? OrderDatum { get; set; }

    public decimal? TotaltPris { get; set; }

    public string? Status { get; set; }

    public virtual Kunder? Kund { get; set; }
}
