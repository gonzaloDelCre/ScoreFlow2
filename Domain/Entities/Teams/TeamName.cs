namespace Domain.Entities.Teams
{
    public class TeamName
    {
        public string Value { get; private set; }

        public TeamName(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El nombre del equipo no puede estar vacío.");
            if (value.Length > 100)
                throw new ArgumentException("El nombre del equipo es demasiado largo.");
            Value = value;
        }
    }
}
