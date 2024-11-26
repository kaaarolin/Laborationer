using System.ComponentModel.DataAnnotations; // För [Key]

namespace Labb_2_databaser.Models2;

public partial class Böcker
{
    [Key] // Markera Isbn13 som primärnyckel
    public string ISBN13 { get; set; } = null!;

    public string Titel { get; set; } = null!;

    public string Språk { get; set; } = null!;

    public decimal? Pris { get; set; }

    public DateOnly? Utgivningsdatum { get; set; }

    public int? FörfattareId { get; set; }

    public string? Genre { get; set; }

    public int? AntalSidor { get; set; }

    public string? Förlag { get; set; }

    public virtual ICollection<Lagersaldo> Lagersaldo { get; set; } = new List<Lagersaldo>();

    public virtual ICollection<Recensioner> Recensioners { get; set; } = new List<Recensioner>();
}

