using System.ComponentModel.DataAnnotations;

namespace Labb_2_databaser.Models2;

public class Butiker
{
    [Key]
    public int ButikId { get; set; } 
    public string Butiksnamn { get; set; } = null!;
    public string Adress { get; set; } = null!;
    public string Stad { get; set; } = null!;
    public string? Postnummer { get; set; }
    public string Land { get; set; } = null!;
    public string? Telefon { get; set; }

    public ICollection<Lagersaldo> Lagersaldo { get; set; } = new List<Lagersaldo>();
}


