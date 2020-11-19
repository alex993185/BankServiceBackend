using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankService.Backend.API.Entities
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

        public List<AccountDTO> Accounts { get; set; }
    }
}
