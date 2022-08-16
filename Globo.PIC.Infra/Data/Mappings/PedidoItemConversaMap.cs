using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemConversaMap : IEntityTypeConfiguration<PedidoItemConversa>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemConversa> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_CONVERSA")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItem)
                .HasColumnName("id_pedidoitem")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DescricaoConversa)
                .HasColumnName("ds_conversa")
                .HasConversion(stringConverter)
                .HasColumnType("TEXT");

            builder.Property(c => c.Login)
                .HasColumnName("nm_login")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            //builder.Property(c => c.IdTipo)
            //    .HasColumnName("id_tipo")
            //    .HasColumnType("TINYINT(4)");

            builder.Property(c => c.DataConversa)
                .HasColumnName("dt_conversa")
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            builder.Property(c => c.IdPICPai)
                .HasColumnName("id_pedidoitem_conversa_pai")
                .HasColumnType("BIGINT(20)");

            builder.HasOne(mg => mg.PedidoItem)
               .WithMany(p => p.PedidoItemConversas)
               .HasForeignKey(mg => mg.IdPedidoItem);

            builder.HasOne(mg => mg.Usuario)
               .WithMany(p => p.PedidoItemConversa)
               .HasForeignKey(mg => mg.Login);

            //builder.HasOne(mg => mg.PedidoItemConversaPai)
            //   .WithMany(p => p.PedidoItemConversaPc)
            //   .HasForeignKey(mg => mg.IdPICPai);
        }
    }
}
