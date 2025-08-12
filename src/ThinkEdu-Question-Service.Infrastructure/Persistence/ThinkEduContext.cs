using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Infrastructure.Persistence
{
    public class ThinkEduContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public ThinkEduContext(DbContextOptions<ThinkEduContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(
            //    Assembly.GetExecutingAssembly(),
            //    t => t.GetInterfaces().Any(i =>
            //        i.IsGenericType && !t.IsAbstract &&
            //        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
            //        typeof(BaseEntity<>).IsAssignableFrom(i.GenericTypeArguments[0]))
            //);

        }
    }
}