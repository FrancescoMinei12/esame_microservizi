using Inventario.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Repository;

public class InventarioDbContext(DbContextOptions<InventarioDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articolo>().HasKey(x => x.Id);
        modelBuilder.Entity<Articolo>().Property(e => e.Id).ValueGeneratedOnAdd();
    }
    public DbSet<Articolo> Articoli { get; set; }
}
