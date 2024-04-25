using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Students
{
    public class AddModel : PageModel
    {
        public AddModel(IPersonRepository pr, IUserService us, IRoleService rs, IAcademicGroupsRepository agr, ILogger<AddModel> logger)
        {
            _pr = pr;
            _us = us;
            _rs = rs;
            _agr = agr;
            _logger = logger;
        }

        readonly IPersonRepository _pr;
        readonly IUserService _us;
        readonly IRoleService _rs;
        readonly IAcademicGroupsRepository _agr;
        readonly ILogger<AddModel> _logger;

        [BindProperty]
        public AddStudentViewModel UserModel { get; set; }

        public SelectList AcademicGroups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UserModel = new() { IsActiveAccount = true };
            PopulateGroupsList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateGroupsList();
                return Page(); 
            }

            var userAccount = new UserEntity()
            {
                Username = UserModel.Username,
                PasswordHash = UserModel.Password,
                Email = UserModel.Email,
                IsActiveAccount = UserModel.IsActiveAccount,
                IsEmailConfirmed = true,
                LastVisit = DateTime.Now,
                Roles = new List<RoleEntity>() { (await _rs.FindByNameAsync(UserRoles.TEACHER))! },
            };
            var newId = await _us.CreateUserAsync(userAccount);

            var studentEntity = new StudentEntity()
            {
                FirstName = UserModel.FirstName,
                LastName = UserModel.LastName,
                MiddleName = UserModel.MiddleName,
                Course = UserModel.Course,
                AcademicGroupId = UserModel.AcademicGroupId,
                CommandId = UserModel.CommandId,
                Account = userAccount,
                Id = newId,
            };

            try
            {
                await _pr.CreateStudent(studentEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Can't create user account for '{userAccount.Username}'. Exception: {ex.Message}");
                ModelState.AddModelError("", "Не удалось создать аккаунт. Повторите попытку позднее");
                return Page();
            }

            return Redirect("./Index");
        }

        private async void PopulateGroupsList(object selectedGroup = null)
        {
            var groups = (await _agr.GetAllGroupsAsync()).Select(AcademicGroupMapper.Map).ToList();
            AcademicGroups = new SelectList(groups, nameof(AcademicGroupViewModel.Id), nameof(AcademicGroupViewModel.Name), selectedGroup);
        }
    }
}
