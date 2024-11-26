using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_2_databaser
{
    public class Böcker
    {
        [Key]
        public string ISBN13 { get; set; }
        public string Titel { get; set; }
        public string Språk { get; set; }
        public decimal Pris { get; set; }
        public DateTime Utgivningsdatum { get; set; }
        public string Genre { get; set; }
        public int AntalSidor { get; set; }
        public string Förlag { get; set; }
        public Författare Författarna { get; set; }

    }
}
