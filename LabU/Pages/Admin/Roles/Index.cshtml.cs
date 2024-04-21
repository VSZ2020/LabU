using LabU.Core.Identity;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Roles
{
    public class IndexModel : PageModel
    {
        public IndexModel(UnitOfWork uow, ILogger<IndexModel> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        readonly UnitOfWork _uow;
        readonly ILogger<IndexModel> _logger;

        public List<UserRoleViewModel> Roles { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            var roles = (await _uow.RoleService.GetRoles()).Select(r => RolesMapper.Map(r)).ToList();
            Roles = roles;

            return Page();
        }
    }
}
