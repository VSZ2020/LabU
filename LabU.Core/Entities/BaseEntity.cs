using System.ComponentModel.DataAnnotations;

namespace LabU.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
