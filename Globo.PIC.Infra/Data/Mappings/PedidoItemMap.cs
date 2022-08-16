using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemMap : IEntityTypeConfiguration<PedidoItem>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedido)
                .HasColumnName("id_pedido")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdPedidoItemPai)
                .HasColumnName("id_pedido_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdItem)
                .HasColumnName("id_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Quantidade)
                .HasColumnName("nr_qtd")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0")
                .IsRequired();

            builder.Property(c => c.Numero)
                .HasColumnName("cd_numero")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasDefaultValue("0")
                .HasMaxLength(20);

            builder.Property(c => c.ValorItens)
                .HasColumnName("vl_valor_itens")
                .HasColumnType("DECIMAL(18,2)")
                .HasDefaultValue("0");

            builder.Property(c => c.Valor)
                .HasColumnName("vl_valor")
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(c => c.ValorUnitario)
                .HasColumnName("vl_valor_unitario")
                .HasColumnType("DECIMAL(18,2)")
                .HasDefaultValue("0");

            builder.Property(c => c.DataNecessidade)
                .HasColumnName("dt_necessario")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.LocalEntrega)
                .HasColumnName("nm_local_entrega")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataEntrega)
                .HasColumnName("dt_entrega")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.NomeItem)
                .HasColumnName("nm_item")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Descricao)
                .HasColumnName("nm_descricao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.UnidadeMedida)
                .HasColumnName("nm_unidade_medida")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.Justificativa)
               .HasColumnName("nm_justificativa")
               .HasColumnType("VARCHAR(500)")
               .HasConversion(stringConverter)
               .HasMaxLength(500);

            builder.Property(c => c.JustificativaCancelamento)
               .HasColumnName("nm_justificativa_cancelamento")
               .HasColumnType("VARCHAR(500)")
               .HasConversion(stringConverter)
               .HasMaxLength(500);

            builder.Property(c => c.JustificativaDevolucao)
               .HasColumnName("nm_justificativa_devolucao")
               .HasColumnType("VARCHAR(500)")
               .HasConversion(stringConverter)
               .HasMaxLength(500);

            builder.Property(c => c.CanceladoPorLogin)
                .HasColumnName("nm_login_cancelamento")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataCancelamento)
                .HasColumnName("dt_cancelamento")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DevolvidoPorLogin)
                .HasColumnName("nm_login_devolucao")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataDevolucao)
                .HasColumnName("dt_devolucao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.AprovadoPorLogin)
                .HasColumnName("nm_login_aprovacao")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataAprovacao)
                .HasColumnName("dt_aprovacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.ReprovadoPorLogin)
                .HasColumnName("nm_login_reprovacao")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataReprovacao)
                .HasColumnName("dt_reprovacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Observacao)
                .HasColumnName("nm_observacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);
             
            builder.HasOne(mg => mg.Pedido)
               .WithMany(p => p.Itens)
               .HasForeignKey(mg => mg.IdPedido);

            builder.HasOne(mg => mg.PedidoItemPai)
               .WithMany(p => p.PedidoItensFilhos)
               .HasForeignKey(mg => mg.IdPedidoItemPai);

            builder.HasOne(mg => mg.Item)
               .WithMany(p => p.PedidoItens)
               .HasForeignKey(mg => mg.IdItem);

            builder.HasOne(mg => mg.CanceladoPor)
               .WithMany(p => p.PedidosItemLoginCancelamento)
               .HasForeignKey(mg => mg.CanceladoPorLogin);

            builder.HasOne(mg => mg.DevolvidoPor)
               .WithMany(p => p.PedidosItemLoginDevolucao)
               .HasForeignKey(mg => mg.DevolvidoPorLogin);

            builder.HasOne(mg => mg.AprovadoPor)
               .WithMany(p => p.PedidosItemLoginAprovacao)
               .HasForeignKey(mg => mg.AprovadoPorLogin);

            builder.HasOne(mg => mg.ReprovadoPor)
               .WithMany(p => p.PedidosItemLoginReprovacao)
               .HasForeignKey(mg => mg.ReprovadoPorLogin);

            builder.HasOne(mg => mg.PedidoItemArte)
               .WithOne(p => p.PedidoItem);

            builder.HasOne(mg => mg.PedidoItemVeiculo)
               .WithOne(p => p.PedidoItem);
        }
    }
}
