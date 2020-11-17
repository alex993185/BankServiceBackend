using System;
using System.ComponentModel.DataAnnotations;

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
        public string Gender { get; set; }
    }
}
