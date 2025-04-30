using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Standings
{
    public sealed class GoalsFor
    {
        public int Value { get; }

        public GoalsFor(int value)
        {
            if (value < 0)
                throw new ArgumentException("Goals For cannot be negative.", nameof(value));

            Value = value;
        }

        public static implicit operator int(GoalsFor goalsFor) => goalsFor.Value;
        public static explicit operator GoalsFor(int value) => new GoalsFor(value);

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => obj is GoalsFor other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
