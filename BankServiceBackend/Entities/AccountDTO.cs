﻿using System.ComponentModel.DataAnnotations;

namespace BankServiceBackend.Entities
{
    public class AccountDTO
    {
        public long AccountNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Dispo { get; set; }

        public double Credit { get; set; }
    }
}