using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using JEDI_Carpool.DAL.Common;

namespace JEDI_Carpool.DAL
{
    public class AccountDAL
    {

        private const string GetAccountQuery = @"
            SELECT acc.*, loc.[Address], loc.[City], loc.[Country]
            FROM [dbo].[Account] as acc JOIN [dbo].[Location] as loc ON acc.[AddressId] = loc.[LocationId]
            WHERE acc.[Email] = @Email
        ";
        public static AccountModel GetAccountDetails(LoginViewModel model)
        {
            var account = new AccountModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", model.Email));

            var dt = DBCommand.GetDataWithCondition(GetAccountQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                account.AccountId = int.Parse(row["AccountId"].ToString());
                account.Email = row["Email"].ToString().Trim();
                account.FirstName = row["FirstName"].ToString();
                account.LastName = row["LastName"].ToString();
                account.Address = row["Address"].ToString();
                account.City = row["City"].ToString();
                account.Country = row["Country"].ToString();
            }

            return account;

        }


        private const string GetAllAccountsQuery = @"SELECT * FROM Account";
        public static List<AccountModel> GetAllAccounts()
        {
            var accounts = new List<AccountModel>();
            AccountModel account;

            var dt = DBCommand.GetData(GetAllAccountsQuery);
            foreach (DataRow row in dt.Rows)
            {
                account = new AccountModel();
                account.AccountId = int.Parse(row["AccountId"].ToString());
                account.Email = row["Email"].ToString().Trim();
                account.FirstName = row["FirstName"].ToString();
                account.LastName = row["LastName"].ToString();
                account.Address = row["Address"].ToString();
                account.City = row["City"].ToString();
                account.Country = row["Country"].ToString();

                accounts.Add(account);
            }

            return accounts;
        }

    }
}