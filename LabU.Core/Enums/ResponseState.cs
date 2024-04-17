namespace LabU.Core
{
    /// <summary>
    /// Состояния ответа на задание
    /// </summary>
    public enum ResponseState
    {
        /// <summary>
        /// Не определен
        /// </summary>
        Undefined = -1,

        /// <summary>
        /// Ожидает проверки преподавателя
        /// </summary>
        WaitForRevision = 0,

        /// <summary>
        /// На проверке
        /// </summary>
        UnderReview,
        
        /// <summary>
        /// Возвращен на доработку
        /// </summary>
        ReturnedBack,

        /// <summary>
        /// Принят
        /// </summary>
        Accepted,
    }
}
