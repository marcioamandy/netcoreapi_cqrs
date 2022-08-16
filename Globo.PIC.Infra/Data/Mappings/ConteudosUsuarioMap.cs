using Globo.PIC.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class ConteudosUsuarioMap : IEntityTypeConfiguration<UsuarioConteudo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<UsuarioConteudo> builder)
        {
			
			var stringConverter = new ValueConverter<string, string>(
				//trim and remove duplicated spaces between words
				v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
				v => v.Trim()
			);

			builder
                .ToTable("TB_USUARIO_CONTEUDOS")
				.HasKey(c => new { c.Login, c.IdConteudo});

			builder.Property(c => c.Login)
                .HasColumnName("nm_login")
                .HasColumnType("VARCHAR(100)")
				.HasConversion(stringConverter);

            builder.Property(c => c.IdConteudo)
                .HasColumnName("id_conteudo")
                .HasColumnType("BIGINT(20)")
                .IsRequired();  
        }
    }
}
