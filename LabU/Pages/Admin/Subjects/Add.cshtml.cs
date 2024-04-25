using LabU.Core.Entities;
using LabU.Core.Interfaces;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Subjects
{
    public class AddModel : PageModel
    {
        public AddModel(ISubjectRepository sr)
        {
            _sr = sr;
        }

        readonly ISubjectRepository _sr;

        [BindProperty]
        public AddSubjectViewModel Subject { get; set; }

        public SelectList AcademicTerms { get; set; }

        public void OnGet()
        {
            var curYear = DateTime.Now.Year;
            var acadYear = DateTime.Now.Month < 7 ? $"{curYear}-{curYear + 1}" : $"{curYear - 1}-{curYear}";
            Subject = new()
            {
                Name = "Без названия",
                Description = "",
                AcademicTerm = DateTime.Now.Month < 7 ? "Весенний" : "Осенний",
                AcademicYear = acadYear,
            };
            PopulateTermsList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User == null || User.Identity == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var entity = new SubjectEntity()
                {
                    Name = Subject.Name,
                    Description = Subject.Description,
                    AcademicYear = Subject.AcademicYear,
                    AcademicTerm = Subject.AcademicTerm == "Осенний" ? Core.AcademicTerms.Autumn : Core.AcademicTerms.Spring
                };

                try
                {
                    await _sr.AddSubjectAsync(entity);
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Не удалось создать предмет. Попробуйте позже");
                    return Page();
                }

                return RedirectToPage("./Index");
            }

            PopulateTermsList();
            return Page();
        }

        private void PopulateTermsList()
        {
            AcademicTerms = new SelectList(new List<string>()
            {
                "Осенний",
                "Весенний",
            }, nameof(Subject.AcademicTerm));
        }
    }
}
