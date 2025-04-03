using System; // Asegúrate de tener esta línea
using Domain.Entities.Notifications;
using Domain.Enum;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

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

        public void UpdateFullName(UserFullName fullName)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName), "El nombre completo no puede ser nulo.");
        }

        public void UpdateEmail(UserEmail email)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email), "El correo electrónico no puede ser nulo.");
        }

        public void UpdatePassword(UserPasswordHash passwordHash)
        {
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash), "El hash de la contraseña no puede ser nulo.");
        }

        public void UpdateRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentNullException(nameof(role), "El rol no puede ser nulo o vacío.");

            // Usa Enum.Parse correctamente
            if (global::System.Enum.TryParse<UserRole>(role, true, out var parsedRole))
            {
                Role = parsedRole;
            }
            else
            {
                throw new ArgumentException("El valor proporcionado no es un rol válido.", nameof(role));
            }
        }

        public User() => Notifications = new List<Notification>();

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
