using LabU.Core.Interfaces;
using LabU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabU.Pages.Student.Task.Response
{
    public class EditModel : PageModel
    {
        public EditModel(ITaskRepository tr, ITaskResponseService trr, ILogger<EditModel> logger)
        {
            _tr = tr;
            _trr = trr;
        }

        private readonly ITaskRepository _tr;
        private readonly ITaskResponseService _trr;
        private ILogger<EditModel> _logger;

        [BindProperty]
        public TaskResponseViewModel TaskResponse { get; set; } = default!;

    }
}
