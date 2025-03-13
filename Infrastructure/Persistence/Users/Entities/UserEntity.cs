using Domain.Enum;
using Infrastructure.Persistence.Notifications.Entities;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Users.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Notification> Notifications { get; set; }
    }
}
