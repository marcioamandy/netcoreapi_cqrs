using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemArteCompraDocumentoMap : IEntityTypeConfiguration<PedidoItemArteCompraDocumento>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemArteCompraDocumento> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_ARTE_COMPRA_DOCUMENTO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdCompra)
                .HasColumnName("id_compra")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Login)
                .HasColumnName("nm_login")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataDocumento)
                .HasColumnName("dt_documento")
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            builder.Property(c => c.Quantidade)
                .HasColumnName("nr_qtd")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0");

            builder.Property(c => c.ValorCompra)
                .HasColumnName("vl_valor_compra")
                .HasColumnType("DECIMAL(18,2)")
                .HasDefaultValue("0")
                .IsRequired();

            builder.Property(c => c.Fornecedor)
                .HasColumnName("nm_fornecedor")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Observacao)
                .HasColumnName("nm_observacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.HasOne(mg => mg.Compra)
               .WithMany(p => p.Documentos)
               .HasForeignKey(mg => mg.IdCompra);

            builder.HasOne(mg => mg.User)
               .WithMany(p => p.Documentos)
               .HasForeignKey(mg => mg.Login);
        }
    }
}
