using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class UnidadeNegocioMap : IEntityTypeConfiguration<UnidadeNegocio>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<UnidadeNegocio> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_UNIDADE_NEGOCIO")
                .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("BIGINT(20)")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.Codigo)
                .HasColumnName("nm_codigo")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.CodigoOrganizacaoInventario)
                .HasColumnName("nm_codigo_oi")
                .HasColumnType("VARCHAR(20)")
                .HasConversion(stringConverter)
                .HasMaxLength(20);

            builder.Property(c => c.Nome)
                .HasColumnName("nm_nome")
                .HasColumnType("VARCHAR(200)")
                .HasConversion(stringConverter)
                .HasMaxLength(200);

            builder.Property(c => c.Uf)
              .HasColumnName("nm_uf")
              .HasColumnType("VARCHAR(100)")
              .HasConversion(stringConverter)
              .HasMaxLength(100);
        }
    }
}
