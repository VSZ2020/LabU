using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Teachers
{
    public class IndexModel : PageModel
    {
        public IndexModel(UnitOfWork uow)
        {
            _uow = uow;
        }

        private UnitOfWork _uow;

        public List<TeacherViewModel> Teachers { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            var teachers = await _uow.PersonService.GetTeachers();
            Teachers = teachers.Select(t => TeacherMapper.Map(t)).ToList();

            return Page();
        }
    }
}
