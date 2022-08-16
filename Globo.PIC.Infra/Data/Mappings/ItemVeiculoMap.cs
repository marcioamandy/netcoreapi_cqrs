using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class ItemVeiculoMap : IEntityTypeConfiguration<ItemVeiculo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ItemVeiculo> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_ITEM_VEICULO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItemVeiculo)
                .HasColumnName("id_pedido_veiculos_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdItem)
                .HasColumnName("id_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Placa)
                .HasColumnName("nm_placa")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.Cidade)
                .HasColumnName("nm_cidade")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataChegada)
                .HasColumnName("dt_data_chegada")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataDevolucao)
                .HasColumnName("dt_data_devolucao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.ObservacaoComprador)
                .HasColumnName("nm_observacao_comprador")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.ObservacaoEP)
                .HasColumnName("nm_observacao_ep")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.IdTipoPagamento)
                .HasColumnName("id_tipo_pagamento")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Modelo)
                .HasColumnName("nm_modelo")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Ano)
                .HasColumnName("nr_ano")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Cor)
                .HasColumnName("nm_cor")
                .HasColumnType("VARCHAR(30)")
                .HasConversion(stringConverter)
                .HasMaxLength(30);

            builder.Property(c => c.Periodo)
                .HasColumnName("nm_periodo")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataAtivoAte)
                .HasColumnName("dt_data_ativo_ate")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataAprovacao)
                .HasColumnName("dt_data_aprovacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataReprovacao)
                .HasColumnName("dt_data_reprovacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.JustificativaReprovacao)
                .HasColumnName("nm_justificativa_reprovacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.HoraFechamento)
                .HasColumnName("nm_hora_fechamento")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.DataConfirmacaoEnvioAprovacao)
                .HasColumnName("dt_data_confirmacao_envio_aprovacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.BloqueioEmprestimos)
                .HasColumnName("st_bloqueio_emprestimos")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.JustificativaBloqueio)
                .HasColumnName("nm_justificativa_bloqueio")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.HasOne(mg => mg.PedidoItemVeiculo)
               .WithMany(p => p.ItensVeiculo)
               .HasForeignKey(mg => mg.IdPedidoItemVeiculo);

            builder.HasOne(mg => mg.Item)
               .WithOne(p => p.ItemVeiculo)
               .HasForeignKey<ItemVeiculo>(mg => mg.IdItem);
        }
    }
}
