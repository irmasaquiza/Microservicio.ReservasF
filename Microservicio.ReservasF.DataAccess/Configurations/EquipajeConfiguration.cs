using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Configurations
{
    public class EquipajeConfiguration
        : IEntityTypeConfiguration<EquipajeEntity>
    {
        public void Configure(
            EntityTypeBuilder<EquipajeEntity> builder)
        {
            builder.ToTable(
                "equipaje",
                "ventas");

            builder.HasKey(e => e.IdEquipaje)
                .HasName("pk_equipaje");

            builder.Property(e => e.IdEquipaje)
                .HasColumnName("id_equipaje");

            // Concurrency token PostgreSQL


            // FK interna
            builder.Property(e => e.IdBoleto)
                .HasColumnName("id_boleto")
                .IsRequired();

            // Datos equipaje
            builder.Property(e => e.Tipo)
                .HasColumnName("tipo")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.PesoKg)
                .HasColumnName("peso_kg")
                .HasColumnType("numeric(5,2)")
                .IsRequired();

            builder.Property(e => e.DescripcionEquipaje)
                .HasColumnName("descripcion_equipaje")
                .HasMaxLength(150);

            builder.Property(e => e.PrecioExtra)
                .HasColumnName("precio_extra")
                .HasColumnType("numeric(8,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.DimensionesCm)
                .HasColumnName("dimensiones_cm")
                .HasMaxLength(50);

            builder.Property(e => e.NumeroEtiqueta)
                .HasColumnName("numero_etiqueta")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValueSql(
                    "'EQ-' || LPAD(FLOOR(RANDOM() * 9999999999)::TEXT, 10, '0')");

            builder.Property(e => e.EstadoEquipaje)
                .HasColumnName("estado_equipaje")
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue("REGISTRADO");

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
            builder.HasIndex(e => e.NumeroEtiqueta)
                .IsUnique()
                .HasDatabaseName("uq_equipaje_num_etiqueta");

            builder.HasIndex(e => e.IdBoleto)
                .HasDatabaseName("ix_equipaje_boleto");

            // Relación interna real
            builder.HasOne(e => e.Boleto)
                .WithMany(b => b.Equipajes)
                .HasForeignKey(e => e.IdBoleto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_equipaje_boleto");
        }
    }
}