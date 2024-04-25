using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace LabU.Pages.Admin.Users
{
    public class ResetPasswordModel : PageModel
    {
        public ResetPasswordModel(IUserService us, IPersonRepository pr, IAuthService authService)
        {
            _pr = pr;
            _us = us;
            _as = authService;
        }

        readonly IPersonRepository _pr;
        readonly IUserService _us;
        readonly IAuthService _as;

        [BindProperty]
        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите новый пароль")]
        public string Password { get; set; }

        [BindProperty]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public int UserId { get; set; }

        public string Username { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (User == null || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (id == null)
                return NotFound();

            var user = await _us.FindByIdAsync(id.Value);
            if (user == null)
                return NotFound();

            UserId = id.Value;

            BasePersonEntity? person = await _pr.FindStudentByIdAsync(id.Value);
            if (person == null)
                person = await _pr.FindTeacherByIdAsync(id.Value);
            Username = person?.ToFullName() ?? "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (User == null || !User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            if (id == null)
                return NotFound();

            var user = await _us.FindByIdAsync(id.Value);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                var successReset = await _as.ChangePasswordAsync(id.Value, Password);
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError("", "Не удалось изменить пароль. Возникла ошибка записи в базу");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
