using LabU.Core.Interfaces;
using LabU.Mappers;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LabU.Pages.Teacher.Task
{
    public class DetailsModel : PageModel
    {
        public DetailsModel(ITaskRepository tr, ITaskResponseService trr, ILogger<DetailsModel> logger)
        {
            _tr = tr;
            _trr = trr;
            _logger = logger;
        }

        readonly ITaskRepository _tr;
        readonly ITaskResponseService _trr;
        readonly ILogger<DetailsModel> _logger;

        public TaskViewModel StudentTask { get; set; }
        public List<TaskResponseViewModel> TaskResponses { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
                return NotFound("Задание не найдено");

            var task = await _tr.FindByIdAsync(id.Value);
            if (task == null)
                return NotFound("Задание не найдено");

            StudentTask = TaskMapper.Map(task);

            var responses = (await _trr.GetTaskResponsesAsync(id.Value)).OrderBy(tr => tr.SubmissionDate);
            TaskResponses = responses.Select(tr => TaskResponseMapper.Map(tr)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return BadRequest("Не указан идентификатор задания для принятия");

            var task = await _tr.FindByIdAsync(id.Value);
            if (task == null)
                return NotFound("Задание не найдено");

            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _tr.ChangeTaskStatus(id.Value, Core.ResponseState.Accepted);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"Can't change status of task {id.Value} to status 'Accepted'. Exception: {ex.Message}" + (ex.InnerException != null ? " " + ex.InnerException.Message : ""));
                ModelState.AddModelError("", "Не удалось принять задание. Попробуйте позже");
                return Page();
            }

            return RedirectToPage("");
        }
    }
}
