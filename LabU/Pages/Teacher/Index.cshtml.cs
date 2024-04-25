using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LabU.Pages.Teacher
{
    public class IndexModel : PageModel
    {
        public IndexModel(ITaskRepository tr, IUserService us, IConfiguration config)
        {
            _tr = tr;
            _us = us;
            _config = config;
        }

        readonly ITaskRepository _tr;
        readonly IUserService _us;
        readonly IConfiguration _config;

        public List<TaskViewModel> ActiveTasks { get; set; } = new();
        public List<TaskViewModel> CompletedTasks { get; set; } = new();
        public List<TaskViewModel> OverdueTasks { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0)
                return Unauthorized();

            var teacherTasks = await _tr.GetTasksAsync(
                filter: t => t.Users.Any(u => u.Id == userId) && t.IsAvailable,
                includeProps: string.Join(",", nameof(TaskEntity.Users), nameof(TaskEntity.Subject))
                );

            var mappedTasks = teacherTasks.Select(t => TaskMapper.Map(t)).ToList();

            var activeTasks = mappedTasks.Where(t => t.Status != Core.ResponseState.Accepted && t.Deadline > DateTime.Now).ToList();
            var completedTasks = mappedTasks.Where(t => t.Status == Core.ResponseState.Accepted).ToList();
            var overdueTasks = mappedTasks.Where(t => t.Status != Core.ResponseState.Accepted && t.Deadline < DateTime.Now).ToList();

            ActiveTasks = activeTasks;
            CompletedTasks = completedTasks;
            OverdueTasks = overdueTasks;

            if (_config["InfoMessage"] is string msg && !string.IsNullOrEmpty(msg))
            {
                ViewData["InfoMessage"] = msg;
            }

            return Page();
        }
    }
}
