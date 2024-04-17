using LabU.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<TaskEntity> Tasks { get; set; } = default!;
        public DbSet<TaskResponseEntity> TaskResponses { get; set; } = default!;
        public DbSet<ResponseAttachmentEntity> Attachments { get; set; } = default!;

        public DbSet<SubjectEntity> Subjects { get; set; } = default!;

        public DbSet<AcademicGroupEntity> AcademicGroups { get; set; } = default!;
        public DbSet<StudentEntity> Students { get; set; } = default!;
        public DbSet<TeacherEntity> Teachers { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BasePersonEntity>().UseTpcMappingStrategy();
            
            modelBuilder
                .Entity<SubjectEntity>()
                .HasMany(s => s.Users)
                .WithMany(t => t.Subjects)
                .UsingEntity<PersonSubjectTable>(
                    j => j
                        .HasOne(p => p.Person)
                        .WithMany(p => p.PersonSubjectTable)
                        .HasForeignKey(k => k.UserId),
                    j => j
                        .HasOne(s => s.Subject)
                        .WithMany(s => s.PersonSubjectTable)
                        .HasForeignKey(k => k.SubjectId),
                    j =>
                    {
                        j.HasKey(k => new { k.SubjectId, k.UserId });
                        j.ToTable("PersonSubjectTable");
                    }
                );
            
            modelBuilder
                .Entity<TaskEntity>()
                .HasMany(s => s.Users)
                .WithMany(t => t.Tasks)
                .UsingEntity<TaskPersonTable>(
                    j => j
                        .HasOne(p => p.Person)
                        .WithMany(p => p.TaskPersonTable)
                        .HasForeignKey(k => k.UserId),
                    j => j
                        .HasOne(s => s.Task)
                        .WithMany(s => s.TaskPersonTable)
                        .HasForeignKey(k => k.TaskId),
                    j =>
                    {
                        j.HasKey(k => new { k.TaskId, k.UserId });
                        j.ToTable("TaskPersonTable");
                    }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
