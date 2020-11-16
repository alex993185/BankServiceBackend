using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServiceBackend.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CustomerNumber { get; set; } // TODO: Use for authentication, use from session

        [Required]
        public string HashedPassword { get; set; } // TODO: Use for authentication

        [Required]
        public string Name { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public IEnumerable<Account> Accounts { get; set; }
    }
}
