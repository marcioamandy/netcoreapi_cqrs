using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class ItemCatalogoMap : IEntityTypeConfiguration<ItemCatalogo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ItemCatalogo> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_ITEM_CATALOGO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdItem)
                .HasColumnName("id_item")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.IdConteudo)
                .HasColumnName("id_conteudo")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.BloqueadoOutrosConteudos)
                .HasColumnName("st_bloqueado")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.Property(c => c.JustificativaBloqueio)
                .HasColumnName("nm_justificativa_bloqueio")
                .HasColumnType("VARCHAR(500)")
                .HasConversion(stringConverter)
                .HasMaxLength(500);

            builder.Property(c => c.AtivoAte)
                .HasColumnName("dt_ativo_ate")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataInicio)
                .HasColumnName("dt_data_inicio")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.DataFim)
                .HasColumnName("dt_data_fim")
                .HasColumnType("TIMESTAMP");

            builder.Property(c => c.Ativo)
                .HasColumnName("st_ativo")
                .HasColumnType("TINYINT(4)").HasDefaultValue(false);

            builder.HasOne(mg => mg.Item)
               .WithMany(p => p.ItemCatalogos)
               .HasForeignKey(mg => mg.IdItem);
        }
    }
}
