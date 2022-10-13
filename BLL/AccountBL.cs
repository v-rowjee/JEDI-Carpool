using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL.Common
{
    public interface IAccountBL
    {
        AccountModel GetAccount(LoginViewModel model);
        List<AccountModel> GetAllAccounts();
        string UpdateAccount(AccountModel model);
    }
    public class AccountBL : IAccountBL
    {
        public IAccountDAL accountDAL;
        public AccountBL(IAccountDAL accountDAL)
        {
            this.accountDAL = accountDAL;
        }
        public AccountBL()
        {
            this.accountDAL = new AccountDAL();
        }


        public AccountModel GetAccount(LoginViewModel model)
        {
            return accountDAL.GetAccount(model);
        }

        public List<AccountModel> GetAllAccounts()
        {
            return accountDAL.GetAllAccounts();
        }

        public string UpdateAccount(AccountModel model)
        {
            if (ValidateDuplicatedEmail(model))
            {
                return accountDAL.UpdateAccount(model);
            }
            return "DuplicatedEmail";
        }

        private bool ValidateDuplicatedEmail(AccountModel model)
        {
            var accounts = accountDAL.GetAllAccounts()
                .Where(x => x.Email.Equals(model.Email))
                .FirstOrDefault(a => a.AccountId != model.AccountId);

            return accounts == null;
        }

    }
}