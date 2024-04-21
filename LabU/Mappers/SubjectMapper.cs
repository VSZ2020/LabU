using LabU.Core.Entities;
using LabU.Models;
using LabU.Utils.Extensions;

namespace LabU.Mappers
{
    public class SubjectMapper
    {
        public static SubjectViewModel Map(SubjectEntity entity, bool isStudentsFullName = false, bool isTeacherFullName = false, string usersDelimeter = ", ")
        {
            var students = new List<StudentViewModel>();
            var teachers = new List<TeacherViewModel>();
            if (entity.Users != null)
            {
                foreach (var user in entity.Users)
                {
                    if (user is TeacherEntity teacher)
                    {
                        teachers.Add(TeacherMapper.Map(teacher));
                    }
                    if (user is StudentEntity student)
                    {
                        students.Add(StudentMapper.Map(student));
                    }
                }
            }

            return new SubjectViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description, 
                AcademicTerm = AcademicTermMapper.Map(entity.AcademicTerm),
                AcademicYear = entity.AcademicYear,
                Students = string.Join(usersDelimeter, isStudentsFullName ? students.ToFullNames() : students.ToShortNames()),
                Teachers = string.Join(usersDelimeter, isTeacherFullName ? teachers.ToFullNames() : teachers.ToShortNames()),
            };
        }
    }
}
