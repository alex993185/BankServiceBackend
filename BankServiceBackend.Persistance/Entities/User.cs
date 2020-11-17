using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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

        public Gender Gender { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public override string ToString()
        {
            return $"User { JsonConvert.SerializeObject(this, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) }";
        }
    }
}
