using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_2_databaser
{
    public class Butiker
    {
        [Key]
        public int ButikID { get; set; }
        public string Butiksnamn { get; set; }
        public string Adress { get; set; }
        public string Stad { get; set; }
        public string Postnummer { get; set; }
        public string Land { get; set; }
        public string Telefon { get; set; }


    }
}
