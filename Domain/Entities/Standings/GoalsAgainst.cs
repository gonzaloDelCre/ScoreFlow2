using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Standings
{
    public sealed class GoalsAgainst
    {
        public int Value { get; }

        public GoalsAgainst(int value)
        {
            if (value < 0)
                throw new ArgumentException("Goals Against cannot be negative.", nameof(value));

            Value = value;
        }

        public static implicit operator int(GoalsAgainst goalsAgainst) => goalsAgainst.Value;
        public static explicit operator GoalsAgainst(int value) => new GoalsAgainst(value);

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => obj is GoalsAgainst other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
