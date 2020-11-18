using System;
using BankServiceBackend.Entities;
using BankServiceBackend.Persistance.Entities;
using BankServiceBackend.Persistance.Enums;
using BankServiceBackend.Persistance.Exceptions;

namespace BankServiceBackend.Extensions
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
            return new UserDTO { CustomerNumber = user.CustomerNumber, Birthday = user.Birthday, FirstName = user.FirstName, Name = user.Name, Gender = user.Gender.ToString() };
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
