using LabU.Core.Entities;
using LabU.Data.Repository;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LabU.Pages.Student.Task.Response
{
    public class AddModel : PageModel
    {
        public AddModel(UnitOfWork uow, ILogger<AddModel> logger)
        {
            _uow = uow;
        }

        private readonly UnitOfWork _uow;
        private ILogger<AddModel> _logger;

        [BindProperty]
        public AddTaskResponseViewModel TaskResponse { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? taskId)
        {
            if (taskId == null)
                return RedirectToPage("./Index");

            var taskService = _uow.TasksService;
            var task = await taskService.FindByIdAsync(taskId.Value);

            if (task.Deadline.Value < DateTime.Now)
            {
                return BadRequest("Срок ответа на задание вышел!");
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
                _uow.ResponseService.AddResponse(response);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Attempt to add new response for task {TaskResponse.TaskId}. Exception: {ex.Message}");
                ModelState.AddModelError("", "Не удалось создать ответ на задание. Попробуйте позже");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
