using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemArteMap : IEntityTypeConfiguration<PedidoItemArte>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemArte> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_ARTE")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItem)
                .HasColumnName("id_pedido_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.QuantidadePendenteCompra)
                .HasColumnName("nr_qtd_pendente_compra")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0");

            builder.Property(c => c.QuantidadePendenteEntrega)
                .HasColumnName("nr_qtd_pendente_entrega")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0");

            builder.Property(c => c.QuantidadeEntregue)
               .HasColumnName("nr_qtd_entregue")
               .HasColumnType("BIGINT(20)")
               .HasDefaultValue("0");

            builder.Property(c => c.QuantidadeComprada)
               .HasColumnName("nr_qtd_comprada")
               .HasColumnType("BIGINT(20)")
               .HasDefaultValue("0");

            builder.Property(c => c.QuantidadeDevolvida)
               .HasColumnName("nr_qtd_devolvida")
               .HasColumnType("BIGINT(20)")
               .HasDefaultValue("0");

            builder.Property(c => c.MarcacaoCena)
                .HasColumnName("st_marcacao_cena")
                .HasColumnType("TINYINT(4)");

            builder.Property(c => c.IdStatus)
                .HasColumnName("id_status")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Referencias)
               .HasColumnName("nm_referencias")
               .HasColumnType("VARCHAR(500)")
               .HasConversion(stringConverter)
               .HasMaxLength(500);

            builder.Property(c => c.SugestaoFornecedor)
               .HasColumnName("nm_sugestao_fornecedor")
               .HasColumnType("VARCHAR(500)")
               .HasConversion(stringConverter)
               .HasMaxLength(500);

            builder.Property(c => c.SolicitacaoDirigida)
                .HasColumnName("st_solicitacao_dirigida")
                .HasColumnType("TINYINT(4)");

            builder.Property(c => c.CompradoPorLogin)
                .HasColumnName("nm_login_comprador")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataVinculoComprador)
                .HasColumnName("dt_vinculo_comprador")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataVisualizacaoComprador)
                .HasColumnName("dt_visualizacao_comprador")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.IdTipo)
                .HasColumnName("id_tipo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DataReenvio)
                .HasColumnName("dt_reenvio")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataEdicaoReenvio)
                .HasColumnName("dt_edicao_reenvio")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.QuantidadeAprovacaoCompra)
             .HasColumnName("nr_qtd_aprovacao_compra")
             .HasColumnType("BIGINT(20)")
             .HasDefaultValue("0");

            builder.Property(c => c.DataEntregaPrevista)
                .HasColumnName("dt_entrega_prevista")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.ObservacaoAprovacaoCompra)
                .HasColumnName("nm_observacao_aprovacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.FlagDevolvidoBase)
                .HasColumnName("st_devolvido_base")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.FlagDevolvidoComprador)
                .HasColumnName("st_devolvido_comprador")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.FlagItemNaoEncontrado)
                .HasColumnName("st_item_nao_encontrado")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.HasOne(mg => mg.Status)
               .WithMany(p => p.PedidoItensArte)
               .HasForeignKey(mg => mg.IdStatus);            

            builder.HasOne(mg => mg.Comprador)
               .WithMany(p => p.PedidosItemArteLoginComprador)
               .HasForeignKey(mg => mg.CompradoPorLogin);

            builder.HasOne(mg => mg.PedidoItem)
               .WithOne(p => p.PedidoItemArte)
               .HasForeignKey<PedidoItemArte>(mg => mg.IdPedidoItem);
        }
    }
}
