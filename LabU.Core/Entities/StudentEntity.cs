using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    [Table("students")]
    public class StudentEntity : BasePersonEntity
    {
        #region AcademicGroup
        public int? AcademicGroupId { get; set; }

        /// <summary>
        /// Академическая группа студента. Например, ФтМ-120302
        /// </summary>
        [ForeignKey(nameof(AcademicGroupId))]
        public AcademicGroupEntity? AcademicGroup { get; set; }
        #endregion

        /// <summary>
        /// Номер курса обучения
        /// </summary>
        public int Course { get; set; }

        /// <summary>
        /// Идентификатор команды/бригады
        /// </summary>
        public int CommandId { get; set; }
    }
}
