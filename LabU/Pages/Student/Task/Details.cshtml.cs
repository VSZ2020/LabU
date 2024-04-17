using LabU.Core.Dto;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using LabU.Core.Entities;
using LabU.Models;
using LabU.Utils.Extensions;

namespace LabU.Pages.Student.Task
{
    public class DetailsModel : PageModel
    {
        public DetailsModel(ITaskService taskService, ITaskResponseService taskRespService)
        {
            _taskService = taskService;
            _taskResponseService = taskRespService;
        }
        
        private readonly ITaskService _taskService;
        private readonly ITaskResponseService _taskResponseService;

        public TaskViewModel? StudentTask { get; private set; }
        public List<TaskResponseViewModel> PreviousTaskResponses { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (User is null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (!User.IsInRole(UserRoles.ADMINISTRATOR) && !_taskService.IsOwnedByUser(id.Value, userId))
                return Unauthorized();

            var task = await _taskService.FindByIdAsync(id.Value);
            if (task is null)
                return BadRequest("Такого задания не существует");
            
            var attachedStudents = await _taskService.GetAttachedStudentsAsync(id.Value);
            var attachedTeachers = await _taskService.GetAttachedReviewersAsync(id.Value);
            StudentTask = new TaskViewModel()
            {
                Id = task!.Id,
                Name = task.Name,
                Description = task.Description,
                Deadline = task.Deadline, Status = task.Status, 
                IsAvailable = task.IsAvailable,
                Subject = task.Subject?.Name ?? "-",
                Students = string.Join("\n", attachedStudents.ToShortNames()),
                Reviewers = string.Join("\n", attachedTeachers.ToShortNames()),
            };

            var taskResponses = await _taskResponseService.GetAllAsync(id.Value);
            PreviousTaskResponses = taskResponses
                .Select(tr => new TaskResponseViewModel()
                {
                    Id = tr.Id,
                    TaskId = tr.TaskId,
                    SenderId = tr.SenderId, 
                    SubmissionDate = tr.SubmissionDate, 
                    SenderName = tr.Sender?.ToShortName() ?? "-", 
                    Comment = tr.Comment, 
                    AttachmentName = tr.Attachment?.FileName ?? "-"
                }).ToList();
            return Page();
        }
    }
}
