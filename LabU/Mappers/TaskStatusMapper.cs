using LabU.Core;

namespace LabU.Mappers
{
    public class TaskStatusMapper
    {
        public static string Map(ResponseState state)
        {
            return state switch
            {
                ResponseState.WaitForRevision => "Ожидает проверки",
                ResponseState.UnderReview => "Проверяется",
                ResponseState.ReturnedBack => "Возвращен на доработку",
                ResponseState.Accepted => "Принят",
                _ => "-"
            };
        }
    }
}
