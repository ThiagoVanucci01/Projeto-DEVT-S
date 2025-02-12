using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projeto_DEVT_S.Models;

namespace Projeto_DEVT_S.Data
{
    public class ProjetoDevContext : IdentityDbContext
    {
        public ProjetoDevContext(DbContextOptions<ProjetoDevContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<ItemOrcamento> ItensOrcamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Servico>().ToTable("Servicos");
            modelBuilder.Entity<Orcamento>().ToTable("Orcamentos");
            modelBuilder.Entity<ItemOrcamento>().ToTable("ItensOrcamento");
        }
    }
}
