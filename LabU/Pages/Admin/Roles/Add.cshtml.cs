using LabU.Core.Entities.Identity;
using LabU.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LabU.Pages.Admin.Roles
{
    public class AddModel : PageModel
    {
        public AddModel(UnitOfWork uow, ILogger<AddModel> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        private readonly UnitOfWork _uow;
        readonly ILogger<AddModel> _logger;

        [Required]
        [StringLength(100, ErrorMessage = "Длина названия не должна превышать 100 символов")]
        [BindProperty]
        public string? Name { get; set; }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var availableRoles = await _uow.RoleService.GetRoles();
            if (availableRoles.Any(r => r.Name == Name))
            {
                ModelState.AddModelError("", "Роль с таким названием уже существует");
                return Page();
            }

            try
            {
                await _uow.RoleService.CreateAsync(new RoleEntity()
                {
                    Name = Name,
                    NormalizedName = Name.ToUpper()
                });
                await _uow.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"An error occcured, while saving a new role with name {Name}");
            }

            return RedirectToPage("./Index");
        }
    }
}
