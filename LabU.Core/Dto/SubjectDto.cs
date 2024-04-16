namespace LabU.Core.Entities
{
    /// <summary>
    /// Дисциплина
    /// </summary>
    public class SubjectDto : BaseEntity
    {
        /// <summary>
        /// Название дисциплины
        /// </summary>
        public string Name { get; set; } = "Дисциплина без названия";

        /// <summary>
        /// Краткое описание дисциплины
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Учебный год в формате "yyyy-yyyy". Пример: 2023-2024.
        /// </summary>
        public string AcademicYear { get; set; }

        /// <summary>
        /// Тип учебного семестра - осенний или весенний
        /// </summary>
        public AcademicTerms AcademicTerm { get; set; }

        /// <summary>
        /// Прикрепленные преподаватели
        /// </summary>
        public string? Teachers { get; set; }

        /// <summary>
        /// Студенты, прикрепленные к данной дисциплине
        /// </summary>
        public string? Students { get; set; }
    }
}
