using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    /// <summary>
    /// 
    /// </summary>
    public class PedidoItemVeiculoTrackingMap : IEntityTypeConfiguration<PedidoItemVeiculoTracking>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemVeiculoTracking> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_VEICULO_TRACKING")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItem)
                .HasColumnName("id_pedido_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.TrackingDate)
                .HasColumnName("dt_tracking")
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            builder.Property(c => c.StatusPosition)
                .HasColumnName("nr_ordem")
                .HasColumnType("BIGINT(20)")
                .IsRequired();

            builder.Property(c => c.StatusId)
                .HasColumnName("id_status")
                .HasColumnType("BIGINT(20)")
                .IsRequired();

            builder.Property(c => c.ChangeById)
                .HasColumnName("ds_alterado_por")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(c => c.Ativo)
                .HasColumnName("st_ativo")
                .HasColumnType("TINYINT(4)");

            builder
                .HasOne(u => u.Status)
                .WithMany(u => u.Tracking)
                .HasForeignKey(u => u.StatusId);

            builder
                .HasOne(u => u.PedidoItemVeiculo)
                .WithMany(u => u.Trackings)
                .HasForeignKey(u => u.IdPedidoItem);

            builder
                .HasOne(u => u.ChangedBy)
				.WithMany(u => u.TrackingVeiculo)
				.HasForeignKey(u => u.ChangeById);


        }
    }
}