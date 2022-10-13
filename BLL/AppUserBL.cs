using JEDI_Carpool.DAL;
using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.BLL
{
    public interface IAppUserBL
    {
        bool AuthenticateUser(LoginViewModel model);
        string RegisterUser(RegisterViewModel model);
    }

    public class AppUserBL
    {
        public IAppUserDAL AppUserDAL;
        public IAccountDAL AccountDAL;
        public AppUserBL(IAppUserDAL AppUserDAL,IAccountDAL AccountDAL)
        {
            this.AppUserDAL = AppUserDAL;
            this.AccountDAL = AccountDAL;
        }
        public AppUserBL()
        {
            AppUserDAL = new AppUserDAL();
            AccountDAL = new AccountDAL();
        }



        public bool AuthenticateUser(LoginViewModel model)
        {
            return AppUserDAL.AuthenticateUser(model);
        }

        public string RegisterUser(RegisterViewModel model)
        {
            if (ValidateDuplicatedEmail(model.Email))
            {
                return AppUserDAL.RegisterUser(model);
            }
            return "DuplicatedEmail";
        }

        private bool ValidateDuplicatedEmail(string email)
        {
            var accounts = AccountDAL.GetAllAccounts().FirstOrDefault(x => x.Email.Equals(email));

            return accounts == null;
        }
    }
}