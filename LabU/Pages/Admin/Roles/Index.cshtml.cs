using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Roles
{
    public class IndexModel : PageModel
    {
        public IndexModel(IRoleService rs, ILogger<IndexModel> logger)
        {
            _rs = rs;
            _logger = logger;
        }

        readonly IRoleService _rs;
        readonly ILogger<IndexModel> _logger;

        public List<UserRoleViewModel> Roles { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            var roles = (await _rs.GetRoles()).Select(r => RolesMapper.Map(r)).ToList();
            Roles = roles;

            return Page();
        }
    }
}
