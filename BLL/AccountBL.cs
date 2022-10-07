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

        public static CarModel GetCar(LoginViewModel model)
        {
            return AccountDAL.GetCar(model.Email);
        }

        public static List<AccountModel> GetAllAccounts()
        {
            return AccountDAL.GetAllAccounts();
        }


    }
}