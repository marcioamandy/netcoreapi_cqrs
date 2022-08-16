using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class AcionamentoMap : IEntityTypeConfiguration<Acionamento>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Acionamento> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_ACIONAMENTO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedido)
                .HasColumnName("id_pedido_veiculos")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Roteiro)
                .HasColumnName("nm_roteiro")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.LocalEntrega)
                .HasColumnName("nm_local_entrega")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.DataSaida)
                .HasColumnName("dt_data_saida")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.HoraHigienizacaoSet)
                .HasColumnName("nm_hora_higienizacao_set")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.DataInicioGravacao)
                .HasColumnName("dt_data_inicio_gravacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataTerminoGravacao)
                .HasColumnName("dt_data_termino_gravacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Observacao)
                .HasColumnName("nm_observacao")
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

            builder.HasOne(mg => mg.PedidoVeiculo)
               .WithMany(p => p.Acionamento)
               .HasForeignKey(mg => mg.IdPedido);
        }
    }
}
