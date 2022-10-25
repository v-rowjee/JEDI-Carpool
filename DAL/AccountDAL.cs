using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using JEDI_Carpool.DAL.Common;
using System.Reflection;
using System.Security.Principal;

namespace JEDI_Carpool.DAL
{
    public interface IAccountDAL
    {
        AccountModel GetAccount(LoginViewModel model);
        List<AccountModel> GetAllAccounts();
        string UpdateAccount(AccountModel model);
        bool DeleteAccount(int id);
    }
    public class AccountDAL : IAccountDAL
    {
        private const string GetAccountQuery = @"
            SELECT acc.*, loc.[Region], loc.[City], loc.[Country]
            FROM [dbo].[Account] as acc 
            FULL JOIN [dbo].[Location] as loc ON acc.[AddressId] = loc.[LocationId]
            WHERE acc.[Email] = @Email
        ";
        private const string GetAllAccountsQuery = @"
            SELECT a.*, l.Region, l.City, l.Country 
            FROM Account a LEFT JOIN Location l ON a.AddressId=l.LocationId";
        private const string UpdateAccountQuery = @"
            IF NOT EXISTS ( SELECT * FROM Location WHERE Region=@Region AND City=@City AND Country=@Country )
                BEGIN
                    INSERT INTO Location (Region, City, Country) VALUES (@Region, @City, @Country);
                    UPDATE Account 
                    SET FirstName=@FirstName, LastName=@LastName, Phone=@Phone, Email=@Email, AddressId=SCOPE_IDENTITY()
                    WHERE AccountId=@AccountId;
                END
            ELSE
                BEGIN
                    UPDATE Account
                    SET FirstName=@FirstName, LastName=@LastName, Phone=@Phone, Email=@Email, AddressId= ( SELECT LocationId FROM Location WHERE Region=@Region AND City=@City AND Country=@Country )
                    WHERE AccountId=@AccountId;
                END
            ";
        private const string DeleteAccountQuery = @"
            DELETE FROM AppUser WHERE AccountId=@AccountId;
            DELETE FROM Car WHERE DriverId=@AccountId;
            DELETE FROM Booking WHERE PassengerId=@AccountId;
            DELETE FROM Ride WHERE DriverId=@AccountId;
            DELETE FROM Account WHERE AccountId=@AccountId";


        public AccountModel GetAccount(LoginViewModel model)
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
                account.Phone = row["Phone"].ToString();

                var address = new LocationModel();
                address.Region = row["Region"].ToString();
                address.City = row["City"].ToString();
                address.Country = row["Country"].ToString();
                account.Address = address;
            }

            return account;

        }

        public List<AccountModel> GetAllAccounts()
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
                account.Phone = row["Phone"].ToString();

                var address = new LocationModel();
                address.Region = row["Region"].ToString();
                address.City = row["City"].ToString();
                address.Country = row["Country"].ToString();
                account.Address = address;

                accounts.Add(account);
            }

            return accounts;
        }

        public string UpdateAccount(AccountModel model)
        {
            var isValid = false;
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@AccountId", model.AccountId));
            parameters.Add(new SqlParameter("@Email", model.Email));
            parameters.Add(new SqlParameter("@FirstName", model.FirstName));
            parameters.Add(new SqlParameter("@LastName", model.LastName));
            parameters.Add(new SqlParameter("@Phone", model.Phone));
            parameters.Add(new SqlParameter("@Region", model.Address.Region));
            parameters.Add(new SqlParameter("@City", model.Address.City));
            parameters.Add(new SqlParameter("@Country", model.Address.Country));

            isValid = DBCommand.InsertUpdateData(UpdateAccountQuery, parameters);
            

            return isValid ? "Success" : "Error";
        }

        public bool DeleteAccount(int id)
        {
            var parameter = new SqlParameter("@AccountId", id);
            return DBCommand.DeleteData(DeleteAccountQuery, parameter);
        }
    }
}