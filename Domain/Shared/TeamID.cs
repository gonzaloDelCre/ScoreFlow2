namespace Domain.Shared
{
    public class TeamID
    {
        public int Value { get; private set; }

        public TeamID(int value)
        {
            Value = value;
        }
    }
}
