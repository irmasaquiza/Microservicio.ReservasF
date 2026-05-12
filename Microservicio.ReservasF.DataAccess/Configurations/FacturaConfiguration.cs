using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Configurations
{
    public class FacturaConfiguration
        : IEntityTypeConfiguration<FacturaEntity>
    {
        public void Configure(
            EntityTypeBuilder<FacturaEntity> builder)
        {
            builder.ToTable(
                "facturas",
                "ventas");

            builder.HasKey(e => e.IdFactura)
                .HasName("pk_facturas");

            builder.Property(e => e.IdFactura)
                .HasColumnName("id_factura");

            builder.Property(e => e.GuidFactura)
                .HasColumnName("guid_factura")
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");

            // Referencias
            builder.Property(e => e.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();

            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva")
                .IsRequired();

            // Datos factura
            builder.Property(e => e.NumeroFactura)
                .HasColumnName("numero_factura")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(e => e.FechaEmision)
                .HasColumnName("fecha_emision")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

            // Valores económicos
            builder.Property(e => e.Subtotal)
                .HasColumnName("subtotal")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.ValorIva)
                .HasColumnName("valor_iva")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.CargoServicio)
                .HasColumnName("cargo_servicio")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.Total)
                .HasColumnName("total")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            // Información adicional
            builder.Property(e => e.ObservacionesFactura)
                .HasColumnName("observaciones_factura")
                .HasMaxLength(300);

            builder.Property(e => e.OrigenCanalFactura)
                .HasColumnName("origen_canal_factura")
                .HasMaxLength(50);

            // Estado
            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasColumnType("char(3)")
                .HasDefaultValue("ABI");

            builder.Property(e => e.FechaInhabilitacionUtc)
                .HasColumnName("fecha_inhabilitacion_utc")
                .HasColumnType("timestamp");

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

            // Integración
            builder.Property(e => e.ServicioOrigen)
                .HasColumnName("servicio_origen")
                .HasMaxLength(50)
                .HasDefaultValue("VUELOS");

            builder.Property(e => e.MotivoInhabilitacion)
                .HasColumnName("motivo_inhabilitacion")
                .HasMaxLength(250);

            // Concurrency token PostgreSQL
            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsConcurrencyToken();

            // Índices
            builder.HasIndex(e => e.GuidFactura)
                .IsUnique()
                .HasDatabaseName("uq_facturas_guid");

            builder.HasIndex(e => e.NumeroFactura)
                .IsUnique()
                .HasDatabaseName("uq_facturas_numero");

            builder.HasIndex(e => e.IdCliente)
                .HasDatabaseName("ix_facturas_cliente");

            builder.HasIndex(e => e.IdReserva)
                .IsUnique()
                .HasDatabaseName("uq_facturas_reserva");

            builder.HasIndex(e => e.Estado)
                .HasDatabaseName("ix_facturas_estado");

            builder.HasIndex(e => e.FechaEmision)
                .HasDatabaseName("ix_facturas_fecha");

            // Relación interna real
            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.Facturas)
                .HasForeignKey(e => e.IdReserva)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_facturas_reserva");
        }
    }
}