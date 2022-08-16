using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class ConteudoMap : IEntityTypeConfiguration<Conteudo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Conteudo> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_CONTEUDO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.Codigo)
                .HasColumnName("nm_codigo")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Nome)
               .HasColumnName("nm_nome")
               .HasColumnType("VARCHAR(500)")
               .HasConversion(stringConverter)
               .HasMaxLength(500)
               .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnName("nm_status")
                .HasColumnType("VARCHAR(1)")
                .HasConversion(stringConverter)
                .HasMaxLength(1)
                .IsRequired();

            builder.Property(c => c.Sigiloso)
                .HasColumnName("st_sigiloso")
                .HasColumnType("TINYINT(4)");
        }
    }
}
