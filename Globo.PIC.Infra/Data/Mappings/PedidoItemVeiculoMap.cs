using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoItemVeiculoMap : IEntityTypeConfiguration<PedidoItemVeiculo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoItemVeiculo> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_ITEM_VEICULO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedidoItem)
                .HasColumnName("id_pedido_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.PersonagemUtilizacao)
                .HasColumnName("nm_personagem_utilizacao")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Modelo)
                .HasColumnName("nm_modelo")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.IdTipo)
                .HasColumnName("id_tipo_veiculos")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdSubCategoria)
                .HasColumnName("id_subcategoria_veiculos")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.NroOpcoes)
                .HasColumnName("nr_opcoes")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0")
                .IsRequired();

            builder.Property(c => c.Ano)
                .HasColumnName("nr_ano")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Cor)
                .HasColumnName("nm_cor")
                .HasColumnType("VARCHAR(30)")
                .HasConversion(stringConverter)
                .HasMaxLength(30);

            builder.Property(c => c.DataChegadaVeiculo)
                .HasColumnName("dt_chegada")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataDevolucaoVeiculo)
                .HasColumnName("dt_devolucao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Continuidade)
                .HasColumnName("st_continuidade")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.CenaAcao)
                .HasColumnName("st_cena_acao")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.SobreCenaAcao)
                .HasColumnName("nm_sobrecenaacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.IdStatus)
                .HasColumnName("id_status")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdOrigem)
                .HasColumnName("id_origem")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Observacao)
                .HasColumnName("nm_observacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.LocalGravacao)
                .HasColumnName("nm_localgravacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.Passageiros)
                .HasColumnName("nr_passageiros")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.HorasVoo)
                .HasColumnName("nm_horasvoo")
                .HasColumnType("VARCHAR(50)")
                .HasConversion(stringConverter)
                .HasMaxLength(50);

            builder.Property(c => c.HorasParado)
                .HasColumnName("nm_horasparado")
                .HasColumnType("VARCHAR(50)")
                .HasConversion(stringConverter)
                .HasMaxLength(50);

            builder.Property(c => c.Tag)
                .HasColumnName("id_tag")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Necessidades)
                .HasColumnName("nm_necessidades")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.ValorMaximo)
                .HasColumnName("vl_valor_maximo")
                .HasColumnType("DECIMAL(18,2)")
                .HasDefaultValue("0");

            builder.Property(c => c.LocalFaturamento)
                .HasColumnName("nm_local_faturamento")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataDevolver)
                .HasColumnName("dt_devolver")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.JustificativaDevolver)
                .HasColumnName("nm_justificativa_devolver")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.HasOne(mg => mg.Status)
               .WithMany(p => p.Status)
               .HasForeignKey(mg => mg.IdStatus);

            builder.HasOne(mg => mg.Tipo)
               .WithMany(p => p.PedidosVeiculosTipos)
               .HasForeignKey(mg => mg.IdTipo);

            builder.HasOne(mg => mg.SubCategoria)
               .WithMany(p => p.PedidosVeiculosSubCategorias)
               .HasForeignKey(mg => mg.IdSubCategoria);

            builder.HasOne(mg => mg.PedidoItem)
               .WithOne(p => p.PedidoItemVeiculo)
               .HasForeignKey<PedidoItemVeiculo>(mg => mg.IdPedidoItem);
        }
    }
}
