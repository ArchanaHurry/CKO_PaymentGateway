using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Types;

namespace PaymentGateway.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public string Merchant { get; set; }
        public long MerchantBankAccount { get; set; }
        public long CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public int CVV { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; } // ISO 4217
        public string TransactionType { get; set; }
        public string BankTransactionId { get; set;  }
        public string BankResponseStatus { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public TransactionStatus Status { get; set; }
    }

}