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
            articolo.Property(e => e.Descrizione).HasColumnType("TEXT");
            articolo.Property(e => e.Prezzo).IsRequired().HasColumnType("DECIMAL(10, 2)");
            articolo.Property(e => e.QuantitaDisponibile).IsRequired();
            articolo.Property(e => e.CodiceSKU).IsRequired().HasMaxLength(50).IsUnicode(false);
            articolo.HasIndex(e => e.CodiceSKU).IsUnique();
            articolo.Property(e => e.Categoria).HasMaxLength(100);
            articolo.Property(e => e.DataInserimento)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
            articolo.HasOne(e => e.Fornitore)
                  .WithMany(f => f.Articoli)
                  .HasForeignKey(e => e.Fk_fornitore)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Fornitore>(fornitore =>
        {
            fornitore.ToTable("Fornitori");
            fornitore.HasKey(x => x.Id);
            fornitore.Property(e => e.Id).ValueGeneratedOnAdd();
            fornitore.Property(e => e.Nome).IsRequired().HasMaxLength(255);
            fornitore.Property(e => e.Indirizzo).HasMaxLength(255);
            fornitore.Property(e => e.Telefono).HasMaxLength(15);
            fornitore.Property(e => e.Email).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<TransactionalOutbox>(outbox =>
        {
            outbox.ToTable("TransactionalOutbox");
            outbox.HasKey(x => x.Id);
            outbox.Property(e => e.Id).ValueGeneratedOnAdd();
            outbox.Property(e => e.Message).IsRequired().HasColumnType("TEXT");
            outbox.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            outbox.Property(e => e.Processed).IsRequired().HasDefaultValue(false);
        });
    }
    public DbSet<Articolo> Articoli { get; set; }
    public DbSet<Fornitore> Fornitori { get; set; }
    public DbSet<TransactionalOutbox> Outboxes { get; set; }
}
