using LabU.Core.Entities;
using LabU.Core.Interfaces;
using LabU.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace LabU.Pages.Account
{
    public class IndexModel : PageModel
    {
        public IndexModel(IUserService us, IPersonRepository pr, IAuthService aServ)
        {
            _us = us;
            _pr = pr;
            _as = aServ;
        }

        readonly IAuthService _as;
        readonly IUserService _us;
        readonly IPersonRepository _pr;

        [BindProperty]
        public EditUserViewModel CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0)
                return RedirectToPage("/Account/Login");

            var user = await _us.FindByIdAsync(userId);
            if (user == null)
                return RedirectToPage("/Account/Login");

            BasePersonEntity? person = await _pr.FindTeacherByIdAsync(userId);
            if (person == null)
                person = await _pr.FindStudentByIdAsync(userId);


            CurrentUser = new EditUserViewModel()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                LastName = person.LastName,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0)
                return RedirectToPage("/Account/Login");

            var user = await _us.FindByIdAsync(userId);
            if (user == null)
                return RedirectToPage("/Account/Login");

            if (!string.IsNullOrEmpty(CurrentUser.OldPassword) && string.IsNullOrEmpty(CurrentUser.Password))
            {
                ModelState.AddModelError("", "Не задан новый пароль");
            }

            if (string.IsNullOrEmpty(CurrentUser.OldPassword) && !string.IsNullOrEmpty(CurrentUser.Password))
            {
                ModelState.AddModelError("", "Введите старый пароль");
            }

            if (!ModelState.IsValid)
                return Page();

            user.Username = CurrentUser.Username;
            user.Email = CurrentUser.Email;

            if (!string.IsNullOrEmpty(CurrentUser.Password) && !string.IsNullOrEmpty(CurrentUser.OldPassword))
            {
                var changePasswordResult = await _as.ChangePasswordAsync(userId, CurrentUser.OldPassword, CurrentUser.Password);
                if (!changePasswordResult)
                {
                    ModelState.AddModelError("", "Не удалось изменить пароль пользователя. Неверно указан старый пароль");
                }
            }

            try
            {
                await _us.UpdateUserAsync(user);

                var person = await _pr.FindTeacherByIdAsync(userId); 
                if (person != null)
                {
                    person.LastName = CurrentUser.LastName;
                    person.FirstName = CurrentUser.FirstName;
                    person.MiddleName = CurrentUser.MiddleName;
                    await _pr.EditTeacherAsync(person);
                }
                else
                {
                    var student = await _pr.FindStudentByIdAsync(userId);

                    student.LastName = CurrentUser.LastName;
                    student.FirstName = CurrentUser.FirstName;
                    student.MiddleName = CurrentUser.MiddleName;
                    await _pr.EditStudentAsync(student);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Не удалось сохранить изменения. Обратитесь к администратору");
                return Page();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return RedirectToPage(returnUrl);

            ViewData["SaveStatus"] = "Успешно сохранено!";
            return Page();
        }
    }
}
