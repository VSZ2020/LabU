using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LabU.Pages.Admin.Users
{
    public class EditRolesModel : PageModel
    {
        public EditRolesModel(IRoleService rs, IUserService us, ILogger<EditRolesModel> logger)
        {
            _rs = rs;
            _us = us;
            _logger = logger;
        }

        readonly IUserService _us;
        readonly IRoleService _rs;
        readonly ILogger<EditRolesModel> _logger;

        [BindProperty]
        public List<UserRoleViewModel> Roles { get; set; }

        public string Username { get; private set; }

        [BindProperty]
        public int UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (id == null)
                return NotFound("Пустой идентификатор пользователя");

            var user = await _us.FindByIdAsync(id.Value);
            if (user == null)
                return RedirectToPage("./Index");

            Username = user.Username ?? "";
            UserId = user.Id;

            var allRoles = await _rs.GetRoles();
            Roles = allRoles.Select(r => RolesMapper.Map(r, user.Roles.Any(u => u.Id == r.Id))).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] roles)
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Не удалось обновить список ролей пользователя");
                return Page();
            }

            if (roles != null)
            {
                var newUserRoles = roles.Select(r => int.Parse(r)).ToArray();

                try
                {
                    await _us.UpdateUserRolesAsync(UserId, newUserRoles);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError($"Can't update roles for user {UserId}. Exception: {ex.Message}");
                    ModelState.AddModelError("", "Не удалось обновить список ролей пользователя. Ошибка записи в базу данных");
                    RedirectToPage("./Index");
                }

            }
            return RedirectToPage("./Index");
        }
    }
}
