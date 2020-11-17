using System;
using System.ComponentModel.DataAnnotations;

namespace BankServiceBackend.Persistance.Entities
{
    public class TransferTransaction
    {
        [Key]
        public long Id { get; set; }
        public Account SourceAccount { get; set; }
        public Account TargetAccount { get; set; }
        public double AmountInEuro { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
