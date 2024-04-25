using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LabU.Pages.Student
{
    public class SubjectsModel : PageModel
    {
        public SubjectsModel(ISubjectRepository sr)
        {
            this.subjectService = sr;
            Subjects = new();
        }

        private readonly ISubjectRepository subjectService;

        public List<SubjectViewModel> Subjects { get; private set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if (User is null || User.Identity == null|| !User.Identity.IsAuthenticated)
                return Unauthorized();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0)
                return Unauthorized();

            var subjects = await subjectService.GetSubjectsAsync(userId);
            Subjects.AddRange(subjects.Select(s => SubjectMapper.Map(s)).ToList());

            return Page();
        }
    }
}
