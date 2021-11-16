namespace TransactionSystem.Actors.Messages
{
    public class CreateUserMessage
    {
        public int Id { get; private set; }
        public long StartingBalance { get; private set; }
        public CreateUserMessage(int id, long startingBalance)
        {
            Id = id;
            StartingBalance = startingBalance;
        }
    }
}