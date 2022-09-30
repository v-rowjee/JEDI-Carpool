using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.BLL
{
    public static class AppUserBL
    {
        public static bool AuthenticateUser(LoginViewModel model)
        {
            return AppUserDAL.AuthenticateUser(model);
        }

        public static bool RegisterUser(RegisterViewModel model)
        {
            return AppUserDAL.RegisterUser(model);
        }
    }
}