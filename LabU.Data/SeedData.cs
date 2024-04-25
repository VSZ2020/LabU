using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

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
            var data = JsonNode.Parse(File.ReadAllText("users.private.json"));

            var roles = DefaultRoles();
            builder
                .Entity<UserEntity>()
                .HasData(
                [
                    new UserEntity()
                    {
                        Id = 1,
                        Username = "admin",
                        PasswordHash = data!["admin"]!.GetValue<string>(),
                        IsActiveAccount = true,
                        IsEmailConfirmed = true,
                        Email = "admin@lab-u.ru",
                        LastVisit = DateTime.Now,
                    },
                    new UserEntity()
                    {
                        Id = 2,
                        Username = "teacher",
                        PasswordHash = "SoLLbbU372xbU9FEhU4UbeeVAug=",
                        IsActiveAccount = true,
                        IsEmailConfirmed = true,
                        Email = "vasechkin@lab-u.ru",
                        LastVisit = DateTime.Now.AddDays(-5),
                    },
                    new UserEntity()
                    {
                        Id = 3,
                        Username = "petrov",
                        PasswordHash = "itdHQ0I8rtzf7tyP4MuM63c74B4=",
                        IsActiveAccount = true,
                        IsEmailConfirmed = true,
                        Email = "petrov@lab-u.ru",
                        LastVisit = DateTime.Now.AddDays(-10),
                    },
                    new UserEntity()
                    {
                        Id = 4,
                        Username = "student",
                        PasswordHash = "IEA2oe9uc2DlNjAOp4xq60qTM90=",
                        IsActiveAccount = true,
                        IsEmailConfirmed = true,
                        Email = "ivanov@lab-u.ru",
                        LastVisit = DateTime.Now.AddDays(-5),
                    },
                    new UserEntity()
                    {
                        Id = 5,
                        Username = "alice",
                        PasswordHash = "UisnajVr3zkBPfq+os1D4UHsyeg=",
                        IsActiveAccount = false,
                        IsEmailConfirmed = true,
                        Email = "alisova@lab-u.ru",
                        LastVisit = DateTime.Now.AddDays(-48),
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
                    new UserRoleTable(){ UserId = 3, RoleId = 2},
                    new UserRoleTable(){ UserId = 4, RoleId = 3},
                    new UserRoleTable(){ UserId = 5, RoleId = 3},
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
                    new (){ SubjectId = 1, UserId = 4},
                    new (){ SubjectId = 1, UserId = 5},

                    new (){ SubjectId = 2, UserId = 1},
                    new (){ SubjectId = 2, UserId = 2},
                    new (){ SubjectId = 2, UserId = 3},
                    new (){ SubjectId = 2, UserId = 4},
                    new (){ SubjectId = 2, UserId = 5},

                    new (){ SubjectId = 3, UserId = 1},
                    new (){ SubjectId = 3, UserId = 2},
                    new (){ SubjectId = 3, UserId = 3},
                    new (){ SubjectId = 3, UserId = 4},
                    new (){ SubjectId = 3, UserId = 5},
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
                        LastName = "Admin",
                        FirstName = "Admin",
                        MiddleName = "",
                        Function = "Администратор портала",
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
                    new (){
                        Id = 3,
                        LastName = "Петров",
                        FirstName = "Петр",
                        MiddleName = "Петрович",
                        Function = "Преподаватель",
                        Address = "",
                    },
                ]);

            builder
                .Entity<StudentEntity>()
                .HasData(
                [
                    new (){
                        Id = 4,
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        CommandId = 1,
                        Course = 2,
                        AcademicGroupId = 1,
                    },
                    new (){
                        Id = 5,
                        LastName = "Алисова",
                        FirstName = "Алиса",
                        MiddleName = "Алисовна",
                        CommandId = 1,
                        Course = 2,
                        AcademicGroupId = 2,
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
                        Status = Core.ResponseState.ReturnedBack,
                        SubjectId = 3,
                        TaskType = Core.Enums.TaskTypes.AcademicGroupTask,
                    },

                    new(){
                        Id = 4,
                        Name = "Задание 4",
                        Deadline = DateTime.Now.AddHours(-3),
                        Description = "Просроченное задание",
                        IsAvailable = true,
                        Status = Core.ResponseState.ReturnedBack,
                        SubjectId = 1,
                        TaskType = Core.Enums.TaskTypes.AcademicGroupTask,
                    },
                    new(){
                        Id = 5,
                        Name = "Задание 5",
                        Deadline = DateTime.Now.AddHours(48),
                        Description = "Выполненное задание",
                        IsAvailable = true,
                        Status = Core.ResponseState.Accepted,
                        SubjectId = 2,
                        TaskType = Core.Enums.TaskTypes.SingleUser,
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
                    new (){ TaskId = 1, UserId = 4},
                    new (){ TaskId = 1, UserId = 5},

                    new (){ TaskId = 2, UserId = 1},
                    new (){ TaskId = 2, UserId = 2},
                    new (){ TaskId = 2, UserId = 4},
                    new (){ TaskId = 2, UserId = 5},

                    new (){ TaskId = 3, UserId = 1},
                    new (){ TaskId = 3, UserId = 3},
                    new (){ TaskId = 3, UserId = 4},
                    new (){ TaskId = 3, UserId = 5},

                    new (){ TaskId = 4, UserId = 1},
                    new (){ TaskId = 4, UserId = 2},
                    new (){ TaskId = 4, UserId = 3},
                    new (){ TaskId = 4, UserId = 4},
                    new (){ TaskId = 4, UserId = 5},

                    new (){ TaskId = 5, UserId = 1},
                    new (){ TaskId = 5, UserId = 2},
                    new (){ TaskId = 5, UserId = 3},
                    new (){ TaskId = 5, UserId = 4},
                    new (){ TaskId = 5, UserId = 5},
                ]);
        } 
        #endregion
    }
}
