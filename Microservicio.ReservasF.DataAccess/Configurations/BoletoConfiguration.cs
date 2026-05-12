using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Configurations
{
    public class BoletoConfiguration
        : IEntityTypeConfiguration<BoletoEntity>
    {
        public void Configure(
            EntityTypeBuilder<BoletoEntity> builder)
        {
            builder.ToTable(
                "boleto",
                "ventas");

            builder.HasKey(e => e.IdBoleto)
                .HasName("pk_boleto");

            builder.Property(e => e.IdBoleto)
                .HasColumnName("id_boleto");

            // Concurrency Token PostgreSQL
            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsConcurrencyToken();

            // Relaciones internas
            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva")
                .IsRequired();

            builder.Property(e => e.IdDetalle)
                .HasColumnName("id_detalle")
                .IsRequired();

            builder.Property(e => e.IdFactura)
                .HasColumnName("id_factura")
                .IsRequired();

            // Referencias lógicas
            builder.Property(e => e.IdVuelo)
                .HasColumnName("id_vuelo")
                .IsRequired();

            builder.Property(e => e.IdAsiento)
                .HasColumnName("id_asiento")
                .IsRequired();

            builder.Property(e => e.CodigoBoleto)
                .HasColumnName("codigo_boleto")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Clase)
                .HasColumnName("clase")
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue("ECONOMICA");

            builder.Property(e => e.PrecioVueloBase)
                .HasColumnName("precio_vuelo_base")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.PrecioAsientoExtra)
                .HasColumnName("precio_asiento_extra")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.ImpuestosBoleto)
                .HasColumnName("impuestos_boleto")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.CargoEquipaje)
                .HasColumnName("cargo_equipaje")
                .HasColumnType("numeric(8,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.PrecioFinal)
                .HasColumnName("precio_final")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.EstadoBoleto)
                .HasColumnName("estado_boleto")
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue("ACTIVO");

            builder.Property(e => e.FechaEmision)
                .HasColumnName("fecha_emision")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

            builder.Property(e => e.EsEliminado)
                .HasColumnName("es_eliminado")
                .HasDefaultValue(false);

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVO");

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
            builder.HasIndex(e => e.CodigoBoleto)
                .IsUnique()
                .HasDatabaseName("uq_boleto_codigo");

            builder.HasIndex(e => e.IdReserva)
                .HasDatabaseName("ix_boleto_reserva");

            builder.HasIndex(e => e.IdDetalle)
                .IsUnique()
                .HasDatabaseName("uq_boleto_detalle");

            builder.HasIndex(e => e.IdVuelo)
                .HasDatabaseName("ix_boleto_vuelo");

            builder.HasIndex(e => e.IdFactura)
                .HasDatabaseName("ix_boleto_factura");

            // Relaciones internas reales
            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.Boletos)
                .HasForeignKey(e => e.IdReserva)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_boleto_reserva");

            builder.HasOne(e => e.Detalle)
                .WithOne(d => d.Boleto)
                .HasForeignKey<BoletoEntity>(e => e.IdDetalle)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_boleto_detalle");

            builder.HasOne(e => e.Factura)
                .WithMany(f => f.Boletos)
                .HasForeignKey(e => e.IdFactura)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_boleto_factura");
        }
    }
}