using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankServiceBackend.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountNumber { get; set; } // TODO: Use for authentication, use from session
       
        [Required]
        public string HashedPin { get; set; } // TODO: Use for authentication

        [Required]
        public string Name { get; set; }
        
        [Required]
        public double Dispo { get; set; }

        public double Credit { get; set; } // TODO: Must not be changed!
    }
}
