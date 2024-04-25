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
    public class EditModel : PageModel
    {
        public EditModel(IPersonRepository pr, IUserService us, IRoleService rs, IAcademicGroupsRepository agr, ILogger<EditModel> logger)
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
        readonly ILogger<EditModel> _logger;

        [BindProperty]
        public EditStudentViewModel Student { get; set; }

        public SelectList AcademicGroups { get; set; }

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

            var entity = await _pr.FindStudentByIdAsync(id.Value);
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

                Course = entity.Course,
                AcademicGroupId = entity.AcademicGroupId,
                CommandId = entity.CommandId,
            };

            PopulateGroupsList(Student.AcademicGroupId);

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

            var entityStudent = await _pr.FindStudentByIdAsync(id.Value);
            var entityAccount = await _us.FindByIdAsync(id.Value);
            if (entityStudent == null || entityAccount == null)
            {
                ModelState.AddModelError("", "Сущность пользователя потеряна. Не удалось сохранить");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                PopulateGroupsList(entityStudent.AcademicGroupId);
                return Page();
            }


            entityAccount.Username = Student.Username;
            entityAccount.Email = Student.Email;
            entityAccount.IsEmailConfirmed = Student.IsEmailConfirmed;
            entityAccount.IsActiveAccount = Student.IsActiveAccount;

            entityStudent.LastName = Student.LastName;
            entityStudent.FirstName = Student.FirstName;
            entityStudent.MiddleName = Student.MiddleName;

            entityStudent.Course = Student.Course;
            entityStudent.CommandId = Student.CommandId;
            entityStudent.AcademicGroupId = Student.AcademicGroupId > 0 ? Student.AcademicGroupId : null;

            try
            {
                await _pr.EditStudentAsync(entityStudent);
                await _us.UpdateUserAsync(entityAccount);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Can't save changes for student with Id = {id.Value}. Exception: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "Не удалось сохранить изменения");
                return Page();
            }

            return RedirectToPage("./Index");
        }


        private async void PopulateGroupsList(object selectedGroup = null)
        {
            var groups = (await _agr.GetAllGroupsAsync()).Select(AcademicGroupMapper.Map).ToList();
            AcademicGroups = new SelectList(groups, nameof(AcademicGroupViewModel.Id), nameof(AcademicGroupViewModel.Name), selectedGroup);
        }
    }
}
