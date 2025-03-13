using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class UserPasswordHash
    {
        public string Value { get; private set; }

        public UserPasswordHash(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("La contraseña no puede estar vacía.");
            Value = value;
        }
    }
}