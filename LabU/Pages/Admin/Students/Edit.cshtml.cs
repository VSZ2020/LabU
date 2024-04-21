using LabU.Core.Identity;
using LabU.Data.Repository;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Students
{
    public class EditModel : PageModel
    {
        public EditModel(UnitOfWork u, ILogger<EditModel> logger)
        {
            _u = u;
            _logger = logger;
        }

        readonly UnitOfWork _u;
        readonly ILogger<EditModel> _logger;

        [BindProperty]
        public EditStudentViewModel Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (id == null)
            {
                return NotFound($"Идентификатор не задан");
            }

            var entity = await _u.PersonService.FindStudentByIdAsync(id.Value);
            if (entity == null)
                return Redirect("./Index");

            Student = new EditStudentViewModel()
            {
                Id = entity.Id,
                Username = entity.Account!.Username,
                Email = entity.Account.Email,
                IsActiveAccount = entity.Account.IsActiveAccount,
                IsEmailConfirmed = entity.Account.IsEmailConfirmed,

                LastName = entity.LastName,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,

                Course = entity.Cource,
                AcademicGroupId = entity.AcademicGroupId,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (id == null)
            {
                return NotFound("Идентификатора не существует");
            }

            if (!ModelState.IsValid)
            { return Page(); }

            var entityStudent = await _u.PersonService.FindStudentByIdAsync(id.Value);
            var entityAccount = await _u.UserService.FindByIdAsync(id.Value);
            if (entityStudent == null || entityAccount == null)
            {
                ModelState.AddModelError("", "Сущность пользователя потеряна. Не удалось сохранить");
                return Page();
            }

            entityAccount.Username = Student.Username;
            entityAccount.Email = Student.Email;
            entityAccount.IsEmailConfirmed = Student.IsEmailConfirmed;
            entityAccount.IsActiveAccount = Student.IsActiveAccount;

            entityStudent.LastName = Student.LastName;
            entityStudent.FirstName = Student.FirstName;
            entityStudent.MiddleName = Student.MiddleName;

            entityStudent.Cource = Student.Course;
            entityStudent.AcademicGroupId = Student.AcademicGroupId;

            try
            {
                await _u.PersonService.EditStudent(entityStudent);
                await _u.UserService.UpdateUserAsync(entityAccount);
                await _u.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Can't save changes for student with Id = {id.Value}. Exception: {ex.Message}");
                ModelState.AddModelError("", "Не удалось сохранить изменения");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
