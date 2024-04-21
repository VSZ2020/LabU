using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        public IndexModel(UnitOfWork uow)
        {
            _uow = uow;
        }

        private UnitOfWork _uow;

        public List<UserViewModel> Users { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            //Дополнительный слой защиты от неавторизованного доступа
            if (!User.IsInRole(UserRoles.ADMINISTRATOR))
            {
                return Unauthorized();
            }

            var users = (await _uow.UserService.GetAllUsers()).ToList();
            var persons = (await _uow.UserService.GetAllPersons()).ToList();
            Users = Enumerable.Range(0, users.Count).Select(i => UsersMapper.Map(users[i], persons.FirstOrDefault(p => p.Id == users[i].Id))).ToList();

            return Page();
        }
    }
}
