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
    public class RideDAL
    {
        private const string CreateRideQuery = @"
            DECLARE @OrgId INT;
            DECLARE @DestId INT;

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

            INSERT INTO Ride (DriverId, OriginId, DestinationId, DateTime, Fare, Comment) VALUES (@DriverId, @OrgId, @DestId, @DateTime, @Fare, @Comment)

";
        public static string Share(ShareRideViewModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DriverId", model.DriverId));
            parameters.Add(new SqlParameter("@DateTime", model.Date.Date.Add(model.Time.TimeOfDay)));
            parameters.Add(new SqlParameter("@Fare", model.Fare));
            parameters.Add(new SqlParameter("@Comment", model.Comment));

            parameters.Add(new SqlParameter("@OAddress",model.Origin.Address));
            parameters.Add(new SqlParameter("@OCity", model.Origin.City));
            parameters.Add(new SqlParameter("@OCountry", model.Origin.Country));

            parameters.Add(new SqlParameter("@DAddress", model.Destination.Address));
            parameters.Add(new SqlParameter("@DCity", model.Destination.City));
            parameters.Add(new SqlParameter("@DCountry", model.Destination.Country));

            DBCommand.InsertUpdateData(CreateRideQuery, parameters);

            return "Success";
        }



        private const string SearchRidesQuery = @"
            SELECT acc.FirstName, acc.LastName, acc.Email,rid.Fare, rid.Comment, car.PlateNumber, car.Model, car.Year, car.Seat, car.Color
            FROM Account acc 
            INNER JOIN Ride rid ON rid.DriverId=acc.AccountId 
            INNER JOIN Car car ON car.DriverId=acc.AccountId
            INNER JOIN Riders grp ON grp.RideId=rid.RideId
            WHERE OriginId IN (SELECT LocationId FROM Location WHERE City=@OCity AND Country=@OCountry)
            AND DestinationId IN (SELECT LocationId FROM Location WHERE City=@DCity AND Country=@DCountry)
            AND DAY(DateTime) >= DAY(@DateTime)
            AND car.Seat >= (SELECT SUM(Seat) FROM Riders GROUP BY RideId)
";

        public static List<RideViewModel> GetRidesWithCondition(SearchRideViewModel model)
        {
            var rides = new List<RideViewModel>();
            RideViewModel ride;

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@OAddress", model.OAddress));
            parameters.Add(new SqlParameter("@OCity", model.OAddress));
            parameters.Add(new SqlParameter("@OCountry", model.OCountry));

            parameters.Add(new SqlParameter("@DAddress", model.DAddress));
            parameters.Add(new SqlParameter("@DCity", model.DCity));
            parameters.Add(new SqlParameter("@DCountry", model.DCountry));

            parameters.Add(new SqlParameter("@DateTime", model.Date));

            var dt = DBCommand.GetDataWithCondition(SearchRidesQuery, parameters);

            foreach (DataRow row in dt.Rows)
            {
                ride = new RideViewModel();

                var driver = new AccountModel();
                driver.FirstName = row["FirstName"].ToString();
                driver.LastName = row["LastName"].ToString();
                ride.Driver = driver;

                var car = new CarModel();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Seat = int.Parse(row["Seat"].ToString());
                car.Color = row["Color"].ToString();

                var origin = new LocationModel();
                origin.Address = row["OAddress"].ToString();
                origin.City = row["OCity"].ToString();
                origin.Country = row["OCountry"].ToString();

                var destination = new LocationModel();
                destination.Address = row["DAddress"].ToString();
                destination.City = row["DCity"].ToString();
                destination.Country = row["DCountry"].ToString();

                ride.Driver = driver;
                ride.Car = car;
                ride.Fare = Convert.ToInt32(row["Fare"]);
                ride.DateTime = DateTime.Parse(row["DateTime"].ToString());
                ride.Origin = origin;
                ride.Destination = destination;

                rides.Add(ride);
            }

            return rides;
        }


        private const string GetAllRidesQuery = @"
            SELECT acc.FirstName, acc.LastName, acc.Email, car.Model, car.PlateNumber, car.Seat, car.Year, car.Color,
            rid.DateTime, rid.Comment, rid.Fare, org.Address as OAddress, org.City as OCity, org.Country as OCountry,
            dest.Address as DAddress, dest.City as DCity, dest.Country as DCountry
            FROM Account acc
            INNER JOIN Car car ON car.DriverId=acc.AccountId
            INNER JOIN Ride rid ON rid.DriverId=acc.AccountId
            INNER JOIN Location org ON org.LocationId=rid.OriginId
            INNER JOIN Location dest ON dest.LocationId=rid.DestinationId
            WHERE car.Seat >= (SELECT SUM(Seat) FROM Riders GROUP BY RideId)
";

        public static List<RideViewModel> GetAllRides()
        {
            var rides = new List<RideViewModel>();
            RideViewModel ride;
            var dt = DBCommand.GetData(GetAllRidesQuery);

            foreach (DataRow row in dt.Rows)
            {
                ride = new RideViewModel();

                var driver = new AccountModel();
                driver.FirstName = row["FirstName"].ToString();
                driver.LastName = row["LastName"].ToString();

                var car = new CarModel();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Seat = int.Parse(row["Seat"].ToString());
                car.Color = row["Color"].ToString();

                var origin = new LocationModel();
                origin.Address = row["OAddress"].ToString();
                origin.City = row["OCity"].ToString();
                origin.Country = row["OCountry"].ToString();

                var destination = new LocationModel();
                destination.Address = row["DAddress"].ToString();
                destination.City = row["DCity"].ToString();
                destination.Country = row["DCountry"].ToString();

                ride.Driver = driver;
                ride.Car = car;
                ride.Fare = Convert.ToInt32(row["Fare"]);
                ride.DateTime = DateTime.Parse(row["DateTime"].ToString());
                ride.Origin = origin;
                ride.Destination = destination;

                rides.Add(ride);
            }

            return rides;
        }

    }
}