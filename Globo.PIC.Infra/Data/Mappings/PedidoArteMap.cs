using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoArteMap : IEntityTypeConfiguration<PedidoArte>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoArte> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ARTE")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedido)
                .HasColumnName("id_pedido")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DataNecessidade)
                .HasColumnName("dt_necessario")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.LocalUtilizacao)
             .HasColumnName("nm_local_utilizacao")
             .HasColumnType("VARCHAR(200)")
             .HasConversion(stringConverter)
             .HasMaxLength(200);

            builder.Property(c => c.DescricaoCena)
                .HasColumnName("ds_cena")
                .HasConversion(stringConverter)
                .HasColumnType("TEXT");

            builder.Property(c => c.IdStatus)
                .HasColumnName("id_status")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DataSolicitacaoCancelamento )
                .HasColumnName("dt_solicitacao_cancelamento")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataConfirmacaoCompra)
                .HasColumnName("dt_confirmacao_compra")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.BaseLogin)
                .HasColumnName("nm_login_base")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataVinculoBase)
                .HasColumnName("dt_vinculo_base")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataReenvio)
                .HasColumnName("dt_reenvio")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataEdicaoReenvio)
                .HasColumnName("dt_edicao_reenvio")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.FlagPedidoAlimentos)
                .HasColumnName("st_pedido_alimentos")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.FlagFastPass)
                .HasColumnName("st_fastpass")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.HasOne(mg => mg.Pedido)
               .WithOne(p => p.PedidoArte)
               .HasForeignKey<PedidoArte>(mg => mg.IdPedido);

            builder.HasOne(mg => mg.Status)
               .WithMany(p => p.PedidosArte)
               .HasForeignKey(mg => mg.IdStatus);

            builder.HasOne(mg => mg.Base)
               .WithMany(p => p.PedidosLoginArteBase)
               .HasForeignKey(mg => mg.BaseLogin);
        }
    }
}
