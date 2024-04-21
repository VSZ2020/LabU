using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities.Identity
{
    /// <summary>
    /// Учетная запись пользователя
    /// </summary>
    [Table("users")]
    public class UserEntity : BaseEntity
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? Username { get; set; }

        public string? PasswordHash { get; set; }

        /// <summary>
        /// Адрес электронной почты для связи и сброса пароля
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Флаг подтверждения адреса электронной почты
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Набор ролей пользователя
        /// </summary>
        public List<RoleEntity>? Roles { get; set; }

        /// <summary>
        /// Определяет доступность аккаунта
        /// </summary>
        public bool IsActiveAccount { get; set; }

        /// <summary>
        /// Время последнего посещения
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime LastVisit { get; set; }

        public string? SecurityStamp { get; set; }

        /// <summary>
        /// Число неудачных попыток входа
        /// </summary>
        public int AccessFiledCount { get; set; }

        public List<UserRoleTable> UserRoleTable { get; set; }
    }
}
