using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Teachers
{
    public class EditModel : PageModel
    {
        public EditModel(IPersonRepository pr, IUserService us, IRoleService rs, ILogger<EditModel> logger)
        {
            _pr = pr;
            _us = us;
            _rs = rs;

            _logger = logger;
        }

        readonly IPersonRepository _pr;
        readonly IUserService _us;
        readonly IRoleService _rs;

        readonly ILogger<EditModel> _logger;

        [BindProperty]
        public EditTeacherViewModel Teacher { get; set; }

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

            var entity = await _pr.FindTeacherByIdAsync(id.Value);
            if (entity == null)
                return Redirect("./Index");

            Teacher = new EditTeacherViewModel()
            {
                Id = entity.Id,
                Username = entity.Account!.Username,
                Email = entity.Account.Email,
                IsActiveAccount = entity.Account.IsActiveAccount,
                IsEmailConfirmed = entity.Account.IsEmailConfirmed,

                LastName = entity.LastName,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,

                Address = entity.Address,
                Function = entity.Function,
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

            var entityTeacher = await _pr.FindTeacherByIdAsync(id.Value);
            var entityAccount = await _us.FindByIdAsync(id.Value);
            if (entityTeacher == null || entityAccount == null)
            {
                ModelState.AddModelError("", "Сущность пользователя потеряна. Не удалось сохранить");
                return Page();
            }

            entityAccount.Username = Teacher.Username;
            entityAccount.Email = Teacher.Email;
            entityAccount.IsEmailConfirmed = Teacher.IsEmailConfirmed;
            entityAccount.IsActiveAccount = Teacher.IsActiveAccount;

            entityTeacher.LastName = Teacher.LastName;
            entityTeacher.FirstName = Teacher.FirstName;
            entityTeacher.MiddleName = Teacher.MiddleName;

            entityTeacher.Address = Teacher.Address;
            entityTeacher.Function = Teacher.Function;

            try
            {
                await _pr.EditTeacherAsync(entityTeacher);
                await _us.UpdateUserAsync(entityAccount);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Can't save changes for teacher with Id = {id.Value}. Exception: {ex.Message}");
                ModelState.AddModelError("", "Не удалось сохранить изменения");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
