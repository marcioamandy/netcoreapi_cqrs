
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemArteDevolucaoMap : IEntityTypeConfiguration<PedidoItemArteDevolucao>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemArteDevolucao> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_ARTE_DEVOLUCAO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItemArte)
                .HasColumnName("id_pedidoarteitem")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdPedidoItemArteOriginal)
                .HasColumnName("id_pedidoarteitemOriginal")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.idTipo)
                .HasColumnName("id_tipo")
                .HasColumnType("BIGINT(20)")
                .IsRequired();

            builder.Property(c => c.Comprador)
                .HasColumnName("nm_comprador")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataDevolucao)
                .HasColumnName("dt_devolucao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Quantidade)
                .HasColumnName("nr_qtd")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0");

            builder.Property(c => c.Justificativa)
                .HasColumnName("nm_justificativa")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.HasOne(mg => mg.PedidoItemArte)
               .WithMany(p => p.Devolucoes)
               .HasForeignKey(mg => mg.IdPedidoItemArte);

            builder.HasOne(mg => mg.UserComprador)
               .WithMany(p => p.Devolucao)
               .HasForeignKey(mg => mg.Comprador);
        }
    }
}
