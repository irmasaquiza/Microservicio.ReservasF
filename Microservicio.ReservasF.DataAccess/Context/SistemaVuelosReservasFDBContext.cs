using Microsoft.EntityFrameworkCore;

using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Context
{
    public class SistemaReservasDbContext
        : DbContext
    {
        public SistemaReservasDbContext(
            DbContextOptions<SistemaReservasDbContext> options)
            : base(options)
        {
        }

        // =========================
        // RESERVAS
        // =========================

        public DbSet<ReservaEntity> Reservas
            => Set<ReservaEntity>();

        public DbSet<ReservaDetalleEntity> ReservaDetalles
            => Set<ReservaDetalleEntity>();

        public DbSet<FacturaEntity> Facturas
            => Set<FacturaEntity>();

        public DbSet<BoletoEntity> Boletos
            => Set<BoletoEntity>();

        public DbSet<EquipajeEntity> Equipajes
            => Set<EquipajeEntity>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SistemaReservasDbContext).Assembly);
        }
    }
}