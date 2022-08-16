using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemAnexoMap : IEntityTypeConfiguration<PedidoItemAnexo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemAnexo> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_ANEXO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItem)
                .HasColumnName("id_pedido_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.NomeArquivo)
                .HasColumnName("nm_arquivo")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.NomeOriginal)
                .HasColumnName("nm_original")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Tipo)
                .HasColumnName("ds_tipo")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataPedidoItemAnexo)
                .HasColumnName("dt_arquivo")
                .HasColumnType("TIMESTAMP");

            builder.HasOne(mg => mg.PedidoItem)
               .WithMany(p => p.Arquivos)
               .HasForeignKey(mg => mg.IdPedidoItem);
        }
    }
}
