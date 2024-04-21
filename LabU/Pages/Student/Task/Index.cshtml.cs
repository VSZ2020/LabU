using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using LabU.Data.Repository;
using LabU.Mappers;
using LabU.Models;
using LabU.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LabU.Pages.Student.Task
{
    public class IndexModel : PageModel
    {
        public IndexModel(UnitOfWork uow)
        {
            _taskService = uow.TasksService;
            _taskResponseService = uow.ResponseService;
        }
        private readonly ITaskRepository _taskService;
        private readonly ITaskResponseService _taskResponseService;

        public TaskViewModel? StudentTask { get; private set; }
        public List<TaskResponseViewModel> PreviousTaskResponses { get; private set; } = new();


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest("Не указан идентификатор задания");
            }

            if (User is null || User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            if (!User.IsInRole(UserRoles.ADMINISTRATOR) && !_taskService.IsOwnedByUser(id.Value, userId))
                return BadRequest("Вы не имеете доступа к данному заданию");

            var propsToInclude = string.Join(",",
                nameof(TaskEntity.Subject),
                nameof(TaskEntity.Responses),
                nameof(TaskEntity.Users));

            var task = await _taskService.FindByIdAsync(id.Value, propsToInclude);
            if (task is null)
                return NotFound("Такого задания не существует");

            StudentTask = TaskMapper.Map(task, isFullStudentsName: false, isFullTeachersName: true, ",");

            var taskResponses = await _taskResponseService.GetAllAsync(id.Value);
            PreviousTaskResponses = taskResponses.Select(tr => TaskResponseMapper.Map(tr)).ToList();

            return Page();
        }
    }
}
