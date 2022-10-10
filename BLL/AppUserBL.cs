﻿using JEDI_Carpool.DAL;
using JEDI_Carpool.DAL.Common;
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

        public static string RegisterUser(RegisterViewModel model)
        {
            if (ValidateDuplicatedEmail(model.Email))
            {
                return AppUserDAL.RegisterUser(model);
            }
            return "DuplicatedEmail";
        }

        private static bool ValidateDuplicatedEmail(string email)
        {
            var accounts = AccountDAL.GetAllAccounts().FirstOrDefault(x => x.Email.Equals(email));

            return accounts == null;
        }
    }
}