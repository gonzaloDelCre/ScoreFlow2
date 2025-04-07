namespace Domain.Entities.Standings
{
    public class Draws
    {
        public int Value { get; private set; }

        public Draws(int value)
        {
            if (value < 0) throw new ArgumentException("El número de empates no puede ser negativo.");
            Value = value;
        }
    }
}
