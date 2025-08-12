using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Infrastructure.Configurations;

namespace ThinkEdu_Question_Service.Infrastructure.Configurations
{
    public class QuestionEntityTypeConfiguration : BaseStatusEntityTypeConfiguration<Question, string>
    {
        public override void Configure(EntityTypeBuilder<Question> builder)
        {
            base.Configure(builder);

            builder.ToTable("Questions");
            builder.Property(q => q.Id).HasColumnName("id");
            builder.Property(q => q.Title).IsRequired().HasMaxLength(2000).HasColumnName("title").HasColumnType("varchar(2000)");
            builder.Property(q => q.Type).IsRequired().HasMaxLength(50).HasColumnName("type").HasColumnType("varchar(50)");
            builder.Property(q => q.Group).IsRequired().HasMaxLength(50).HasColumnName("group").HasColumnType("varchar(50)");
            builder.Property(q => q.Level).IsRequired().HasMaxLength(50).HasColumnName("level").HasColumnType("varchar(50)");
            builder.Property(q => q.FileUrl).IsRequired(false).HasMaxLength(2000).HasColumnName("file_url").HasColumnType("varchar(2000)");
            builder.Property(q => q.Explanation).IsRequired(false).HasMaxLength(2000).HasColumnName("explanation").HasColumnType("varchar(2000)");
            builder.Property(q => q.ParentId).IsRequired(false).HasMaxLength(255).HasColumnName("parent_id").HasColumnType("varchar(2555)");
            builder.Property(q => q.STT).IsRequired(false).HasMaxLength(10).HasColumnName("stt").HasColumnType("int");
            builder.Property(q => q.LessonId).IsRequired(false).HasMaxLength(255).HasColumnName("lesson_id").HasColumnType("varchar(255)");
            builder.Property(q => q.Tenant_Id).IsRequired(false).HasMaxLength(255).HasColumnName("tenant_id").HasColumnType("varchar(255)");
        }
    }
}