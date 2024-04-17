using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using LabU.Core.Entities;
using LabU.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    [Required(ErrorMessage = "Не указан логин")]
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
    
    public async Task<IActionResult> OnPostAsync()
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
        if (await _authService.AuthUserAsync(Username!, Password!) == null)
        {
            ModelState.AddModelError("","Неверный пароль для входа");
            return Page();
        }
        
        await AuthenticateAsync(user);
        
        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            return Redirect(ReturnUrl ?? "./Index");
        return Redirect("./Index");
    }

    private async Task AuthenticateAsync(UserEntity user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username!),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
        };
        var claimsIdentity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await HttpContext.SignInAsync(claimsPrincipal);
    }
}