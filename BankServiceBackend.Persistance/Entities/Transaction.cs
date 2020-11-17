using System;
using System.ComponentModel.DataAnnotations;

namespace BankServiceBackend.Persistance.Entities
{
    public class Transaction
    {
        [Key]
        public long Id { get; set; }
        public TransactionType Type { get; set; }
        public Account Account { get; set; }
        public double AmountInEuro { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public enum TransactionType
    {
        Withdrawal,
        Deposit
    }
}