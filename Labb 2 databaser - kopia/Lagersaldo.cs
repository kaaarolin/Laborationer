using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_2_databaser
{
    public class Lagersaldo
    {
        [Key]
        public int ButikID { get; set; }
        public int ISBN { get; set; }
        public int Antal { get; set; }
        public Butiker Butikerna { get; set; }
        public Böcker Böckerna { get; set; }
    }
}
