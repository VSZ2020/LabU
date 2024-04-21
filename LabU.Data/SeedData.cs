using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data
{
    public class SeedData
    {
        public static void SeedDataContext(ModelBuilder builder)
        {
            SeedPersons(builder);
            SeedAcademicGroups(builder);
            SeedSubjects(builder);
            SeedTasks(builder);

            SeedPersonSubjectRelationships(builder);
            SeedTaskPersonRelationships(builder);
        }

        public static void SeedUsersContext(ModelBuilder builder)
        {
            SeedRoles(builder);
            SeedUsers(builder);
            SeedUserRoleRelationships(builder);
        }


        #region DefaultRoles
        /// <summary>
        /// Доступные по-умолчанию роли пользователя
        /// </summary>
        /// <returns></returns>
        public static RoleEntity[] DefaultRoles()
        {
            return [
                new RoleEntity()
                {
                    Id = 1,
                    Name = UserRoles.ADMINISTRATOR,
                    NormalizedName = UserRoles.ADMINISTRATOR.ToUpper()
                },
                new RoleEntity()
                {
                    Id = 2,
                    Name = UserRoles.TEACHER,
                    NormalizedName = UserRoles.TEACHER.ToUpper()
                },
                new RoleEntity()
                {
                    Id = 3,
                    Name = UserRoles.STUDENT,
                    NormalizedName = UserRoles.STUDENT.ToUpper()
                },
                new RoleEntity()
                {
                    Id = 4,
                    Name = UserRoles.GUEST,
                    NormalizedName = UserRoles.GUEST.ToUpper()
                }
            ];
        } 
        #endregion

        #region SeedRoles
        public static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<RoleEntity>().HasData(DefaultRoles());
        } 
        #endregion

        #region SeedUsers
        public static void SeedUsers(ModelBuilder builder)
        {
            var roles = DefaultRoles();
            builder
                .Entity<UserEntity>()
                .HasData(
                [
                    new UserEntity()
                    {
                        //TODO: Read from external file
                        Id = 1,
                        Username = "admin",
                        PasswordHash = "admin",
                        IsActiveAccount = true,
                        IsEmailConfirmed = true,
                        Email = "admin@lab-u.ru",
                        LastVisit = DateTime.Now,
                    },
                    new UserEntity()
                    {
                        Id = 2,
                        Username = "teacher",
                        PasswordHash = "teacher",
                        IsActiveAccount = true,
                        IsEmailConfirmed = true,
                        Email = "vasechkin@lab-u.ru",
                        LastVisit = DateTime.Now.AddDays(-5),
                    },
                    new UserEntity()
                    {
                        Id = 3,
                        Username = "student",
                        PasswordHash = "student",
                        IsActiveAccount = true,
                        IsEmailConfirmed = true,
                        Email = "ivanov@lab-u.ru",
                        LastVisit = DateTime.Now.AddDays(-10),
                    },
                ]);
        }
        #endregion

        #region SeedUserRoleRelationships
        public static void SeedUserRoleRelationships(ModelBuilder builder)
        {
            builder
                .Entity<UserRoleTable>()
                .HasData(
                [
                    new UserRoleTable(){ UserId = 1, RoleId = 1},
                    new UserRoleTable(){ UserId = 1, RoleId = 2},
                    new UserRoleTable(){ UserId = 2, RoleId = 2},
                    new UserRoleTable(){ UserId = 3, RoleId = 3},
                ]);
        } 
        #endregion

        #region SeedSubjects
        public static void SeedSubjects(ModelBuilder builder)
        {
            var subjects = new List<SubjectEntity>()
            {
                new()
                {
                    Id = 1,
                    Name = "Физика",
                    Description = "",
                    AcademicYear = "2023-2024",
                    AcademicTerm = Core.AcademicTerms.Spring,
                },
                new()
                {
                    Id = 2,
                    Name = "Биология",
                    Description = "",
                    AcademicYear = "2023-2024",
                    AcademicTerm = Core.AcademicTerms.Spring,
                },
                new()
                {
                    Id = 3,
                    Name = "История",
                    Description = "",
                    AcademicYear = "2023-2024",
                    AcademicTerm = Core.AcademicTerms.Spring,
                },
            };
            builder
                .Entity<SubjectEntity>()
                .HasData(subjects);
        }
        #endregion

        #region SeedPersonSubjectRelationships
        public static void SeedPersonSubjectRelationships(ModelBuilder builder)
        {
            builder
                .Entity<PersonSubjectTable>()
                .HasData(
                [
                    new (){ SubjectId = 1, UserId = 1},
                    new (){ SubjectId = 1, UserId = 2},
                    new (){ SubjectId = 1, UserId = 3},

                    new (){ SubjectId = 2, UserId = 2},
                    new (){ SubjectId = 2, UserId = 3},

                    new (){ SubjectId = 3, UserId = 2},
                    new (){ SubjectId = 3, UserId = 3},
                ]);
        } 
        #endregion

        #region SeedPersons
        public static void SeedPersons(ModelBuilder builder)
        {
            builder
                .Entity<TeacherEntity>()
                .HasData(
                [
                    new (){
                        Id = 1,
                        LastName = "Adminov",
                        FirstName = "Admin",
                        MiddleName = "Adminovich",
                        Function = "",
                        Address = "",
                    },
                    new (){
                        Id = 2,
                        LastName = "Васечкин",
                        FirstName = "Василий",
                        MiddleName = "Васильевич",
                        Function = "Преподаватель",
                        Address = "",
                    },
                ]);

            builder
                .Entity<StudentEntity>()
                .HasData(
                [
                    new (){
                        Id = 3,
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        CommandId = 1,
                        Cource = 2,
                        AcademicGroupId = 1,
                    },
                ]);
        } 
        #endregion

        #region SeedAcademicGroups
        public static void SeedAcademicGroups(ModelBuilder builder)
        {
            builder
                .Entity<AcademicGroupEntity>()
                .HasData(
                [
                    new(){
                        Id = 1,
                        Name = "Гр-1000"
                    },
                    new(){
                        Id = 2,
                        Name = "Гр-2000"
                    },
                    new(){
                        Id = 3,
                        Name = "Гр-3000"
                    },
                ]);
        }
        #endregion

        #region SeedTasks
        public static void SeedTasks(ModelBuilder builder)
        {
            builder
                .Entity<TaskEntity>()
                .HasData(
                [
                    new(){
                        Id = 1,
                        Name = "Задание 1",
                        Deadline = DateTime.Now.AddDays(14),
                        Description = "",
                        IsAvailable = true,
                        Status = Core.ResponseState.Undefined,
                        SubjectId = 1,
                        TaskType = Core.Enums.TaskTypes.AcademicGroupTask,
                    },
                    new(){
                        Id = 2,
                        Name = "Задание 2",
                        Deadline = DateTime.Now.AddDays(7),
                        Description = "",
                        IsAvailable = true,
                        Status = Core.ResponseState.WaitForRevision,
                        SubjectId = 2,
                        TaskType = Core.Enums.TaskTypes.AcademicGroupTask,
                        TaskPersonTable = new List<TaskPersonTable>(){
                            
                        }
                    },
                    new(){
                        Id = 3,
                        Name = "Задание 3",
                        Deadline = DateTime.Now.AddHours(10),
                        Description = "Описание задания с подходящим сроком исполнения",
                        IsAvailable = true,
                        Status = Core.ResponseState.UnderReview,
                        SubjectId = 3,
                        TaskType = Core.Enums.TaskTypes.AcademicGroupTask,
                    },

                    new(){
                        Id = 4,
                        Name = "Задание 4",
                        Deadline = DateTime.Now.AddHours(-3),
                        Description = "Просроченное задание",
                        IsAvailable = true,
                        Status = Core.ResponseState.UnderReview,
                        SubjectId = 1,
                        TaskType = Core.Enums.TaskTypes.AcademicGroupTask,
                    },
                ]);
        }
        #endregion

        #region SeedTaskPersonRelationships
        public static void SeedTaskPersonRelationships(ModelBuilder builder)
        {
            builder
                .Entity<TaskPersonTable>()
                .HasData(
                [
                    new (){ TaskId = 1, UserId = 1},
                    new (){ TaskId = 1, UserId = 2},
                    new (){ TaskId = 1, UserId = 3},

                    new (){ TaskId = 2, UserId = 2},
                    new (){ TaskId = 2, UserId = 3},

                    new (){ TaskId = 3, UserId = 2},
                    new (){ TaskId = 3, UserId = 3},
                ]);
        } 
        #endregion
    }
}
