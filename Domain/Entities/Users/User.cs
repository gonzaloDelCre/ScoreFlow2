using Domain.Entities.Notifications;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.Users
{
    public class User
    {
        private int userID;
        private string fullName;
        private string email;

        public UserID UserID { get; private set; }
        public UserFullName FullName { get; private set; }
        public UserEmail Email { get; private set; }
        public UserPasswordHash PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public UserRole Role { get; set; } = UserRole.Espectador;

        public ICollection<Notification> Notifications { get; private set; }

        //User Constructor Basic
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

        //Update Method
        public void Update(UserFullName fullName, UserEmail email, UserPasswordHash passwordHash)
        {
            FullName = fullName ?? FullName;
            Email = new UserEmail(email.Value);
            PasswordHash = passwordHash ?? PasswordHash;
        }

        public User() => Notifications = new List<Notification>();

        //Create User Register
        public User(string fullName, string email, string passwordHash, UserRole role)
        {
            FullName = new UserFullName(fullName);
            Email = new UserEmail(email);
            PasswordHash = new UserPasswordHash(passwordHash);
            Role = role;
            CreatedAt = DateTime.UtcNow;
            Notifications = new List<Notification>();
        }

    }
}
