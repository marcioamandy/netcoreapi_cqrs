﻿using Globo.PIC.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
			
			var stringConverter = new ValueConverter<string, string>(
				//trim and remove duplicated spaces between words
				v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
				v => v.Trim()
			);

			builder
                .ToTable("TB_ROLES_USUARIO")
				.HasKey(c => new { c.Name, c.Login});

			builder.Property(c => c.Login)
                .HasColumnName("nm_login")
                .HasColumnType("VARCHAR(100)")
				.HasConversion(stringConverter);

            builder.Property(c => c.Name)
                .HasColumnName("nm_role")
                .HasColumnType("VARCHAR(100)")
				.HasConversion(stringConverter);

            builder.HasOne(mg => mg.User)
                .WithMany(p => p.Roles)
                .HasForeignKey(mg => mg.Login);
        }
    }
}
