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
        bool DeleteAccount(int id);
    }
    public class AccountBL : IAccountBL
    {
        public IAccountDAL AccountDAL;
        public AccountBL(IAccountDAL AccountDAL)
        {
            this.AccountDAL = AccountDAL;
        }
        public AccountBL()
        {
            this.AccountDAL = new AccountDAL();
        }


        public AccountModel GetAccount(LoginViewModel model)
        {
            return AccountDAL.GetAccount(model);
        }

        public List<AccountModel> GetAllAccounts()
        {
            return AccountDAL.GetAllAccounts();
        }

        public string UpdateAccount(AccountModel model)
        {
            if (ValidateDuplicatedEmail(model))
            {
                return AccountDAL.UpdateAccount(model);
            }
            return "DuplicatedEmail";
        }

        public bool DeleteAccount(int id)
        {
            return AccountDAL.DeleteAccount(id);
        }

        private bool ValidateDuplicatedEmail(AccountModel model)
        {
            var accounts = AccountDAL.GetAllAccounts()
                .Where(x => x.Email.Equals(model.Email))
                .FirstOrDefault(a => a.AccountId != model.AccountId);

            return accounts == null;
        }

    }
}