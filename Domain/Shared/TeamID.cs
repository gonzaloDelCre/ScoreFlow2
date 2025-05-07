namespace Domain.Shared
{
    public class TeamID
    {
        public int Value { get; }

        public TeamID(int value)
        {
            if (value == null || value <= 0)
                throw new ArgumentException("TeamID debe ser un valor positivo y no nulo.");
            Value = value;
        }

    }
}
