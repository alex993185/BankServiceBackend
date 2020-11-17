using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServiceBackend.Persistance.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CustomerNumber { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public DateTime Birthday { get; set; }

        public string Gender { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
