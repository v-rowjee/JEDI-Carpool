﻿using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL.Common
{
    public class AccountBL
    {
        public static AccountModel GetAccount(LoginViewModel model)
        {
            return AccountDAL.GetAccount(model);
        }

        public static List<AccountModel> GetAllAccounts()
        {
            return AccountDAL.GetAllAccounts();
        }

        public static string UpdateAccount(AccountModel model)
        {
            if (ValidateDuplicatedEmail(model))
            {
                return AccountDAL.UpdateAccount(model);
            }
            return "DuplicatedEmail";
        }

        private static bool ValidateDuplicatedEmail(AccountModel model)
        {
            var accounts = AccountDAL.GetAllAccounts()
                .Where(x => x.Email.Equals(model.Email))
                .FirstOrDefault(a => a.AccountId != model.AccountId);

            return accounts == null;
        }

    }
}