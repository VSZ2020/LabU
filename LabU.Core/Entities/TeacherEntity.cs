using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    [Table("teachers")]
    public class TeacherEntity : BasePersonEntity
    {
        /// <summary>
        /// Должность преподавателя
        /// </summary>
        public string? Function { get; set; }

        public string? Address { get; set; }
    }
}
