using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Infrastructure.Configurations
{
    public class BaseStatusEntityTypeConfiguration<T, TKey> : BaseEntityTypeConfiguration<T, TKey>
        where T : BaseStatusEntity<TKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(b => b.Status).HasColumnName("status").HasColumnType("varchar(50)").HasMaxLength(20);
        }
    }
}