using JEDI_Carpool.Models;
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

        public static bool UpdateAccount(AccountModel model)
        {
            return AccountDAL.UpdateAccount(model);
        }

    }
}