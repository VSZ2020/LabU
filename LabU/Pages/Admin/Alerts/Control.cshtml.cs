using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Alerts;

public class ControlModel : PageModel
{
    public ControlModel(IConfiguration config)
    {
        _config = config;
    }

    readonly IConfiguration _config;

    [Display(Name = "Информационное сообщение")]
    [BindProperty]
    public string? InformationMessage { get; set; }
    
    
    public void OnGet()
    {
        if (_config["InfoMessage"] is string msg && !string.IsNullOrEmpty(msg))
        {
            InformationMessage = msg;
        }
    }


    public  IActionResult OnPost()
    {

        if (ModelState.IsValid)
        {
            ViewData["InfoMessage"] = InformationMessage;
            _config["InfoMessage"] = InformationMessage;
        }
        return Page();
    }
}