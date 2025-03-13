using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class UserID
    {
        public int Value { get; private set; }

        public UserID(int value)
        {
            Value = value;
        }
    }
}
