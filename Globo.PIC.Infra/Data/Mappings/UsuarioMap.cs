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
	public class UsuarioMap : IEntityTypeConfiguration<Usuario>
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		public void Configure(EntityTypeBuilder<Usuario> builder)
		{

			var stringConverter = new ValueConverter<string, string>(
				//trim and remove duplicated spaces between words
				v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
				v => v.Trim()
			);

			builder
				.ToTable("TB_USUARIO")
				.HasKey(c => c.Login);

			builder.Property(c => c.Login)
				.HasColumnName("nm_login")
				.HasColumnType("VARCHAR(100)")
				.HasConversion(stringConverter)
				.HasMaxLength(100)
				.ValueGeneratedNever()
				.IsRequired();

			builder.Property(c => c.Name)
				.HasColumnName("nm_nome")
				.HasColumnType("VARCHAR(250)")
				.HasConversion(stringConverter)
				.HasMaxLength(250);

			builder.Property(c => c.LastName)
				.HasColumnName("nm_sobrenome")
				.HasColumnType("VARCHAR(500)")
				.HasConversion(stringConverter)
				.HasMaxLength(500);

			builder.Property(c => c.Apelido)
				.HasColumnName("nm_apelido")
				.HasColumnType("VARCHAR(500)")
				.HasConversion(stringConverter)
				.HasMaxLength(500);

			builder.Property(c => c.Email)
				.HasColumnName("nm_email")
				.HasColumnType("VARCHAR(500)")
				.HasConversion(stringConverter)
				.HasMaxLength(500);

			builder.Property(c => c.IsActive)
				.HasColumnName("st_ativo")
				.HasColumnType("TINYINT")
				.IsRequired();

			builder.Property(c => c.IdDepartamento)
				.HasColumnName("id_departamento")
				.HasColumnType("BIGINT(20)");

			builder.Property(c => c.IdUnidadeNegocio)
				.HasColumnName("id_unidade_negocio")
				.HasColumnType("BIGINT(20)");

			builder.HasOne(mg => mg.Departamento)
			   .WithMany(p => p.Usuarios)
			   .HasForeignKey(mg => mg.IdDepartamento);

			builder.HasOne(mg => mg.UnidadeNegocio)
			   .WithMany(p => p.Usuarios)
			   .HasForeignKey(mg => mg.IdUnidadeNegocio);

		}
	}
}