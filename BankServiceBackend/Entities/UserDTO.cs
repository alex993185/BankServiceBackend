using System;
using System.ComponentModel.DataAnnotations;
using BankServiceBackend.Persistance.Entities;

namespace BankServiceBackend.Entities
{
    public class UserDTO
    {
        public long CustomerNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}
