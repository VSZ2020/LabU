using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LabU.Pages.Student
{
    public class SubjectsModel : PageModel
    {
        public SubjectsModel(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
            Subjects = new();
        }

        private readonly ISubjectService subjectService;

        public List<SubjectDto> Subjects { get; }


        public async Task<IActionResult> OnGetAsync()
        {
            if (User is null || !User.Identity.IsAuthenticated || !User.IsInRole(UserRoles.ADMINISTRATOR) || !User.IsInRole(UserRoles.STUDENT))
                return Unauthorized();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var subjects = await subjectService.GetAllAsync(userId);
            Subjects.AddRange(subjects);

            return Page();
        }
    }
}
