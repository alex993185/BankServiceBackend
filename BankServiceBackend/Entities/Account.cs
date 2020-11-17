using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServiceBackend.Entities
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

        public override string ToString()
        {
            return $"Account {Name} (Number = {AccountNumber}, Credit = {Credit}, Dispo = {Dispo})";
        }
    }
}
