using Laboration_2._databaser;
using Microsoft.EntityFrameworkCore;

public class BokhandelContext : DbContext
{
    public DbSet<Butiker> Butiker { get; set; }
    public DbSet<Böcker> Böcker { get; set; }
    public DbSet<Lagersaldo> Lagersaldo { get; set; }
    public DbSet<Författare> Författare { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=localhost;Database=Laboration_1;Integrated Security=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lagersaldo>()
            .HasOne(ls => ls.Böcker)
            .WithMany(b => b.Lagersaldo)
            .HasForeignKey(ls => ls.Isbn)
            .HasPrincipalKey(b => b.ISBN13);

        modelBuilder.Entity<Lagersaldo>()
            .HasOne(ls => ls.Butiker)
            .WithMany(b => b.Lagersaldo)
            .HasForeignKey(ls => ls.ButikId);

        modelBuilder.Entity<Böcker>()
            .HasOne(b => b.Författare)
            .WithMany(f => f.Böcker)
            .HasForeignKey(b => b.FörfattareId);

    }

}

