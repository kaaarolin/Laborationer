using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb_2_databaser.Models2;

public partial class Författare
{
    [Key]
    public int Id { get; set; }
    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public DateOnly? Födelsedatum { get; set; }

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
