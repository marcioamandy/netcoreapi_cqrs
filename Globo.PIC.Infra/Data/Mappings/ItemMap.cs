using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_ITEM")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(c => c.IdTipo)
                .HasColumnName("id_tipo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdSubCategoria)
                .HasColumnName("id_subcategoria")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Valor)
                .HasColumnName("vl_valor")
                .HasColumnType("DECIMAL(18,2)")
                .HasDefaultValue("0");

            builder.Property(c => c.Fornecedor)
                .HasColumnName("nm_fornecedor")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.CodigoFornecedor)
                .HasColumnName("cd_fornecedor")
                .HasColumnType("VARCHAR(14)")
                .HasConversion(stringConverter)
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(c => c.NomeItem)
                .HasColumnName("nm_nome")
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

            builder.Property(c => c.NroAcordo)
                .HasColumnName("nm_nro_acordo")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.CodItem)
                .HasColumnName("nm_cod_item")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.LinhaAcordo)
                .HasColumnName("nr_linha_acordo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Observacao)
                .HasColumnName("nm_observacao")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.AcordoJuridico)
                .HasColumnName("nm_acordo_juridico")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100);

            builder.Property(c => c.IdTipoNegociacao)
                .HasColumnName("id_tipo_negociacao")
                .HasColumnType("BIGINT(20)");

            builder.HasOne(mg => mg.Tipo)
               .WithMany(p => p.ItemTipos)
               .HasForeignKey(mg => mg.IdTipo);

            builder.HasOne(mg => mg.SubCategoria)
               .WithMany(p => p.ItemSubCategorias)
               .HasForeignKey(mg => mg.IdSubCategoria);

            builder.HasOne(mg => mg.ItemVeiculo)
               .WithOne(p => p.Item);
        }
    }
}
