using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemArteCompraMap : IEntityTypeConfiguration<PedidoItemArteCompra>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemArteCompra> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_ARTE_COMPRA")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItemArte)
                .HasColumnName("id_pedidoarteitem")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Login)
                .HasColumnName("nm_login")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataCompra)
                .HasColumnName("dt_compra")
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

            builder.Property(c => c.Observacoes)
                .HasColumnName("nm_observacoes")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.NumeroDocumento)
                .HasColumnName("cd_nro_documento")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.HasOne(mg => mg.PedidoItemArte)
               .WithMany(p => p.Compras)
               .HasForeignKey(mg => mg.IdPedidoItemArte);

            builder.HasOne(mg => mg.Usuario)
               .WithMany(p => p.Compras)
               .HasForeignKey(mg => mg.Login);
        }
    }
}
