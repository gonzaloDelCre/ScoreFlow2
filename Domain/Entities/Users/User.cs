using Domain.Entities.Notifications;
using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Domain.Entities.Users
{
    public class User
    {
        public UserID UserID { get; private set; }
        public UserFullName FullName { get; private set; }
        public UserEmail Email { get; private set; }
        public UserPasswordHash PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public UserRole Role { get; set; } = UserRole.Espectador;

        public ICollection<Notification> Notifications { get; private set; }

        public User(UserID userID, UserFullName fullName, UserEmail email, UserPasswordHash passwordHash, UserRole role, DateTime createdAt)
        {
            UserID = userID;
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            CreatedAt = createdAt;
            Notifications = new List<Notification>();
        }

        public User() => Notifications = new List<Notification>();

        public void UpdateFullName(string fullName)
        {
            FullName = new UserFullName(fullName);
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            PasswordHash = new UserPasswordHash(passwordHash);
        }
    }
}
