using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class AcionamentoItemMap : IEntityTypeConfiguration<AcionamentoItem>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<AcionamentoItem> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_ACIONAMENTO_ITEM")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdAcionamento)
                .HasColumnName("id_acionamento")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdPedidoItem)
                .HasColumnName("id_pedido_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.CenaAcao)
                .HasColumnName("st_cena_acao")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.SobreCenaAcao)
                .HasColumnName("nm_sobre_cena_acao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.LocalGravacao)
                .HasColumnName("nm_local_gravacao")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.Passageiros)
                .HasColumnName("nr_passageiros")
                .HasColumnType("INT");

            builder.Property(c => c.HorasVoo)
               .HasColumnName("nm_horas_voo")
               .HasColumnType("VARCHAR(100)")
               .HasConversion(stringConverter)
               .HasMaxLength(100);

            builder.Property(c => c.HorasParado)
               .HasColumnName("nm_horas_parado")
               .HasColumnType("VARCHAR(100)")
               .HasConversion(stringConverter)
               .HasMaxLength(100);

            builder.Property(c => c.Taxar)
                .HasColumnName("st_taxat")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.LocalEmbarque)
                .HasColumnName("nm_local_embarque")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.PeriodoUtilizacaoCena)
                .HasColumnName("nm_periodo_utilizacao")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataConfirmacaoCenaAcao)
                .HasColumnName("dt_data_confirmacao_cena_acao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Insulfilm)
                .HasColumnName("st_insulfilm")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.SobreInsulfilm)
                .HasColumnName("nm_sobre_insulfilm")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.DataConfirmacaoInsulfilm)
                .HasColumnName("dt_data_confirmacao_insulfilm")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Adesivagem)
                .HasColumnName("st_adesivagem")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.SobreAdesivagem)
                .HasColumnName("nm_sobre_adesivagem")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.DataConfirmacaoAdesivagem)
                .HasColumnName("dt_data_confirmacao_adesivagem")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Mecanica)
                .HasColumnName("st_mecanica")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.SobreMecanica)
                .HasColumnName("nm_sobre_mecanica")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.DataConfirmacaoMecanica)
                .HasColumnName("dt_data_confirmacao_mecanica")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.MotoristaCena)
                .HasColumnName("st_motorista_cena")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.SobreMotoristaCena)
                .HasColumnName("nm_sobre_motorista_cena")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.DataConfirmacaoMotoristaCena)
                .HasColumnName("dt_data_confirmacao_motorista_cena")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Reboque)
                .HasColumnName("st_reboque")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.SobreReboque)
                .HasColumnName("nm_sobre_reboque")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.DataConfirmacaoReboque)
                .HasColumnName("dt_data_confirmacao_reboque")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataAprovacao)
                .HasColumnName("dt_data_aprovacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.LoginAprovacao)
                .HasColumnName("nm_login_aprovacao")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataReprovacao)
                .HasColumnName("dt_data_reprovacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.LoginReprovacao)
                .HasColumnName("nm_login_reprovacao")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.JustificativaReprovacao)
                .HasColumnName("nm_justificativa_reprovacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.DataCancelamento)
                .HasColumnName("dt_cancelamento")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.JustificativaCancelamento)
                .HasColumnName("nm_justificativa_cancelamento")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.OutraJustificativa)
                .HasColumnName("nm_outra_justificativa")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.HasOne(mg => mg.PedidoItem)
               .WithMany(p => p.AcionamentoItens)
               .HasForeignKey(mg => mg.IdPedidoItem);

            builder.HasOne(mg => mg.Acionamento)
               .WithMany(p => p.AcionamentoPedidoItens)
               .HasForeignKey(mg => mg.IdAcionamento);

            builder.HasOne(mg => mg.AcionamentoItemLoginAprovacao)
               .WithMany(p => p.AcionamentoItemAprovacao)
               .HasForeignKey(mg => mg.LoginAprovacao);

            builder.HasOne(mg => mg.AcionamentoItemLoginReprovacao)
               .WithMany(p => p.AcionamentoItemReprovacao)
               .HasForeignKey(mg => mg.LoginReprovacao);
        }
    }
}
