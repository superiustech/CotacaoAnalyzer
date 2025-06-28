using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infra.Contexts
{
    public class AnalyzerDbContext : DbContext
    {
        public AnalyzerDbContext(DbContextOptions<AnalyzerDbContext> options) : base(options){}
        public DbSet<CWProduto> Produtos { get; set; }
        public DbSet<CWCotacao> Cotacoes { get; set; }
        public DbSet<CWCotacaoItem> CotacaoItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region ENTIDADES
            modelBuilder.Entity<CWProduto>(entity =>
            {
                entity.HasKey(e => e.nCdProduto);
                entity.Property(e => e.sNmProduto).IsRequired().HasMaxLength(200);
                entity.Property(e => e.sCdProduto).IsRequired().HasMaxLength(50);
                entity.Property(e => e.dVlUnitario).HasColumnType("decimal(18,2)");
                entity.ToTable("PRODUTOS");
            });

            modelBuilder.Entity<CWCotacao>(entity =>
            {
                entity.HasKey(e => e.nCdCotacao);
                entity.Property(e => e.sDsCotacao).IsRequired().HasMaxLength(200);
                entity.Property(e => e.tDtCotacao).IsRequired();
                entity.Property(e => e.bFlFreteIncluso).HasColumnType("boolean"); 
                entity.Property(e => e.dVlTotal).HasColumnType("decimal(18,2)");
                entity.HasMany(e => e.lstCotacaoItem).WithOne(e => e.Cotacao).HasForeignKey(e => e.nCdCotacao).OnDelete(DeleteBehavior.Cascade);
                entity.ToTable("COTACAO");
            });

            modelBuilder.Entity<CWCotacaoItem>(entity =>
            {
                entity.HasKey(e => e.nCdCotacaoItem);
                entity.Property(e => e.nSequencial).IsRequired();
                entity.Property(e => e.nPrazoEntrega).IsRequired();
                entity.Property(e => e.dVlProposto).HasColumnType("decimal(18,2)");
                entity.ToTable("COTACAO_ITEM");
            });
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
