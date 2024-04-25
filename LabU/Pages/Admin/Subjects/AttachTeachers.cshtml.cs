using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Mappers;
using LabU.Models;
using LabU.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Admin.Subjects
{
    public class AttachTeachersModel : PageModel
    {
        public AttachTeachersModel(ISubjectRepository sr, IPersonRepository pr, IUserService us)
        {
            _sr = sr;
            _pr = pr;
            _us = us;
        }

        readonly ISubjectRepository _sr;
        readonly IPersonRepository _pr;
        readonly IUserService _us;

        [BindProperty]
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public List<AssignedUserViewModel> AssignedUsers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (!id.HasValue)
                return NotFound("Пустой идентификатор дисциплины");

            var subject = await _sr.FindSubjectByIdAsync(id.Value);
            if (subject == null)
                return NotFound();

            SubjectId = id.Value;
            SubjectName = subject.Name;

            await PopulateUsersListAsync(id.Value);
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? id, string[] teachers)
        {
            if (!User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (!id.HasValue)
                return NotFound("Пустой идентификатор дисциплины");

            if (ModelState.IsValid)
            {
                var teachersIds = teachers.Select(t => int.Parse(t)).ToArray();
                try
                {
                    await _sr.UpdateAttachedTeachersAsync(id.Value, teachersIds);
                }
                catch(DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Не удалось обновить прикрепленных пользователей. Попробуйте позже");
                    return Page();
                }
                return RedirectToPage("./Index");
            }

            await PopulateUsersListAsync(id.Value);
            return Page();
        }


        private async Task PopulateUsersListAsync(int subjectId)
        {
            var assignedUsersIds = (await _sr.GetAttachedTeachers(subjectId)).Select(t => t.Id).ToArray();
            var availableTeachers = (await _pr.GetTeachers()).Select(t => new AssignedUserViewModel()
            {
                Id = t.Id,
                Name = t.ToFullName(),
                IsAssigned = assignedUsersIds.Contains(t.Id),
            }).ToList();

            AssignedUsers = availableTeachers;
        }
    }
}
