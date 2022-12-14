using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemArteEntregaMap : IEntityTypeConfiguration<PedidoItemArteEntrega>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemArteEntrega> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_ARTE_ENTREGA")
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

            builder.Property(c => c.DataEntrega)
                .HasColumnName("dt_entrega")
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            builder.Property(c => c.Quantidade)
                .HasColumnName("nr_qtd")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0");

            builder.Property(c => c.LocalEntrega)
                .HasColumnName("nm_local_entrega")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Recebedor)
                .HasColumnName("nm_recebedor")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Observacao)
                .HasColumnName("nm_observacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500); 

            builder.HasOne(mg => mg.PedidoItemArte)
               .WithMany(p => p.Entregas)
               .HasForeignKey(mg => mg.IdPedidoItemArte);

            builder.HasOne(mg => mg.Usuario)
               .WithMany(p => p.Entregas)
               .HasForeignKey(mg => mg.Login);
        }
    }
}
