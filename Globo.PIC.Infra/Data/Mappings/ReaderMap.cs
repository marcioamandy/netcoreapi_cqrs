using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Infra.Data.Mappings
{
    public class ReaderMap : IEntityTypeConfiguration<Reader>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Reader> builder)
        {
            var stringConverter = new ValueConverter<string, string>(
                //trim and remove duplicated spaces between words
                v => string.IsNullOrEmpty(v) ? null : v.TrimAndRemoveSpaces(),
                v => v.Trim()
            );

            builder
                .ToTable("TB_NOTIFICACAO_LIDAS")
                    .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                    .HasColumnName("id")
                    .HasColumnType("BIGINT(20)")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

            builder.Property(c => c.Login)
                .HasColumnName("ds_login")
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.NotificationId)
                .HasColumnName("id_notificacao")
                .HasColumnType("BIGINT(20)")
                .IsRequired();

            builder.HasOne(mg => mg.Notification)
               .WithMany(p => p.Readers)
               .HasForeignKey(mg => mg.NotificationId);
        }
    }
}
