using System.ComponentModel.DataAnnotations;

namespace Labb_2_databaser.Models2;

public partial class Kunder
{
    [Key] // Markera primärnyckeln
    public int KundId { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public string Epost { get; set; } = null!;

    public string? Telefon { get; set; }

    public string? Adress { get; set; }

    public string? Stad { get; set; }

    public string? Land { get; set; }

    public virtual ICollection<Ordrar> Ordrars { get; set; } = new List<Ordrar>();

    public virtual ICollection<Recensioner> Recensioners { get; set; } = new List<Recensioner>();
}

