using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace LabU.Pages.Account;

public class Login : PageModel
{
    public Login(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    
    [BindProperty]
    [Required(ErrorMessage = "Введите логин")]
    [Display(Name = "Логин")]
    public string? Username { get; set; }
    
    [BindProperty]
    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string? Password { get; set; }
    
    [BindProperty]
    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }

    [BindProperty]
    public string? ReturnUrl { get; set; }
    
    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        UserEntity? user = await _userService.FindByUsernameAsync(Username!);
        if (user == null)
        {
            ModelState.AddModelError("","Пользователя с данным логином не существует");
            return Page();
        }
        if (await _authService.TryAuthUserAsync(Username!, Password!))
        {
            await AuthenticateAsync(user);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl ?? "/Index");
            return Redirect("/Index");
        }
        else
            ModelState.AddModelError("", "Неверный пароль");
        return Page();
    }

    private async Task AuthenticateAsync(UserEntity user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username!),
         
            new Claim(ClaimTypes.Email, user.Email ?? ""),
        };
        if (user.Roles != null)
        {
            foreach(var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));
            }
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await HttpContext.SignInAsync(claimsPrincipal);

        user.LastVisit = DateTime.Now;
        await _userService.UpdateUserAsync(user);
    }
}