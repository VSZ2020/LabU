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
        public IndexModel(ITaskRepository tr, IConfiguration config)
        {
            this._taskService = tr;
            _config = config;
        }

        private readonly ITaskRepository _taskService;
        readonly IConfiguration _config;

        public List<TaskViewModel> ActiveTasks { get; private set; } = new();

        public List<TaskViewModel> CompletedTasks { get; private set; } = new();

        public List<TaskViewModel> OverdueTasks { get; private set; } = new();

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

            var allUserTasks = (await _taskService.GetTasksAsync(
                filter: t => t.Users!.Any(u => u.Id == userId) && t.IsAvailable, 
                includeProps: includeProps))
                .Select(t => Mappers.TaskMapper.Map(t, false, false)).ToList();

            var activeTasks = allUserTasks.Where(t => t.Status != Core.ResponseState.Accepted && t.Deadline > DateTime.Now);
            var completedTasks = allUserTasks.Where(t => t.Status == Core.ResponseState.Accepted);
            var overdueTasks = allUserTasks.Where(t => t.Status != Core.ResponseState.Accepted && t.Deadline < DateTime.Now);

            ActiveTasks.AddRange(activeTasks);
            CompletedTasks.AddRange(completedTasks);
            OverdueTasks.AddRange(overdueTasks);

            if (_config["InfoMessage"] is string msg && !string.IsNullOrEmpty(msg))
            {
                ViewData["InfoMessage"] = msg;
            }

            return Page();
        }
    }
}
