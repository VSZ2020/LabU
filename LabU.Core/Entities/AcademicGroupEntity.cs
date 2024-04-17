using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    /// <summary>
    /// Академическая группа
    /// </summary>
    [Table("academic_groups")]
    public class AcademicGroupEntity : BaseEntity
    {
        /// <summary>
        /// Полное название академической группы. Пример: Фт-120302
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Перечень студентов, состоящих в группе
        /// </summary>
        public List<StudentEntity> Students { get; set; }
    }
}
