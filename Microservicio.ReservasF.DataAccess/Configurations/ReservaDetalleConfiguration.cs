using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Configurations
{
    public class ReservaDetalleConfiguration
        : IEntityTypeConfiguration<ReservaDetalleEntity>
    {
        public void Configure(
            EntityTypeBuilder<ReservaDetalleEntity> builder)
        {
            builder.ToTable(
                "reserva_detalle",
                "ventas");

            builder.HasKey(e => e.IdDetalle)
                .HasName("pk_reserva_detalle");

            builder.Property(e => e.IdDetalle)
                .HasColumnName("id_detalle");

            // Concurrency token PostgreSQL
            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsConcurrencyToken();

            // FK interna
            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva")
                .IsRequired();

            // Referencias lógicas
            builder.Property(e => e.IdPasajero)
                .HasColumnName("id_pasajero")
                .IsRequired();

            builder.Property(e => e.IdAsiento)
                .HasColumnName("id_asiento")
                .IsRequired();

            // Valores económicos
            builder.Property(e => e.SubtotalLinea)
                .HasColumnName("subtotal_linea")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.ValorIvaLinea)
                .HasColumnName("valor_iva_linea")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.TotalLinea)
                .HasColumnName("total_linea")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            // Estado
            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVO");

            builder.Property(e => e.EsEliminado)
                .HasColumnName("es_eliminado")
                .HasDefaultValue(false);

            // Auditoría
            builder.Property(e => e.CreadoPorUsuario)
                .HasColumnName("creado_por_usuario")
                .HasMaxLength(100)
                .HasDefaultValue("SYSTEM");

            builder.Property(e => e.FechaRegistroUtc)
                .HasColumnName("fecha_registro_utc")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

            builder.Property(e => e.ModificadoPorUsuario)
                .HasColumnName("modificado_por_usuario")
                .HasMaxLength(100);

            builder.Property(e => e.FechaModificacionUtc)
                .HasColumnName("fecha_modificacion_utc")
                .HasColumnType("timestamp");

            builder.Property(e => e.ModificacionIp)
                .HasColumnName("modificacion_ip")
                .HasMaxLength(45);

            // Índices
            builder.HasIndex(e => e.IdReserva)
                .HasDatabaseName("ix_rd_reserva");

            builder.HasIndex(e => e.IdPasajero)
                .HasDatabaseName("ix_rd_pasajero");

            builder.HasIndex(e => e.IdAsiento)
                .HasDatabaseName("ix_rd_asiento");

            builder.HasIndex(e => new
            {
                e.IdReserva,
                e.IdPasajero
            })
            .IsUnique()
            .HasDatabaseName("uq_rd_pasajero_reserva");

            builder.HasIndex(e => new
            {
                e.IdReserva,
                e.IdAsiento
            })
            .IsUnique()
            .HasDatabaseName("uq_rd_asiento_reserva");

            // Relación interna real
            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.Detalles)
                .HasForeignKey(e => e.IdReserva)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_rd_reserva");
        }
    }
}