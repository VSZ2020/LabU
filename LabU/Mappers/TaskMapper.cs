using LabU.Core.Entities;
using LabU.Models;
using LabU.Utils.Extensions;

namespace LabU.Mappers
{
    public class TaskMapper
    {
        public static TaskViewModel Map(TaskEntity entity, bool isFullStudentsName = false, bool isFullTeachersName = false, string namesDelimeter = ", ")
        {
            var subject = entity.Subject != null ? SubjectMapper.Map(entity.Subject) : null;

            var students = new List<StudentViewModel>();
            var reviewers = new List<TeacherViewModel>();
            if (entity.Users != null)
            {
                foreach(var user in entity.Users)
                {
                    if (user is TeacherEntity teacher)
                    {
                        reviewers.Add(TeacherMapper.Map(teacher));
                    }
                    if (user is StudentEntity student)
                    {
                        students.Add(StudentMapper.Map(student));
                    }
                }
            }

            return new TaskViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TaskStatusMapper.Map(entity.Status),
                Deadline = entity.Deadline,
                IsAvailable = entity.IsAvailable,
                Subject = subject?.Name ?? "-",
                SubjectId = entity.SubjectId,
                Students = string.Join(namesDelimeter, isFullStudentsName ? students.ToFullNames() : students.ToShortNames()),
                Reviewers = string.Join(namesDelimeter, isFullTeachersName ? reviewers.ToFullNames() : reviewers.ToShortNames()),
            };
        }
    }
}
