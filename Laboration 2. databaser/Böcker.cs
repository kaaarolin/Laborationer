using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Laboration_2._databaser
{
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

        public virtual Författare? Författare { get; set; }

    }
}



