using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_2_databaser
{
    public class Metoder
    {
        public void ListInventoryBookstore()
        {
            using (var context = new BokhandelContext()) 
            {
                var butikers = from s in context.Butikerna
                             select s; 

                foreach (var butiker in butikers)
                {
                    Console.WriteLine($"{butiker.Butiksnamn} - {butiker.Adress}");
                }
            }

        }



    }
}
