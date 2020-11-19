using System;
using System.Linq;
using BankService.Backend.API.Entities;
using BankService.Backend.Persistance.Entities;
using BankService.Backend.Persistance.Enums;
using BankService.Backend.Persistance.Exceptions;

namespace BankService.Backend.API.Extensions
{
    public static class EntityExtensions
    {
        public static Account GetEntity(this AccountDTO account)
        {
            return new Account { AccountNumber = account.AccountNumber, Name = account.Name, Credit = account.Credit, Dispo = account.Dispo };
        }

        public static AccountDTO GetTransferObject(this Account account)
        {
            return new AccountDTO { AccountNumber = account.AccountNumber, Name = account.Name, Credit = account.Credit, Dispo = account.Dispo };
        }


        public static UserDTO GetTransferObject(this User user)
        {
            return new UserDTO { CustomerNumber = user.CustomerNumber, Birthday = user.Birthday, FirstName = user.FirstName, Name = user.Name, Gender = user.Gender.ToString(), Accounts = user.Accounts.Select(a => a.GetTransferObject()).ToList() };
        }

        public static User GetEntity(this UserDTO user)
        {
            if (!Enum.TryParse(user.Gender, true, out Gender gender))
            {
                throw new UserFriendlyException("Gender is not recognized! (Possible: Male, Female, Diverse)");
            }

            return new User { Birthday = user.Birthday, FirstName = user.FirstName, Name = user.Name, Gender = gender };
        }

    }
}
