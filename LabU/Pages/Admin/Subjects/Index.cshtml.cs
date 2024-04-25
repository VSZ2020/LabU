using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Emit;

namespace LabU.Pages.Admin.Subjects
{
    public class IndexModel : PageModel
    {
        public IndexModel(ISubjectRepository sr, ILogger<IndexModel> logger)
        {
            _sr = sr;
            _logger = logger;
        }

        readonly ISubjectRepository _sr;
        readonly ILogger<IndexModel> _logger;

        public List<SubjectViewModel> Subjects { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            var subjects = await _sr.GetSubjectsAsync();
            Subjects = subjects.Select(s => SubjectMapper.Map(s)).ToList();

            return Page();
        }
    }
}
