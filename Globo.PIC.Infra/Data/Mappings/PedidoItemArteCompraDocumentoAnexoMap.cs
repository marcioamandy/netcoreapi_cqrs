using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemArteCompraDocumentoAnexoMap : IEntityTypeConfiguration<PedidoItemArteCompraDocumentoAnexo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemArteCompraDocumentoAnexo> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO_ANEXO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdDocumento)
                .HasColumnName("id_documentos")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.NomeArquivo)
                .HasColumnName("nm_arquivo")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

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

            builder.Property(c => c.DataCompraAnexo)
                .HasColumnName("dt_arquivo")
                .HasColumnType("TIMESTAMP");


            builder.HasOne(mg => mg.Documentos)
               .WithMany(p => p.Arquivos)
               .HasForeignKey(mg => mg.IdDocumento);
        }
    }
}
