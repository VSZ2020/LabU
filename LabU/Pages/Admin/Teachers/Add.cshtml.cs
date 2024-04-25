using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Teachers
{
    public class AddModel : PageModel
    {
        public AddModel(IPersonRepository pr, IUserService us, IRoleService rs, ILogger<AddModel> logger)
        {
            _pr = pr;
            _us = us;
            _rs = rs;
            _logger = logger;
        }

        readonly IPersonRepository _pr;
        readonly IUserService _us;
        readonly IRoleService _rs;

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
                Roles = new List<RoleEntity>() { (await _rs.FindByNameAsync(UserRoles.TEACHER))! },
            };
            var newId = await _us.CreateUserAsync(teacherAccount);

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

            try
            {
                await _pr.CreateTeacher(teacherEntity);
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
