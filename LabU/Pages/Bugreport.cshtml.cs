using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace LabU.Pages
{
    public class BugreportModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string? Description { get; set; }

        [BindProperty]
        public int SenderId { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return RedirectToPage(returnUrl);

            return RedirectToPage("./Index");
        }
    }
}
