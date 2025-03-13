namespace Domain.Entities.Standings
{
    public class StandingID
    {
        public int Value { get; private set; }

        public StandingID(int value)
        {
            if (value <= 0) throw new ArgumentException("El ID de la clasificación debe ser mayor que 0.");
            Value = value;
        }
    }
}