using Domain.Entities.Notifications;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users
{
    public class User
    {
        public UserID UserID { get; private set; }
        public UserFullName FullName { get; private set; }
        public UserEmail Email { get; private set; }
        public UserPasswordHash PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<Notification> Notifications { get; private set; }

        public User(UserID userID, UserFullName fullName, UserEmail email, UserPasswordHash passwordHash, DateTime createdAt)
        {
            UserID = userID;
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = createdAt;
            Notifications = new List<Notification>();
        }

        public User() => Notifications = new List<Notification>();

    }
}
