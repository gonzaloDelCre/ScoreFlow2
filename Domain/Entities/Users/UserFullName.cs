using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class UserFullName
    {
        public string Value { get; private set; }

        public UserFullName(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El nombre completo no puede estar vacío.");
            if (value.Length > 100)
                throw new ArgumentException("El nombre completo es demasiado largo.");
            Value = value;
        }
    }
}
