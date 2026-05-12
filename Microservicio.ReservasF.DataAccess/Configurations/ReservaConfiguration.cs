using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Configurations
{
    public class ReservaConfiguration
        : IEntityTypeConfiguration<ReservaEntity>
    {
        public void Configure(
            EntityTypeBuilder<ReservaEntity> builder)
        {
            builder.ToTable(
                "reservas",
                "ventas");

            builder.HasKey(e => e.IdReserva)
                .HasName("pk_reservas");

            builder.Property(e => e.IdReserva)
                .HasColumnName("id_reserva");

            builder.Property(e => e.GuidReserva)
                .HasColumnName("guid_reserva")
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(e => e.CodigoReserva)
                .HasColumnName("codigo_reserva")
                .HasMaxLength(40)
                .IsRequired();

            // Referencias lógicas
            builder.Property(e => e.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();

            builder.Property(e => e.IdVuelo)
                .HasColumnName("id_vuelo")
                .IsRequired();

            // Fechas
            builder.Property(e => e.FechaReservaUtc)
                .HasColumnName("fecha_reserva_utc")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')");

            builder.Property(e => e.FechaInicio)
                .HasColumnName("fecha_inicio")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(e => e.FechaFin)
                .HasColumnName("fecha_fin")
                .HasColumnType("timestamp")
                .IsRequired();

            // Valores económicos
            builder.Property(e => e.SubtotalReserva)
                .HasColumnName("subtotal_reserva")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.ValorIva)
                .HasColumnName("valor_iva")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.TotalReserva)
                .HasColumnName("total_reserva")
                .HasColumnType("numeric(12,2)")
                .HasDefaultValue(0);

            // Estado
            builder.Property(e => e.OrigenCanalReserva)
                .HasColumnName("origen_canal_reserva")
                .HasMaxLength(50)
                .HasDefaultValue("WEB");

            builder.Property(e => e.EstadoReserva)
                .HasColumnName("estado_reserva")
                .HasColumnType("char(3)")
                .HasDefaultValue("PEN");

            builder.Property(e => e.FechaConfirmacionUtc)
                .HasColumnName("fecha_confirmacion_utc")
                .HasColumnType("timestamp");

            builder.Property(e => e.FechaCancelacionUtc)
                .HasColumnName("fecha_cancelacion_utc")
                .HasColumnType("timestamp");

            builder.Property(e => e.MotivoCancelacion)
                .HasColumnName("motivo_cancelacion")
                .HasMaxLength(250);

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

            // Contacto
            builder.Property(e => e.ContactoEmail)
                .HasColumnName("contacto_email")
                .HasMaxLength(150);

            builder.Property(e => e.ContactoTelefono)
                .HasColumnName("contacto_telefono")
                .HasMaxLength(20);

            builder.Property(e => e.Observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(255);

            builder.Property(e => e.FechaInhabilitacionUtc)
                .HasColumnName("fecha_inhabilitacion_utc")
                .HasColumnType("timestamp");

            builder.Property(e => e.MotivoInhabilitacion)
                .HasColumnName("motivo_inhabilitacion")
                .HasMaxLength(250);

            // Concurrency token PostgreSQL
            builder.Property(e => e.RowVersion)
                .HasColumnName("row_version")
                .IsConcurrencyToken();

            // Índices
            builder.HasIndex(e => e.GuidReserva)
                .IsUnique()
                .HasDatabaseName("uq_reservas_guid");

            builder.HasIndex(e => e.CodigoReserva)
                .IsUnique()
                .HasDatabaseName("uq_reservas_codigo");

            builder.HasIndex(e => e.IdCliente)
                .HasDatabaseName("ix_reservas_cliente");

            builder.HasIndex(e => e.IdVuelo)
                .HasDatabaseName("ix_reservas_vuelo");

            builder.HasIndex(e => e.EstadoReserva)
                .HasDatabaseName("ix_reservas_estado");

            builder.HasIndex(e => e.FechaReservaUtc)
                .HasDatabaseName("ix_reservas_fecha");
        }
    }
}