using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using LabU.Data.Repository;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Teachers
{
    public class AddModel : PageModel
    {
        public AddModel(UnitOfWork uow, ILogger<AddModel> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        readonly UnitOfWork _uow;
        readonly ILogger<AddModel> _logger;

        [BindProperty]
        public AddTeacherViewModel UserModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UserModel = new() { IsActiveAccount = true };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var teacherAccount = new UserEntity()
            {
                Username = UserModel.Username,
                PasswordHash = UserModel.Password,
                Email = UserModel.Email,
                IsActiveAccount = UserModel.IsActiveAccount,
                IsEmailConfirmed = true,
                LastVisit = DateTime.Now,
                Roles = new List<RoleEntity>() { (await _uow.RoleService.FindByNameAsync(UserRoles.TEACHER))! },
            };
            var newId = await _uow.UserService.CreateUserAsync(teacherAccount);

            var teacherEntity = new TeacherEntity()
            {
                FirstName = UserModel.FirstName,
                LastName = UserModel.LastName,
                MiddleName = UserModel.MiddleName,
                Address = UserModel.Address,
                Function = UserModel.Function,
                Account = teacherAccount,
                Id = newId,
            };

            await _uow.PersonService.CreateTeacher(teacherEntity);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Can't create user account for '{teacherAccount.Username}'. Exception: {ex.Message}");
                ModelState.AddModelError("", "Не удалось создать аккаунт. Повторите попытку позднее");
                return Page();
            }

            return Redirect("./Index");
        }
    }
}
