using Labb_2_databaser.Models2;
using Microsoft.EntityFrameworkCore;

public class BokhandelContext : DbContext
{
    public DbSet<Butiker> Butikerna { get; set; }
    public DbSet<Lagersaldo> Lagersaldo { get; set; }

    public BokhandelContext(DbContextOptions<BokhandelContext> options)
        : base(options)
    {
    }

    // Parameterlös konstruktor för migrationsverktyget
    public BokhandelContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Database=Laboration_1;Integrated Security=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Lagersaldo>()
            .HasOne(ls => ls.Böcker)
            .WithMany(b => b.Lagersaldo)
            .HasForeignKey(ls => ls.Isbn)
            .HasPrincipalKey(b => b.ISBN13);

        modelBuilder.Entity<Lagersaldo>()
            .HasOne(ls => ls.Butiker)
            .WithMany(b => b.Lagersaldo)
            .HasForeignKey(ls => ls.ButikId);

        modelBuilder.Entity<Butiker>()
            .ToTable("Butiker")
            .HasKey(b => b.ButikId);
    }
}






