using Inventario.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Repository;

public class InventarioDbContext(DbContextOptions<InventarioDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articolo>(articolo =>
        {
            articolo.ToTable("Articoli");
            articolo.HasKey(x => x.Id);
            articolo.Property(e => e.Id).ValueGeneratedOnAdd();
            articolo.Property(e => e.Nome).IsRequired().HasMaxLength(255);
            articolo.Property(e => e.CodiceSKU).IsRequired().HasMaxLength(50);
            articolo.HasOne(e => e.Fornitore)
                  .WithMany(f => f.Articoli)
                  .HasForeignKey(e => e.Fk_fornitore);
        });

        modelBuilder.Entity<Fornitore>(fornitore =>
        {
            fornitore.ToTable("Fornitori");
            fornitore.HasKey(x => x.Id);
            fornitore.Property(e => e.Id).ValueGeneratedOnAdd();
            fornitore.Property(e => e.Nome).IsRequired().HasMaxLength(255);
            fornitore.Property(e => e.Email).IsRequired().HasMaxLength(100);
        });
    }
    public DbSet<Articolo> Articoli { get; set; }
    public DbSet<Fornitore> Fornitore { get; set; }
}
