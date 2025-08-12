using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Infrastructure.Configurations;

namespace ThinkEdu_Question_Service.Infrastructure.Configurations
{
    public class AnswerEntityTypeConfiguration : BaseStatusEntityTypeConfiguration<Answer, string>
    {
        public override void Configure(EntityTypeBuilder<Answer> builder)
        {
            base.Configure(builder);

            builder.ToTable("Answers");
            builder.Property(a => a.Id).HasColumnName("id");
            builder.Property(a => a.Content).IsRequired(false).HasMaxLength(255).HasColumnName("content").HasColumnType("varchar(2000)");
            builder.Property(a => a.FileUrl).IsRequired(false).HasMaxLength(255).HasColumnName("file_url");
            builder.Property(a => a.QuestionId).IsRequired(false).HasColumnName("question_id").HasColumnType("varchar(255)");
            builder.Property(a => a.IsCorrect).IsRequired().HasColumnName("is_correct");
            builder.Property(a => a.Tenant_Id).IsRequired(false).HasColumnName("tenant_id");
            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}
