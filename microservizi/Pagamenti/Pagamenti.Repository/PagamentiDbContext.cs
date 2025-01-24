using Microsoft.EntityFrameworkCore;
using Pagamenti.Repository.Model;

namespace Pagamenti.Repository;

public class PagamentiDbContext(DbContextOptions<PagamentiDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // MetodoPagamento
        modelBuilder.Entity<MetodoPagamento>(entity =>
        {
            entity.ToTable("MetodiPagamento");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).HasMaxLength(50).IsRequired();
        });

        // Pagamenti
        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.ToTable("Pagamenti");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Importo).HasColumnType("DECIMAL(10, 2)").IsRequired();
            entity.Property(e => e.DataPagamento).HasColumnType("DATETIME").IsRequired();
            entity.Property(e => e.Fk_Ordine).IsRequired();
            entity.HasOne(e => e.MetodoPagamento)
                  .WithMany()
                  .HasForeignKey(e => e.Fk_MetodoPagamento)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
    public DbSet<MetodoPagamento> MetodiPagamento { get; set; }
    public DbSet<Pagamento> Pagamenti { get; set; }
}