using LabU.Core.Entities;
using LabU.Core.Interfaces;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LabU.Pages.Student.Task.Response
{
    public class AddModel : PageModel
    {
        public AddModel(ITaskRepository tr, ITaskResponseService trr, ILogger<AddModel> logger)
        {
            _tr = tr;
            _trr = trr;
        }

        private readonly ITaskRepository _tr;
        private readonly ITaskResponseService _trr;
        private ILogger<AddModel> _logger;

        [BindProperty]
        public AddTaskResponseViewModel TaskResponse { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? taskId)
        {
            if (taskId == null)
                return RedirectToPage("./Index");

            var taskService = _tr;
            var task = await taskService.FindByIdAsync(taskId.Value);

            if (task == null)
                return NotFound("������ ������� �� ����������");

            if (task.Status == Core.ResponseState.Accepted)
            {
                return BadRequest("���� ������� ��� ������� � �� ������� ������");
            }

            if (task.Deadline != null && task.Deadline.Value < DateTime.Now)
            {
                return BadRequest("���� ������ �� ������� �����!");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (!taskService.IsOwnedByUser(taskId.Value, userId))
            {
                _logger.LogInformation($"Attempt to add task response by other user {userId}");
                return RedirectToPage("./Index");
            }

            TaskResponse = new AddTaskResponseViewModel()
            {
                TaskId = taskId.Value,
                Comment = "",
                AttachmentName = "",
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (TaskResponse == null || TaskResponse.TaskId == 0)
            {
                return RedirectToPage("./Index");
            }

            var task = await _tr.FindByIdAsync(TaskResponse.TaskId);
            if (task == null)
                return NotFound("������ ������� �� ����������");

            if (task.Status == Core.ResponseState.Accepted)
            {
                return BadRequest("���� ������� ��� ������� � �� ������� ������");
            }

            if (task.Deadline != null && task.Deadline.Value < DateTime.Now)
            {
                return BadRequest("���� ������ �� ������� �����!");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0)
                return RedirectToPage("./Index");

            var response = new TaskResponseEntity()
            {
                SenderId = userId,
                TaskId = TaskResponse.TaskId,
                SubmissionDate = DateTime.Now,
                Comment = TaskResponse.Comment,
            };

            try
            {
                await _trr.AddResponseAsync(response);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Attempt to add new response for task {TaskResponse.TaskId}. Exception: {ex.Message}");
                ModelState.AddModelError("", "�� ������� ������� ����� �� �������. ���������� �����");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
