using System.ComponentModel.DataAnnotations;

namespace Labb_2_databaser.Models2;

public class Lagersaldo
{
    [Key]
    public int ButikId { get; set; } 
    public string Isbn { get; set; } = null!;
    public int Antal { get; set; }

    public Butiker Butiker { get; set; } = null!;
    public Böcker Böcker { get; set; } = null!;
}


