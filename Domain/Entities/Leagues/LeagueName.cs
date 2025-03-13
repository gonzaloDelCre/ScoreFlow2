namespace Domain.Entities.Leagues
{
    public class LeagueName
    {
        public string Value { get; private set; }

        public LeagueName(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El nombre de la liga no puede estar vacío.");
            if (value.Length > 100)
                throw new ArgumentException("El nombre de la liga es demasiado largo.");
            Value = value;
        }
    }
}