using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class NotificationMap : IEntityTypeConfiguration<Notificacao>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Notificacao> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_NOTIFICACAO")
                    .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                    .HasColumnName("id")
                    .HasColumnType("BIGINT(20)")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

            builder.Property(c => c.Title)
                .HasColumnName("ds_titulo")
                .HasColumnType("VARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("dt_criacao")
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            builder.Property(c => c.Link)
                .HasColumnName("ds_link")
                .HasColumnType("VARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
