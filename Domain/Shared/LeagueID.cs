using System;
using System.Collections.Generic;
using System.Linq;
namespace Domain.Shared
{
    public class LeagueID
    {
        public int Value { get; private set; }

        public LeagueID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID de la liga debe ser mayor que 0.");
            Value = value;
        }
    }
}
