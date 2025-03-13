namespace Domain.Entities.Referees
{
    public class RefereeName
    {
        public string Value { get; private set; }

        public RefereeName(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El nombre del árbitro no puede estar vacío.");
            if (value.Length > 100)
                throw new ArgumentException("El nombre del árbitro es demasiado largo.");
            Value = value;
        }
    }
}