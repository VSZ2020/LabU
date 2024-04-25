using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Students
{
    public class IndexModel : PageModel
    {
        public IndexModel(IPersonRepository pr)
        {
            _pr = pr;
        }

        readonly IPersonRepository _pr;

        public List<StudentViewModel> Students { get; private set; }

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

            var students = await _pr.GetStudents(includeProps: string.Join(",",nameof(StudentEntity.AcademicGroup)));
            Students = students.Select(t => StudentMapper.Map(t)).ToList();

            return Page();
        }
    }
}
