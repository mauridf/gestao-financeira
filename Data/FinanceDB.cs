using gestao_financeira.Models;
using gestao_financeira.Models.gestao_financeira.Models;
using Microsoft.EntityFrameworkCore;

namespace gestao_financeira.Data
{
    public class FinanceContext : DbContext
    {
        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Saida> Saidas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<UsarReserva> UsarReservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
