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
    public class AccountDAL
    {

        private const string GetAccountQuery = @"
            SELECT acc.*, loc.[Address], loc.[City], loc.[Country]
            FROM [dbo].[Account] as acc 
            INNER JOIN [dbo].[Location] as loc ON acc.[AddressId] = loc.[LocationId]
            WHERE acc.[Email] = @Email
        ";
        public static AccountModel GetAccount(LoginViewModel model)
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

                var address = new LocationModel();
                address.Address = row["Address"].ToString();
                address.City = row["City"].ToString();
                address.Country = row["Country"].ToString();
                account.Address = address;
            }

            return account;

        }

        private const string GetCarWithEmailQuery = @"
            SELECT * FROM Car c JOIN Account a ON c.DriverId=a.AccountId
            WHERE a.Email = @Email";

        public static CarModel GetCar(string email)
        {
            CarModel car = null;
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", email));

            var dt = DBCommand.GetDataWithCondition(GetCarWithEmailQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                car = new CarModel();
                car.CarId = int.Parse(row["CarId"].ToString());
                car.PlateNumber = row["PlateNumber"].ToString();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Color = row["Color"].ToString();
                car.Seat = int.Parse(row["Seat"].ToString());
            }

            return car;
        }

        private const string GetCarWithDriverIdQuery = @"
            SELECT * FROM Car
            WHERE DriverId = @DriverId";

        public static CarModel GetCar(int DriverId)
        {
            CarModel car = null;
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DriverId", DriverId));

            var dt = DBCommand.GetDataWithCondition(GetCarWithDriverIdQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                car = new CarModel();
                car.CarId = int.Parse(row["CarId"].ToString());
                car.PlateNumber = row["PlateNumber"].ToString();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Color = row["Color"].ToString();
                car.Seat = int.Parse(row["Seat"].ToString());
            }

            return car;
        }


        private const string GetAllAccountsQuery = @"
            SELECT a.*, l.Address, l.City, l.Country 
            FROM Account a JOIN Location l ON a.AddressId=l.LocationId";
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

                var address = new LocationModel();
                address.Address = row["Address"].ToString();
                address.City = row["City"].ToString();
                address.Country = row["Country"].ToString();
                account.Address = address;

                accounts.Add(account);
            }

            return accounts;
        }

    }
}