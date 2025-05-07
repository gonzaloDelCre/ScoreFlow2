using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TeamPlayers
{
    public class JoinedAt
    {
        public DateTime Value { get; private set; }

        public JoinedAt(DateTime value)
        {
            if (value == default)
                throw new ArgumentException("JoinedAt cannot be default");
            Value = value;
        }

        public static implicit operator DateTime(JoinedAt joinedAt) => joinedAt.Value;
        public static implicit operator JoinedAt(DateTime value) => new JoinedAt(value);
    }
}
