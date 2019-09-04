using System;
using PaymentGateway.Types;

namespace PaymentGateway.Entities
{
    public class Response
    {
        public string TransactionId { get; set; }
        public string BankTransactionId { get; set; }
        public string MerchantBankAccount { get; set; }
        public string CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public int CVV { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; } // ISO 4217
        public string TransactionType { get; set; }
        public string DateTime { get; set; }
        public string Status { get; set; }
    }
}
