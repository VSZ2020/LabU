using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Users
{
    public class EditRolesModel : PageModel
    {
        public EditRolesModel(UnitOfWork uow, ILogger<EditRolesModel> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        readonly UnitOfWork _uow;
        readonly ILogger<EditRolesModel> _logger;

        [BindProperty]
        public List<UserRoleViewModel> Roles { get; private set; } = new();

        [BindProperty]
        public string Username { get; private set; }

        [BindProperty]
        public int UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound("Пустой идентификатор пользователя");

            var user = await _uow.UserService.FindByIdAsync(id.Value);
            if (user == null)
                return RedirectToPage("./Index");

            Username = user.Username ?? "";
            UserId = user.Id;

            var allRoles = await _uow.RoleService.GetRoles();
            Roles = allRoles.Select(r => RolesMapper.Map(r, user.Roles.Any(u => u.Id == r.Id))).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newUserRoles = Roles.Where(r => r.IsChecked).Select(r => r.Id).ToArray();

            try
            {
                await _uow.UserService.UpdateUserRolesAsync(UserId, newUserRoles);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Can't update roles for user {UserId}. Exception: {ex.Message}");
                RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}
