using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<TaskEntity> Tasks { get; set; } = default!;
        public DbSet<TaskResponseEntity> TaskResponses { get; set; } = default!;
        public DbSet<ResponseAttachmentEntity> Attachments { get; set; } = default!;

        public DbSet<SubjectEntity> Subjects { get; set; } = default!;

        public DbSet<AcademicGroupEntity> AcademicGroups { get; set; } = default!;
        public DbSet<StudentEntity> Students { get; set; } = default!;
        public DbSet<TeacherEntity> Teachers { get; set; } = default!;

        public DbSet<PersonSubjectTable> PersonSubjectTable { get; set; } = default!;
        public DbSet<TaskPersonTable> PersonTaskTable { get; set; } = default!;
        public DbSet<UserRoleTable> UserRoleTable { get; set; } = default!;


        public DbSet<UserEntity> Users { get; set; } = default!;
        public DbSet<RoleEntity> Roles { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging(true);
#endif
        }

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
                        .HasForeignKey(k => k.UserId)
                        .HasPrincipalKey(p => p.Id),
                    j => j
                        .HasOne(s => s.Subject)
                        .WithMany(s => s.PersonSubjectTable)
                        .HasForeignKey(k => k.SubjectId)
                        .HasPrincipalKey(s => s.Id),
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
                        .HasForeignKey(k => k.UserId)
                        .HasPrincipalKey(p => p.Id),
                    j => j
                        .HasOne(s => s.Task)
                        .WithMany(s => s.TaskPersonTable)
                        .HasForeignKey(k => k.TaskId)
                        .HasPrincipalKey(t => t.Id),
                    j =>
                    {
                        j.HasKey(k => new { k.TaskId, k.UserId });
                        j.ToTable("TaskPersonTable");
                    }
                );

            modelBuilder
                .Entity<RoleEntity>()
                .HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity<UserRoleTable>(
                    j => j
                        .HasOne(u => u.User)
                        .WithMany(r => r.UserRoleTable)
                        .HasForeignKey(k => k.UserId)
                        .HasPrincipalKey(u => u.Id),
                    j => j
                        .HasOne(r => r.Role)
                        .WithMany(u => u.UserRoleTable)
                        .HasForeignKey(k => k.RoleId)
                        .HasPrincipalKey(r => r.Id),
                    j =>
                    {
                        j.HasKey(k => new { k.RoleId, k.UserId });
                        j.ToTable("UserRoleTable");
                    });

            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.Responses)
                .WithOne(r => r.Task)
                .OnDelete(DeleteBehavior.Cascade);

            SeedData.SeedUsersContext(modelBuilder);

            SeedData.SeedDataContext(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
