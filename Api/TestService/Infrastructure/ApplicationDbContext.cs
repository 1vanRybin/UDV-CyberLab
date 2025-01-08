using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Test?> Tests { get; set; }
        public DbSet<QuestionBase> Questions { get; set; }  
        public DbSet<QuestionCompliance> QuestionCompliances { get; set; }
        public DbSet<QuestionFile> QuestionFiles { get; set; }
        public DbSet<QuestionOpen> QuestionOpens { get; set; }
        public DbSet<QuestionVariant> QuestionVariants { get; set; }
        public DbSet<UserTest?> UserTests { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionBase>()
                .HasDiscriminator<string>("QuestionType") 
                .HasValue<QuestionCompliance>("Compliance")
                .HasValue<QuestionFile>("File")
                .HasValue<QuestionOpen>("Open")
                .HasValue<QuestionVariant>("Variant");

            modelBuilder.Entity<QuestionBase>()
                .HasOne(q => q.Test)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTest>()
                .HasOne(ut => ut.Test)
                .WithMany(t => t.UserTests)
                .HasForeignKey(ut => ut.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
