using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    public abstract class BasePersonEntity
    {
        [Key]
        [ForeignKey(nameof(Account))]
        public int Id { get; set; }

        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }

        public string? Email { get; set; }

        public UserEntity? Account { get; set; }

        /// <summary>
        /// Перечень прикрепленных заданий
        /// </summary>
        public List<TaskEntity>? Tasks { get; set; }
        
        public List<TaskPersonTable>? TaskPersonTable { get; set; }
        
        public List<SubjectEntity>? Subjects { get; set; }
        public List<PersonSubjectTable>? PersonSubjectTable { get; set; }
        
        
    }
}
