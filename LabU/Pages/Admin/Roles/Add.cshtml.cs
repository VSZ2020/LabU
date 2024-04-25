using LabU.Core.Entities.Identity;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LabU.Pages.Admin.Roles
{
    public class AddModel : PageModel
    {
        public AddModel(IRoleService roleService, ILogger<AddModel> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        private readonly IRoleService _roleService;
        readonly ILogger<AddModel> _logger;

        [Required]
        [StringLength(100, ErrorMessage = "Длина названия не должна превышать 100 символов")]
        [BindProperty]
        public string? Name { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            Name = "Новая роль";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var availableRoles = await _roleService.GetRoles();
            if (availableRoles.Any(r => r.Name == Name))
            {
                ModelState.AddModelError("", "Роль с таким названием уже существует");
                return Page();
            }

            try
            {
                await _roleService.CreateAsync(new RoleEntity()
                {
                    Name = Name,
                    NormalizedName = Name.ToUpper()
                });
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"An error occcured, while saving a new role with name {Name}");
            }

            return RedirectToPage("./Index");
        }
    }
}
