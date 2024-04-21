using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Roles
{
    public class RemoveModel : PageModel
    {
        public RemoveModel(UnitOfWork uow, ILogger<RemoveModel> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        readonly UnitOfWork _uow;
        readonly ILogger<RemoveModel> _logger;

        [BindProperty]
        public int RoleId { get; set; }

        [BindProperty]
        public string? RoleName { get; private set; }

        [BindProperty]
        public List<RemoveUserViewModel> UsersToUpdate { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }
            if (id == null)
            {
                return NotFound("Идентификатор роли отсутствует");
            }

            var role = await _uow.RoleService.FindByIdAsync(id.Value);
            if (role == null)
                return Redirect("./Index");

            RoleName = role.Name!;

            var usersToUpdate = await _uow.UserService.GetUsersAsync(
                filter: u => u.Roles!.Any(r => r.Id == id.Value) && u.Roles!.Count == 1,
                includeProps: string.Join(",", nameof(UserEntity.Roles))
                );
            UsersToUpdate = usersToUpdate!.Select(u => new RemoveUserViewModel() { Id = u.Id, Username = u.Username }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(UsersToUpdate != null)
            {
                var ids = UsersToUpdate.Select(u => u.Id).ToArray();
                var guestRole = await _uow.RoleService.FindByNameAsync(UserRoles.GUEST);

                await _uow.UserService.AddToRoleAsync(ids, guestRole!.Id);
                await _uow.UserService.RemoveFromRoleAsync(ids, RoleId);

                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Can't save changes after removing roles from users with ids: {string.Join(",",ids)}. Exception: {ex.Message}");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
