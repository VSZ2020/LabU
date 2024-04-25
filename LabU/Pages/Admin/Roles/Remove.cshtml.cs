using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Roles
{
    public class RemoveModel : PageModel
    {
        public RemoveModel(IUserService us, IRoleService rs, ILogger<RemoveModel> logger)
        {
            _us = us;
            _rs = rs;
            _logger = logger;
        }

        readonly IUserService _us;
        readonly IRoleService _rs;
        readonly ILogger<RemoveModel> _logger;

        public int RoleId { get; set; }

        public string? RoleName { get; private set; }

        [BindProperty]
        public List<RemoveUserViewModel> UsersToUpdate { get; set; }

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

            var role = await _rs.FindByIdAsync(id.Value);
            if (role == null)
                return Redirect("./Index");

            RoleName = role.Name!;
            RoleId = id.Value;

            var usersToUpdate = await _us.GetUsersAsync(
                filter: u => u.Roles!.Any(r => r.Id == id.Value) && u.Roles!.Count == 1,
                includeProps: string.Join(",", nameof(UserEntity.Roles))
                );
            UsersToUpdate = usersToUpdate!.Select(u => new RemoveUserViewModel() { Id = u.Id, Username = u.Username }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            //TODO: Нет связи с формой
            if(UsersToUpdate != null && UsersToUpdate.Count > 0)
            {
                var ids = UsersToUpdate.Select(u => u.Id).ToArray();
                var guestRole = await _rs.FindByNameAsync(UserRoles.GUEST);

                try
                {
                    await _us.AddToRoleAsync(ids, guestRole!.Id);
                    await _us.RemoveFromRoleAsync(ids, id.Value);
                    await _rs.RemoveAsync(id.Value);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Can't remove role from users with ids: {string.Join(",",ids)}. Exception: {ex.Message}");
                    ModelState.AddModelError("", "Не удалось удалить роль. Попробуйте позднее");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
