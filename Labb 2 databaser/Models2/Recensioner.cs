using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb_2_databaser.Models2;

public partial class Recensioner
{
    [Key]
    public int RecensionId { get; set; }

    public string? Isbn { get; set; }

    public string? Boktitel { get; set; }

    public int? KundId { get; set; }

    public int? Betyg { get; set; }

    public string? RecensionText { get; set; }

    public DateOnly? Recensionsdatum { get; set; }

    public virtual Böcker? IsbnNavigation { get; set; }

    public virtual Kunder? Kund { get; set; }
}
