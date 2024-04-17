using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<UserEntity> Users { get; set; } = default!;
        public DbSet<RoleEntity> Roles { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<RoleEntity> defaultRoles =
            [
                new (){ Id = 1, Name = "admin", NormalizedName = "ADMIN" },
                new () { Id = 2, Name = "teacher", NormalizedName = "TEACHER" },
                new () { Id = 3, Name = "student", NormalizedName = "student" },
            ];
            modelBuilder.Entity<RoleEntity>().HasData(defaultRoles);

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity()
                {
                    //TODO: Read from external file
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "abc",
                    IsActiveAccount = true, 
                    IsEmailConfirmed = true,
                    Email = "", 
                    Roles = [defaultRoles[0]],
                });

            modelBuilder
                .Entity<RoleEntity>()
                .HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity<UserRoleTable>(
                    j => j
                        .HasOne(u => u.User)
                        .WithMany(r => r.UserRoleTable)
                        .HasForeignKey(k => k.UserId),
                    j => j
                        .HasOne(r => r.Role)
                        .WithMany(u => u.UserRoleTable)
                        .HasForeignKey(k => k.RoleId),
                    j =>
                    {
                        j.HasKey(k => new { k.RoleId, k.UserId });
                        j.ToTable("UserRoleTable");
                    });
            base.OnModelCreating(modelBuilder);
        }
    }
}
