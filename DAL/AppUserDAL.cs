using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL
{
    public interface IAppUserDAL
    {
        bool AuthenticateUser(LoginViewModel model);
        string RegisterUser(RegisterViewModel model);
    }
    public class AppUserDAL : IAppUserDAL
    {
        private const string AuthenticateUserQuery = @"
            SELECT acc.*
            FROM [dbo].[Account] acc with(nolock) INNER JOIN [dbo].[AppUser] au with(nolock) ON acc.[AccountId]=au.[AccountId] 
            WHERE acc.[Email] = @Email AND au.[Password] = @Password ";
        private const string RegisterUserQueryWithAddress = @"
            DECLARE @LocId INT;
            IF NOT EXISTS (SELECT * FROM [dbo].[Location] WHERE [Address]=@Address AND [City]=@City AND [Country]=@Country)
                BEGIN
                    INSERT INTO [dbo].[Location] ([Address] ,[City] ,[Country])
                    VALUES (@Address ,@City ,@Country);
                    SET @LocId = SCOPE_IDENTITY()
                END
            ELSE 
                BEGIN
                    SET @LocId = (SELECT [LocationId] FROM [dbo].[Location] WHERE [Address]=@Address AND [City]=@City AND [Country]=@Country)
                END

            INSERT INTO [dbo].[Account] ([FirstName] ,[LastName] ,[Email], [Phone] ,[AddressId])
            VALUES (@FirstName ,@LastName ,@Email, @Phone ,@LocId);

            INSERT INTO [dbo].[AppUser] ([AccountId],[Password])
            VALUES ( SCOPE_IDENTITY() , @Password)";
        private const string RegisterUserQueryWithoutAddress = @"
            INSERT INTO [dbo].[Account] ([FirstName] ,[LastName], [Phone] ,[Email])
            VALUES (@FirstName ,@LastName, @Phone ,@Email);

            INSERT INTO [dbo].[AppUser] ([AccountId],[Password])
            VALUES ( SCOPE_IDENTITY() , @Password)";

        public bool AuthenticateUser(LoginViewModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", model.Email));
            parameters.Add(new SqlParameter("@Password", model.Password));

            var dt = DBCommand.GetDataWithCondition(AuthenticateUserQuery, parameters);

            return dt.Rows.Count > 0;
        }

        public string RegisterUser(RegisterViewModel model)
        {
            var result = false;

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", model.Email));
            parameters.Add(new SqlParameter("@Password", model.Password));
            parameters.Add(new SqlParameter("@FirstName", model.FirstName));
            parameters.Add(new SqlParameter("@LastName", model.LastName));
            parameters.Add(new SqlParameter("@Phone", model.Phone));
            
            if (model.Address != null && model.Address.City != null && model.Address.Country != null)
            {
                parameters.Add(new SqlParameter("@Address", model.Address.Address));
                parameters.Add(new SqlParameter("@City", model.Address.City));
                parameters.Add(new SqlParameter("@Country", model.Address.Country));

                result = DBCommand.InsertUpdateData(RegisterUserQueryWithAddress, parameters);
            }
            else result = DBCommand.InsertUpdateData(RegisterUserQueryWithoutAddress, parameters);

            return result ? "Success" : "Error";
        }

    }
}