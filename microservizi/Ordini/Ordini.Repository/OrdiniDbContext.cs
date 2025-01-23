using Ordini.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Ordini.Repository;

public class OrdiniDbContext(DbContextOptions<OrdiniDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(cliente =>
        {
            cliente.ToTable("Clienti");
            cliente.HasKey(x => x.Id);
            cliente.Property(e => e.Id).ValueGeneratedOnAdd();
            cliente.Property(e => e.Nome).IsRequired().HasMaxLength(255);
            cliente.Property(e => e.Cognome).IsRequired().HasMaxLength(255);
            cliente.Property(e => e.Email).IsRequired().HasMaxLength(100);
            cliente.Property(e => e.Telefono).HasMaxLength(15);
            cliente.Property(e => e.Indirizzo).HasMaxLength(255);
        });
        modelBuilder.Entity<Ordine>(ordine =>
        {
            ordine.ToTable("Ordini");
            ordine.HasKey(x => x.Id);
            ordine.Property(e => e.Id).ValueGeneratedOnAdd();
            ordine.Property(e => e.DataOrdine).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            ordine.Property(e => e.Totale).IsRequired().HasColumnType("DECIMAL(10, 2)");
            ordine.Property(e => e.Fk_cliente).IsRequired();
            ordine.HasOne(o => o.Cliente)
                  .WithMany(c => c.Ordini)
                  .HasForeignKey(o => o.Fk_cliente)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<OrdineProdotti>(ordiniProdotti =>
        {
            ordiniProdotti.ToTable("OrdiniProdotti");
            ordiniProdotti.HasKey(x => x.Id);
            ordiniProdotti.Property(e => e.Id).ValueGeneratedOnAdd();
            ordiniProdotti.Property(e => e.Quantita).IsRequired().HasDefaultValue(1);
            ordiniProdotti.Property(e => e.Fk_ordine).IsRequired();
            ordiniProdotti.Property(e => e.Fk_prodotto).IsRequired();

            ordiniProdotti.HasOne(op => op.Ordine)
                          .WithMany(o => o.OrdiniProdotti)
                          .HasForeignKey(op => op.Fk_ordine)
                          .OnDelete(DeleteBehavior.Cascade);
        });
    }
    public DbSet<Cliente> Clienti { get; set; }
    public DbSet<Ordine> Ordini { get; set; }
    public DbSet<OrdineProdotti> OrdineProdotti { get; set; }
}
