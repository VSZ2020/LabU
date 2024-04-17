using LabU.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    /// <summary>
    /// Дисциплина
    /// </summary>
    [Table("subjects")]
    public class SubjectEntity : BaseEntity
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
        public string AcademicYear { get; set; } = string.Empty;

        /// <summary>
        /// Тип учебного семестра - осенний или весенний
        /// </summary>
        public AcademicTerms AcademicTerm { get; set; }


        /// <summary>
        /// Перечень заданий, определенных в дисциплине
        /// </summary>
        public List<TaskEntity>? Tasks { get; set; }
        
        /// <summary>
        /// Прикрепленные преподаватели
        /// </summary>
        public List<BasePersonEntity>? Users { get; set; }
        
        
        public List<PersonSubjectTable>? PersonSubjectTable { get; set; }
    }
}
