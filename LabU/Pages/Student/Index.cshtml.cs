using LabU.Core.Dto;
using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;

namespace LabU.Pages.Student
{
    public class IndexModel : PageModel
    {
        public IndexModel(ITaskService taskService)
        {
            this._taskService = taskService;
        }

        private readonly ITaskService _taskService;

        public List<TaskDto> ActiveTasks { get; private set; } = new();

        public List<TaskDto> CompletedTasks { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var allUserTasks = await _taskService.GetAllAsync(0, userId);

            var activeTasks = allUserTasks.Where(t => t.Status != Core.ResponseState.Accepted).ToList();
            var completedTasks = allUserTasks.Where(t => t.Status == Core.ResponseState.Accepted).ToList();

            ActiveTasks.AddRange(activeTasks);
            CompletedTasks.AddRange(completedTasks);

            return Page();
        }
    }
}
