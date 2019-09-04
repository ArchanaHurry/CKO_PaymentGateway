namespace PaymentGateway.Entities
{
    public class BankTransaction
    {
        public long PayeeBankAccount { get; set; }
        public long PayerCardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public int CVV { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; } // ISO 4217
    }
}
