using System;
using System.Collections.Generic;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Entities.Users
{
    public class User
    {
        public UserID UserID { get; private set; }
        public UserFullName FullName { get; private set; }
        public UserEmail Email { get; private set; }
        public UserPasswordHash PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected User() { }

        public User(
            UserID userID,
            UserFullName fullName,
            UserEmail email,
            UserPasswordHash passwordHash,
            UserRole role,
            DateTime createdAt)
        {
            UserID = userID ?? throw new ArgumentNullException(nameof(userID));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Role = role;
            CreatedAt = createdAt;
        }

        public static User Create(
            string fullName,
            string email,
            string passwordHash,
            UserRole role)
        {
            return new User(
                new UserID(0),
                new UserFullName(fullName),
                new UserEmail(email),
                new UserPasswordHash(passwordHash),
                role,
                DateTime.UtcNow
            );
        }

        public void Update(
            UserFullName? fullName = null,
            UserEmail? email = null,
            UserPasswordHash? passwordHash = null)
        {
            if (fullName != null) FullName = fullName;
            if (email != null) Email = email;
            if (passwordHash != null) PasswordHash = passwordHash;
        }

        public void UpdateRole(UserRole newRole)
        {
            Role = newRole;
        }
    }
}
