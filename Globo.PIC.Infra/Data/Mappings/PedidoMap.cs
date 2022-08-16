using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_PEDIDO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.DataPedido)
                .HasColumnName("dt_pedido")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.NroItens)
                .HasColumnName("nr_itens")
                .HasColumnType("BIGINT(20)")
                .HasDefaultValue("0")
                .IsRequired();

            builder.Property(c => c.ValorItens)
                .HasColumnName("vl_total")
                .HasColumnType("DECIMAL(18,2)")
                .HasDefaultValue("0")
                .IsRequired();

            builder.Property(c => c.Titulo)
                .HasColumnName("nm_titulo")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.IdConteudo)
                .HasColumnName("id_conteudo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdProjeto)
                .HasColumnName("id_projeto")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdTarefa)
                .HasColumnName("id_tarefa")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DescricaoTarefa)
             .HasColumnName("nm_tarefa_descricao")
             .HasColumnType("VARCHAR(200)")
             .HasConversion(stringConverter)
             .HasMaxLength(200);

            builder.Property(c => c.LocalEntrega)
                .HasColumnName("nm_local_entrega")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Observacao)
                .HasColumnName("nm_observacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.DataCriacao)
                .HasColumnName("dt_criacao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.CriadoPorLogin)
                .HasColumnName("nm_criado_por")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.AtualizadoPorLogin)
                .HasColumnName("nm_atualizado_por")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Ativo)
                .HasColumnName("st_ativo")
                .HasColumnType("TINYINT(4)");

            builder.Property(c => c.Finalidade)
             .HasColumnName("nm_finalidade")
             .HasColumnType("VARCHAR(100)")
             .HasConversion(stringConverter)
             .HasMaxLength(100);

            builder.Property(c => c.CentroCusto)
             .HasColumnName("nm_centro_custo")
             .HasColumnType("VARCHAR(100)")
             .HasConversion(stringConverter)
             .HasMaxLength(100);

            builder.Property(c => c.Conta)
             .HasColumnName("nm_conta")
             .HasColumnType("VARCHAR(100)")
             .HasConversion(stringConverter)
             .HasMaxLength(100);

            builder.Property(c => c.DataDevolucao)
                .HasColumnName("dt_devolucao")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DevolvidoPorLogin)
                .HasColumnName("nm_login_devolucao")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.JustificativaDevolucao)
            .HasColumnName("nm_justificativa_devolucao")
            .HasColumnType("VARCHAR(500)")
            .HasConversion(stringConverter)
            .HasMaxLength(500);

            builder.Property(c => c.DataCancelamento)
            .HasColumnName("dt_cancelamento")
            .HasColumnType("TIMESTAMP");

            builder.Property(c => c.CanceladoPorLogin)
            .HasColumnName("nm_login_cancelamento")
            .HasColumnType("VARCHAR(200)")
            .HasConversion(stringConverter)
            .HasMaxLength(200);

            builder.Property(c => c.JustificativaCancelamento)
            .HasColumnName("nm_justificativa_cancelamento")
            .HasColumnType("VARCHAR(500)")
            .HasConversion(stringConverter)
            .HasMaxLength(500);

            builder.Property(c => c.IdTipo)
                .HasColumnName("id_tipo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdTag)
                .HasColumnName("id_tag")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DestinationOrganizationId)
                .HasColumnName("id_organizacao")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DeliverToLocationId)
               .HasColumnName("id_localizacao")
               .HasColumnType("BIGINT(20)");

            builder.Property(c => c.DestinationOrganizationCode)
             .HasColumnName("nm_codigo_organizacao")
             .HasColumnType("VARCHAR(200)")
             .HasConversion(stringConverter)
             .HasMaxLength(200);

            builder.Property(c => c.DeliverToLocationCode)
               .HasColumnName("nm_codigo_localizacao")
               .HasColumnType("VARCHAR(200)")
               .HasConversion(stringConverter)
               .HasMaxLength(200);

            builder.HasOne(mg => mg.CanceladoPor)
               .WithMany(p => p.PedidosLoginCancelamento)
               .HasForeignKey(mg => mg.CanceladoPorLogin);

            builder.HasOne(mg => mg.CriadoPor)
               .WithMany(p => p.PedidosLoginCriadoPor)
               .HasForeignKey(mg => mg.CriadoPorLogin);

            builder.HasOne(mg => mg.AtualizadoPor)
               .WithMany(p => p.PedidosLoginAtualizadoPor)
               .HasForeignKey(mg => mg.AtualizadoPorLogin);

            builder.HasOne(mg => mg.DevolvidoPor)
               .WithMany(p => p.PedidosLoginDevolucao)
               .HasForeignKey(mg => mg.DevolvidoPorLogin);

            builder.HasOne(mg => mg.Conteudo)
               .WithMany(p => p.Pedidos)
               .HasForeignKey(mg => mg.IdConteudo);

            builder.HasOne(mg => mg.PedidoArte)
               .WithOne(p => p.Pedido);

            builder.HasOne(mg => mg.PedidoVeiculo)
               .WithOne(p => p.Pedido);

        }
    }
}
