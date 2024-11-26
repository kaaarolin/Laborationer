using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_2_databaser
{
    public class BokhandelContext: DbContext
    {
        public DbSet<Böcker> Books { get; set; }
        public DbSet<Butiker> Butikerna { get; set; }
        public DbSet<Lagersaldo> Lagersaldon { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder
       optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Database=Laboration_1;Integrated Security=True;TrustServerCertificate=True;");
        }
    }
}
   

