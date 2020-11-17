using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServiceBackend.Persistance.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountNumber { get; set; }
       
        [Required]
        public string HashedPin { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public double Dispo { get; set; }

        public double Credit { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public override string ToString()
        {
            return $"Account {Name} (Number = {AccountNumber}, Credit = {Credit}, Dispo = {Dispo})";
        }
    }
}
