using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2._databaser
{
    public class Lagersaldo
    {
        [Key]
        public int ButikId { get; set; }
        public string Isbn { get; set; } = null!;
        public int Antal { get; set; }

        public Butiker Butiker { get; set; } = null!;
        public Böcker Böcker { get; set; } = null!;
    }
}
