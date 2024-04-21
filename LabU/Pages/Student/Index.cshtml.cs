using LabU.Core.Entities;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LabU.Pages.Student
{
    public class IndexModel : PageModel
    {
        public IndexModel(UnitOfWork uof)
        {
            this._taskService = uof.TasksService;
        }

        private readonly ITaskRepository _taskService;

        public List<TaskViewModel> ActiveTasks { get; private set; } = new();

        public List<TaskViewModel> CompletedTasks { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0)
                return Unauthorized();

            var includeProps = string.Join(",",
                nameof(TaskEntity.Subject),
                nameof(TaskEntity.Users));

            var allUserTasks = await _taskService.GetAllAsync(
                filter: t => t.Users!.Any(u => u.Id == userId) && t.IsAvailable, 
                includeProps: includeProps);

            var activeTasks = allUserTasks.Where(t => t.Status != Core.ResponseState.Accepted).Select(t => Mappers.TaskMapper.Map(t, false, true)).ToList();
            var completedTasks = allUserTasks.Where(t => t.Status == Core.ResponseState.Accepted).Select(t => Mappers.TaskMapper.Map(t, false, true)).ToList();

            ActiveTasks.AddRange(activeTasks);
            CompletedTasks.AddRange(completedTasks);

            return Page();
        }
    }
}
