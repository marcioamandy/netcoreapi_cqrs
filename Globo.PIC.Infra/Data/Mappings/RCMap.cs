using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class RCMap : IEntityTypeConfiguration<RC>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<RC> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_RC")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItem)
                .HasColumnName("id_pedido_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.HeadId)
                .HasColumnName("cd_oracle_head_id")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.Requisition)
                .HasColumnName("cd_oracle_requisition")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.LineId)
                .HasColumnName("cd_oracle_lineid")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.BUId)
                .HasColumnName("cd_oracle_bu")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.CatalogItemKey)
                .HasColumnName("cd_catalog_item_key")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Acordo)
                .HasColumnName("cd_acordo")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.AcordoId)
                .HasColumnName("id_acordo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.AcordoStatus)
                .HasColumnName("st_acordo")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.AcordoLinhaId)
                .HasColumnName("id_linha_acordo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.OrdemCompra)
                .HasColumnName("cd_ordem_compra")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.AcordoLinhaStatus)
                .HasColumnName("st_linha_acordo")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.Status)
                .HasColumnName("st_requisition")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.LinhaStatus)
                .HasColumnName("st_line_requisition")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.Fornecedor)
                .HasColumnName("nm_fornecedor")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.FornecedorId)
                .HasColumnName("id_fornecedor")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Moeda)
               .HasColumnName("ds_moeda")
               .HasColumnType("VARCHAR(10)")
               .HasConversion(stringConverter)
               .HasMaxLength(10);

            builder.Property(c => c.Categoria)
              .HasColumnName("nm_categoria")
              .HasColumnType("VARCHAR(500)")
              .HasConversion(stringConverter)
              .HasMaxLength(500)
              .IsRequired();

            builder.Property(c => c.CategoriaId)
                .HasColumnName("id_categoria")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Valor)
                .HasColumnName("vl_valor")
                .HasColumnType("DECIMAL(18,2)")
                .HasDefaultValue("0");

            builder.Property(c => c.TipoDocumento)
              .HasColumnName("ds_tipo_documento")
              .HasColumnType("VARCHAR(50)")
              .HasConversion(stringConverter)
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(c => c.ImagemUrl)
              .HasColumnName("nm_imagem_url")
              .HasColumnType("VARCHAR(5000)")
              .HasConversion(stringConverter)
              .HasMaxLength(5000);

            builder.Property(c => c.ItemId)
                .HasColumnName("id_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.ItemCodigo)
              .HasColumnName("cd_item")
              .HasColumnType("VARCHAR(20)")
              .HasConversion(stringConverter)
              .HasMaxLength(20);

            builder.Property(c => c.UOM)
              .HasColumnName("nm_uom")
              .HasColumnType("VARCHAR(100)")
              .HasConversion(stringConverter)
              .HasMaxLength(100);

            builder.Property(c => c.UOMCode)
              .HasColumnName("sg_uom")
              .HasColumnType("VARCHAR(20)")
              .HasConversion(stringConverter)
              .HasMaxLength(20);

            builder.Property(c => c.Descricao)
              .HasColumnName("ds_titulo")
              .HasColumnType("TEXT")
              .HasConversion(stringConverter);

            builder.Property(c => c.DescricaoCompleta)
              .HasColumnName("ds_completa")
              .HasColumnType("TEXT")
              .HasConversion(stringConverter);

            builder.HasOne(mg => mg.PedidoItem)
               .WithMany(p => p.RCs)
               .HasForeignKey(mg => mg.IdPedidoItem);
        }
    }
}
