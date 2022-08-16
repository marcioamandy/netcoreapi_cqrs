using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoVeiculoMap : IEntityTypeConfiguration<PedidoVeiculo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PedidoVeiculo> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO_VEICULO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdPedido)
                .HasColumnName("id_pedido")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.AcionadoPorLogin)
                .HasColumnName("nm_login_acionamento")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.DataAcionamento)
               .HasColumnName("dt_acionamento")
               .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataChegada)
               .HasColumnName("dt_chegada")
               .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataDevolucao)
                .HasColumnName("dt_devolucao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.LocalFaturamento)
                .HasColumnName("nm_local_faturamento")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);            

            builder.Property(c => c.IdStatus)
                .HasColumnName("id_status")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.CompradoPorLogin)
                .HasColumnName("nm_login_comprador")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.PreProducao)
                .HasColumnName("st_pre_producao")
                .HasColumnType("TINYINT(4)");

            builder.Property(c => c.DataEnvio)
                .HasColumnName("dt_envio")
                .HasColumnType("TIMESTAMP");

            builder.HasOne(mg => mg.Pedido)
               .WithOne(p => p.PedidoVeiculo)
               .HasForeignKey<PedidoVeiculo>(mg => mg.IdPedido);

            builder.HasOne(mg => mg.Status)
               .WithMany(p => p.PedidosVeiculo)
               .HasForeignKey(mg => mg.IdStatus);

            builder.HasOne(mg => mg.Acionador)
               .WithMany(p => p.PedidosLoginVeiculoAcionamento)
               .HasForeignKey(mg => mg.AcionadoPorLogin);

            builder.HasOne(mg => mg.Comprador)
               .WithMany(p => p.PedidosLoginVeiculoComprador)
               .HasForeignKey(mg => mg.CompradoPorLogin);
        }
    }
}
