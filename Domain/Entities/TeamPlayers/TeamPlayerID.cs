using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TeamPlayers
{
    public class TeamPlayerID
    {
        public int Value { get; private set; }

        public TeamPlayerID(int value)
        {
            if (value < 0)
                throw new ArgumentException("TeamPlayer ID must be >= 0");
            Value = value;
        }

        public static implicit operator int(TeamPlayerID id) => id.Value;
        public static implicit operator TeamPlayerID(int value) => new TeamPlayerID(value);
    }
}
