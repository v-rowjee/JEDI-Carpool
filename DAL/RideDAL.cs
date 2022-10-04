using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL
{
    public class RideDAL
    {
        private const string CreateRideQuery = @"
            DECLARE OrgId INT;
            DECLARE DestId INT;

            IF NOT EXISTS (SELECT * FROM Location WHERE Address=@OAddress AND City=@OCity AND Country=@OCountry)
                BEGIN
                    INSERT INTO Location (Address, City, Country) VALUES (@OAddress, @OCity, @OCountry);
                    SET @OrgId = SCOPE_IDENTITY();
                END
            ELSE
                SET @OrgId = (SELECT LocationId FROM Location WHERE Address=@OAddress AND City=@OCity AND Country=@OCountry)

            IF NOT EXISTS (SELECT * FROM Location WHERE Address=@DAddress AND City=@DCity AND Country=@DCountry)
                BEGIN
                    INSERT INTO Location (Address, City, Country) VALUES (@DAddress, @DCity, @DCountry);
                    SET @DestId = SCOPE_IDENTITY();
                END
            ELSE
                SET @DestId = (SELECT LocationId FROM Location WHERE Address=@DAddress AND City=@Dcity AND Country=@DCountry)

            INSERT INTO Ride (DriverId, OriginId, DestinationId, DateTime, Fare) VALUES (@DriverId, @OrgId, @DestId, @DateTime, @Fare)

";
        public static bool Share(ShareRideViewModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DriverId", model.DriverId));
            parameters.Add(new SqlParameter("@DateTime", model.Date.Date + model.Time.TimeOfDay));
            parameters.Add(new SqlParameter("@Fare", model.Fare));
            parameters.Add(new SqlParameter("@Comment", model.Comment));

            parameters.Add(new SqlParameter("@OAddress",model.OAddress));
            parameters.Add(new SqlParameter("@OCity", model.OCity));
            parameters.Add(new SqlParameter("@OCountry", model.OCountry));

            parameters.Add(new SqlParameter("@DAddress", model.DAddress));
            parameters.Add(new SqlParameter("@DCity", model.DCity));
            parameters.Add(new SqlParameter("@DCountry", model.DCountry));

            DBCommand.InsertUpdateData(CreateRideQuery, parameters);

            return true;
        }
    }
}