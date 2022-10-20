using JEDI_Carpool.DAL;
using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CryptoHelper;
using System.Security.Policy;

namespace JEDI_Carpool.BLL
{
    public interface IAppUserBL
    {
        bool AuthenticateUser(LoginViewModel model);
        string RegisterUser(RegisterViewModel model);
    }

    public class AppUserBL : IAppUserBL
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
            var hashedPassword = AppUserDAL.GetHashedPassword(model);
            return VerifyPassword(hashedPassword, model.Password);
        }

        public string RegisterUser(RegisterViewModel model)
        {
            if (ValidateDuplicatedEmail(model.Email))
            {
                model.Password = HashPassword(model.Password);
                return AppUserDAL.RegisterUser(model);
            }
            return "DuplicatedEmail";
        }

        private bool ValidateDuplicatedEmail(string email)
        {
            var accountsWithSameEmail = AccountDAL.GetAllAccounts().Where(x => x.Email.Equals(email.Trim()));

            return accountsWithSameEmail.Count() == 0;
        }

        // Hash a password
        private string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        // Verify the password hash against the given password
        private bool VerifyPassword(string hash, string password)
        {
            return Crypto.VerifyHashedPassword(hash, password);
        }
    }
}