using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class UsuarioConteudosMap : IEntityTypeConfiguration<UsuarioConteudo>
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
				.HasKey(c => new { c.IdConteudo, c.Login });

			builder.Property(c => c.Login)
                .HasColumnName("nm_login")
                .HasColumnType("VARCHAR(100)")
                .HasConversion(stringConverter)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.IdConteudo)
				.HasColumnName("id_conteudo")
				.HasColumnType("BIGINT(20)")
				.IsRequired(); 

            builder.HasOne(mg => mg.Usuario)
                .WithMany(p => p.UsuariosConteudos)
                .HasForeignKey(mg => mg.Login);

            builder.HasOne(mg => mg.Conteudo)
				.WithMany(p => p.UsuariosConteudos)
				.HasForeignKey(mg => mg.IdConteudo);
		}
	}
}