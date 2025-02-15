namespace FraudDetection.Models
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
    }
}
