using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class UserEmail
    {
        public string Value { get; private set; }

        public UserEmail(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El correo electrónico no puede estar vacío.");
            if (!new EmailAddressAttribute().IsValid(value))
                throw new ArgumentException("El formato del correo electrónico no es válido.");
            Value = value;
        }
    }
}
