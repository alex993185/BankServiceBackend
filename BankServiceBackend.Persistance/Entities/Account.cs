using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BankServiceBackend.Persistance.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountNumber { get; set; }
       
        public string HashedPin { get; set; }

        public string Name { get; set; }
        
        public double Dispo { get; set; }

        public double Credit { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public override string ToString()
        {
            return $"Account { JsonConvert.SerializeObject(this, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) }";
        }
    }
}
