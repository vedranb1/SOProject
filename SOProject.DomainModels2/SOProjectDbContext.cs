using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOProject.DomainModels2
{
    public partial class SOProjectDbContext : DbContext
    {
        public SOProjectDbContext() : base()
        {

        }

        public SOProjectDbContext(DbContextOptions<SOProjectDbContext> options) : base(options) 
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=root;database=SOPDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var Users = modelBuilder.Entity<User>();

            Users.HasKey(u => u.UserID);
            Users.Property(u => u.Email).IsRequired().HasColumnName("Email").HasColumnType("varchar(50)");
            Users.Property(u => u.PasswordHash).IsRequired().HasColumnName("PasswordHash").HasColumnType("varchar(500)");
            Users.Property(u => u.Username).IsRequired().HasColumnName("Username").HasColumnType("varchar(30)");
            Users.Property(u => u.Mobile).IsRequired().HasColumnName("Mobile").HasColumnType("varchar(50)");
            Users.Property(u => u.IsAdmin).HasColumnName("IsAdmin").HasColumnType("bit").HasDefaultValue(0);
            Users.HasMany<Question>(u => u.Questions).WithOne(q => q.User).HasForeignKey(q => q.UserID);
            Users.HasMany<Answer>(u => u.Answers).WithOne(a => a.User).HasForeignKey(a => a.UserID);

            var Categories = modelBuilder.Entity<Category>();

            Categories.HasKey(c => c.CategoryID);
            Categories.Property(c => c.CategoryName).IsRequired().HasColumnName("CategoryName").HasColumnType("varchar(50)");
            Categories.HasMany<Question>(c => c.Questions).WithOne(q => q.Category).HasForeignKey(q => q.CategoryID);

            var Questions = modelBuilder.Entity<Question>();

            Questions.HasKey(q => q.QuestionID);
            Questions.Property(q => q.QuestionName).IsRequired().HasColumnName("QuestionName").HasColumnType("varchar(500)");
            Questions.Property(q => q.QuestionDateAndTime).IsRequired().HasColumnName("QuestionDateAndTime").HasColumnType("datetime");
            Questions.Property(q => q.VotesCount).HasColumnName("VotesCount").HasColumnType("int");
            Questions.Property(q => q.AnswerCount).HasColumnName("AnswersCount").HasColumnType("int");
            Questions.Property(q => q.ViewsCount).HasColumnName("ViewsCount").HasColumnType("int");
            Questions.HasMany<Answer>(q => q.Answers).WithOne(a => a.Question).HasForeignKey(a => a.QuestionID);
           
            var Answers = modelBuilder.Entity<Answer>();

            Answers.HasKey(a => a.AnswerID);
            Answers.Property(a => a.AnswerText).IsRequired().HasColumnName("AnswerText").HasColumnType("varchar(1000)");
            Answers.Property(a => a.AnswerDateAndTime).IsRequired().HasColumnName("AnswerDateAndTime").HasColumnType("datetime");
            Answers.Property(a => a.VotesCount).HasColumnName("VotesCount").HasColumnType("int");
          
            var Votes = modelBuilder.Entity<Vote>();

            Votes.HasKey(v => v.VoteID);
            Votes.Property(v => v.VoteValue).IsRequired().HasColumnName("VoteValue").HasColumnType("int");

        }
            
                

        }

}

