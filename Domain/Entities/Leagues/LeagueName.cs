using System.Text.RegularExpressions;

namespace Domain.Entities.Leagues
{
    public class LeagueName
    {
        private static readonly Regex ValidNameRegex = new Regex(@"^[a-zA-Z0-9\s]+$", RegexOptions.Compiled);

        public string Value { get; private set; }

        public LeagueName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre de la liga no puede estar vacío o solo contener espacios.", nameof(value));

            value = value.Trim();

            if (value.Length > 100)
                throw new ArgumentException("El nombre de la liga no puede superar los 100 caracteres.", nameof(value));

            

            Value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is LeagueName other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }
    }
}