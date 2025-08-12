using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Infrastructure.Configurations
{
    public abstract class BaseEntityTypeConfiguration<T, TKey> : IEntityTypeConfiguration<T>
      where T : BaseEntity<TKey>
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);
    }
}