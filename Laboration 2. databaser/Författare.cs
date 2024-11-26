using Laboration_2._databaser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laboration_2._databaser;

public partial class Författare
{
    [Key]
    public int Id { get; set; }
    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public DateOnly? Födelsedatum { get; set; }

    public virtual ICollection<Böcker> Böcker { get; set; } = new List<Böcker>();
}
