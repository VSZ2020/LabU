using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using LabU.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LabU.Pages.Admin.Teachers
{
    public class RemoveModel : PageModel
    {
        public RemoveModel(UnitOfWork u, ILogger<RemoveModel> logger)
        {
            _u = u;
            _logger = logger;
        }

        readonly UnitOfWork _u;
        readonly ILogger<RemoveModel> _logger;

        [BindProperty]
        public int TeacherId { get; set; }

        public string FullName { get; set; }

        [BindProperty]
        public List<RemoveTaskViewModel> Tasks { get; set; }

        [BindProperty]
        public List<RemoveSubjectViewModel> Subjects { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return RedirectToPage("./Index");

            if (User == null)
            {
                _logger.LogWarning($"Attempt to remove teacher with Id {id.Value} by anonymous user");
                return Unauthorized();
            }

            if (!User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                _logger.LogWarning($"Attempt to remove teacher with Id {id.Value} by user {User.Identity!.Name} (Id = {User.FindFirstValue(ClaimTypes.NameIdentifier)})");
                return Unauthorized();
            }

            var teacherEntity = await _u.PersonService.FindTeacherByIdAsync(id.Value);
            TeacherId = teacherEntity.Id;
            FullName = teacherEntity.ToFullName();

            var taskEntities = await _u.TasksService
                .GetAllAsync(
                t => t.TaskPersonTable.All(tp => tp.UserId == teacherEntity.Id), 
                includeProps: string.Join(",", nameof(TaskEntity.TaskPersonTable)));
            Tasks = taskEntities.Select(t => new RemoveTaskViewModel() { Id = t.Id, Name = t.Name }).ToList();

            var subjectEntities = await _u.SubjectsService.GetAllAsync(
                s => s.PersonSubjectTable.Any(ps => ps.UserId == teacherEntity.Id), 
                includeProps: string.Join(",", nameof(SubjectEntity.PersonSubjectTable)));
            Subjects = subjectEntities.Select(s => new RemoveSubjectViewModel() { Id = s.Id, Name = s.Name }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return RedirectToPage("./Index");

            if (User == null)
            {
                _logger.LogWarning($"Attempt to remove teacher with Id {id.Value} by anonymous user");
                return Unauthorized();
            }

            if (!User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                _logger.LogWarning($"Attempt to remove teacher with Id {id.Value} by user {User.Identity!.Name} (Id = {User.FindFirstValue(ClaimTypes.NameIdentifier)})");
                return Unauthorized();
            }

            if (Tasks != null)
            {
                foreach(var task in Tasks)
                {
                    await _u.TasksService.DetachPersonAsync(TeacherId, task.Id); 
                }
            }

            if (Subjects != null)
            {
                foreach (var subject in Subjects)
                {
                    await _u.SubjectsService.DetachPersonAsync(TeacherId, subject.Id);
                }
            }

            await _u.PersonService.RemoveTeacher(TeacherId);
            await _u.UserService.RemoveUserAsync(TeacherId);

            try
            {
                await _u.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"Can't detach user with id {TeacherId} from subjects and tasks. Exception: {ex.Message}");
                ModelState.AddModelError("","Не удалось открепить пользователя. Повторите попытку позже");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
