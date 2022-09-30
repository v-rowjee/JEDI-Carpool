using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL.Common
{
    public class AccountBL
    {
        public static AccountModel GetAccountDetails(LoginViewModel model)
        {
            return AccountDAL.GetAccountDetails(model);
        }
    }
}