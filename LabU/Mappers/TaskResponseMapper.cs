using LabU.Core.Entities;
using LabU.Models;
using LabU.Utils.Extensions;

namespace LabU.Mappers
{
    public class TaskResponseMapper
    {
        public static TaskResponseViewModel Map(TaskResponseEntity e)
        {
            var taskName = e.Task != null ? e.Task.Name : "-";
            var sender = e.Sender != null ? e.Sender.ToShortName() : "-";
            var attachmentName = e.Attachment != null ? e.Attachment.FileName : "-";

            return new TaskResponseViewModel
            {
                Id = e.Id,
                TaskId = e.TaskId,
                TaskName = taskName,
                SenderId = e.SenderId,
                SubmissionDate = e.SubmissionDate,
                SenderName = sender,
                Comment = e.Comment,
                AttachmentName = attachmentName,
            };
        }
    }
}
