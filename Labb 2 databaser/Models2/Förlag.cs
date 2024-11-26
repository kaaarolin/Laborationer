using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb_2_databaser.Models2;

public partial class Förlag
{
    [Key]
    public int FörlagId { get; set; }

    public string Förlagsnamn { get; set; } = null!;

    public string? Telefon { get; set; }

    public string? Epost { get; set; }

    public string? Adress { get; set; }

    public string? Stad { get; set; }

    public string? Land { get; set; }
}
