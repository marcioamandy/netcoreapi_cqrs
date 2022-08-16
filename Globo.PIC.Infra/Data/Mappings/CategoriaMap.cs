using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_CATEGORIA")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.IdCategoria)
                .HasColumnName("id_categoria")
                .HasColumnType("BIGINT(20)");

            builder.Property(c => c.Nome)
                .HasColumnName("nm_categoria")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(mg => mg.CategoriaPai)
               .WithMany(p => p.Categorias)
               .HasForeignKey(mg => mg.IdCategoria);
        }
    }
}
