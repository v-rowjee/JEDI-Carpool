using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace JEDI_Carpool.DAL
{
    public interface IRideDAL
    {
        string Share(ShareRideViewModel model);
        List<RideViewModel> SearchRide(SearchRideViewModel model);
        List<RideViewModel> GetAllRides();
        RideViewModel GetRide(int? id);
        List<RideViewModel> GetRidesByPassengerId(int id);
    }
    public class RideDAL : IRideDAL
    {
        private const string CreateRideQuery = @"
            DECLARE @OrgId INT;
            DECLARE @DestId INT;

            IF NOT EXISTS (SELECT * FROM Location WHERE Region=@ORegion AND City=@OCity AND Country=@OCountry)
                BEGIN
                    INSERT INTO Location (Region, City, Country) VALUES (@ORegion, @OCity, @OCountry);
                    SET @OrgId = SCOPE_IDENTITY();
                END
            ELSE
                SET @OrgId = (SELECT LocationId FROM Location WHERE Region=@ORegion AND City=@OCity AND Country=@OCountry)

            IF NOT EXISTS (SELECT * FROM Location WHERE Region=@DRegion AND City=@DCity AND Country=@DCountry)
                BEGIN
                    INSERT INTO Location (Region, City, Country) VALUES (@DRegion, @DCity, @DCountry);
                    SET @DestId = SCOPE_IDENTITY();
                END
            ELSE
                SET @DestId = (SELECT LocationId FROM Location WHERE Region=@DRegion AND City=@Dcity AND Country=@DCountry)

            INSERT INTO Ride (DriverId, OriginId, DestinationId, DateTime, Fare, Comment) VALUES (@DriverId, @OrgId, @DestId, @DateTime, @Fare, @Comment)

";
        public string Share(ShareRideViewModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DriverId", model.DriverId));
            parameters.Add(new SqlParameter("@DateTime", model.Date.Date.Add(model.Time.TimeOfDay)));
            parameters.Add(new SqlParameter("@Fare", model.Fare));
            parameters.Add(new SqlParameter("@Comment", model.Comment ?? (object)DBNull.Value));

            parameters.Add(new SqlParameter("@ORegion",model.Origin.Region));
            parameters.Add(new SqlParameter("@OCity", model.Origin.City));
            parameters.Add(new SqlParameter("@OCountry", model.Origin.Country));

            parameters.Add(new SqlParameter("@DRegion", model.Destination.Region));
            parameters.Add(new SqlParameter("@DCity", model.Destination.City));
            parameters.Add(new SqlParameter("@DCountry", model.Destination.Country));

            DBCommand.InsertUpdateData(CreateRideQuery, parameters);

            return "Success";
        }


        private const string SearchRidesQuery = @"
            SELECT acc.FirstName, acc.LastName, acc.Email,rid.Fare, rid.Comment, car.PlateNumber, car.Model, car.Year, car.Seat, car.Color, SUM(Seat)
            FROM Account acc 
            INNER JOIN Ride rid ON rid.DriverId=acc.AccountId 
            INNER JOIN Car car ON car.DriverId=acc.AccountId
            INNER JOIN Booking grp ON grp.RideId=rid.RideId
            WHERE OriginId IN (SELECT LocationId FROM Location WHERE City=@OCity AND Country=@Country)
            AND DestinationId IN (SELECT LocationId FROM Location WHERE City=@DCity AND Country=@Country)
            AND DAY(DateTime) >= DAY(@DateTime)";
        public List<RideViewModel> SearchRide(SearchRideViewModel model)
        {
            var rides = new List<RideViewModel>();
            RideViewModel ride;

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ORegion", model.Origin.Region));
            parameters.Add(new SqlParameter("@OCity", model.Origin.City));
            parameters.Add(new SqlParameter("@OCountry", model.Origin.Country));
            parameters.Add(new SqlParameter("@DRegion", model.Destination.Region));
            parameters.Add(new SqlParameter("@DCity", model.Destination.City));
            parameters.Add(new SqlParameter("@DCountry", model.Destination.Country));
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
                origin.Region = row["ORegion"].ToString();
                origin.City = row["OCity"].ToString();
                origin.Country = row["OCountry"].ToString();

                var destination = new LocationModel();
                destination.Region = row["DRegion"].ToString();
                destination.City = row["DCity"].ToString();
                destination.Country = row["DCountry"].ToString();

                ride.Driver = driver;
                ride.Car = car;
                ride.Fare = Convert.ToInt32(row["Fare"]);
                ride.DateTime = DateTime.Parse(row["DateTime"].ToString());
                ride.Origin = origin;
                ride.Destination = destination;
                ride.Comment = row["Comment"].ToString();

                rides.Add(ride);
            }

            return rides;
        }


        private const string GetAllRidesQuery = @"
            SELECT rid.RideId, rid.Fare, rid.DateTime, rid.Comment,
                acc.AccountId, acc.FirstName, acc.LastName, acc.Email, acc.Phone,
                car.Model, car.PlateNumber, car.Seat, car.Year, car.Color,
                org.Region as ORegion, org.City as OCity, org.Country as OCountry,
                dest.Region as DRegion, dest.City as DCity, dest.Country as DCountry
            FROM Account acc
            INNER JOIN Car car ON car.DriverId=acc.AccountId
            INNER JOIN Ride rid ON rid.DriverId=acc.AccountId
            INNER JOIN Location org ON org.LocationId=rid.OriginId
            INNER JOIN Location dest ON dest.LocationId=rid.DestinationId
            WHERE DateTime >= GETDATE()
            ORDER BY DateTime";
        public List<RideViewModel> GetAllRides()
        {
            var rides = new List<RideViewModel>();
            RideViewModel ride;
            var dt = DBCommand.GetData(GetAllRidesQuery);

            foreach (DataRow row in dt.Rows)
            {
                ride = new RideViewModel();
            
                var car = new CarModel();
                car.PlateNumber = row["PlateNumber"].ToString();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Seat = int.Parse(row["Seat"].ToString());
                car.Color = row["Color"].ToString();

                var driver = new AccountModel();
                driver.AccountId = int.Parse(row["AccountId"].ToString());
                driver.FirstName = row["FirstName"].ToString();
                driver.LastName = row["LastName"].ToString();
                driver.Email = row["Email"].ToString();
                driver.Phone = row["Phone"].ToString();
                driver.Car = car;

                var origin = new LocationModel();
                origin.Region = row["ORegion"].ToString();
                origin.City = row["OCity"].ToString();
                origin.Country = row["OCountry"].ToString();

                var destination = new LocationModel();
                destination.Region = row["DRegion"].ToString();
                destination.City = row["DCity"].ToString();
                destination.Country = row["DCountry"].ToString();

                ride.RideId = int.Parse(row["RideId"].ToString());
                ride.Driver = driver;
                ride.Car = car;
                ride.Fare = Convert.ToInt32(row["Fare"]);
                ride.DateTime = DateTime.Parse(row["DateTime"].ToString());
                ride.Origin = origin;
                ride.Destination = destination;
                ride.Comment = row["Comment"].ToString();

                rides.Add(ride);
            }

            return rides;
        }


        private const string GetRideQuery = @"
            SELECT rid.RideId, rid.Fare, rid.DateTime, rid.Comment,
                acc.AccountId, acc.FirstName, acc.LastName, acc.Email, acc.Phone,
                car.Model, car.PlateNumber, car.Seat, car.Year, car.Color,
                org.Region as ORegion, org.City as OCity, org.Country as OCountry,
                dest.Region as DRegion, dest.City as DCity, dest.Country as DCountry
            FROM Account acc
            INNER JOIN Car car ON car.DriverId=acc.AccountId
            INNER JOIN Ride rid ON rid.DriverId=acc.AccountId
            INNER JOIN Location org ON org.LocationId=rid.OriginId
            INNER JOIN Location dest ON dest.LocationId=rid.DestinationId
            WHERE rid.RideId = @RideId";
        public RideViewModel GetRide(int? id)
        {
            RideViewModel ride = null;

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@RideId", id));

            var dt = DBCommand.GetDataWithCondition(GetRideQuery,parameters);
            foreach (DataRow row in dt.Rows)
            {
                ride = new RideViewModel();

                var car = new CarModel();
                car.PlateNumber = row["PlateNumber"].ToString();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Seat = int.Parse(row["Seat"].ToString());
                car.Color = row["Color"].ToString();

                var driver = new AccountModel();
                driver.AccountId = int.Parse(row["AccountId"].ToString());
                driver.FirstName = row["FirstName"].ToString();
                driver.LastName = row["LastName"].ToString();
                driver.Email = row["Email"].ToString();
                driver.Phone = row["Phone"].ToString();
                driver.Car = car;

                var origin = new LocationModel();
                origin.Region = row["ORegion"].ToString();
                origin.City = row["OCity"].ToString();
                origin.Country = row["OCountry"].ToString();

                var destination = new LocationModel();
                destination.Region = row["DRegion"].ToString();
                destination.City = row["DCity"].ToString();
                destination.Country = row["DCountry"].ToString();

                ride.RideId = int.Parse(row["RideId"].ToString());
                ride.Driver = driver;
                ride.Car = car;
                ride.Fare = Convert.ToInt32(row["Fare"]);
                ride.DateTime = DateTime.Parse(row["DateTime"].ToString());
                ride.Origin = origin;
                ride.Destination = destination;
                ride.Comment = row["Comment"].ToString();

            }
            return ride;
        }


        private const string GetRidesByPassegerIdQuery = @"
            SELECT rid.RideId, rid.Fare, rid.DateTime, rid.Comment,
                acc.AccountId, acc.FirstName, acc.LastName, acc.Email, acc.Phone,
                car.Model, car.PlateNumber, car.Seat, car.Year, car.Color,
                org.Region as ORegion, org.City as OCity, org.Country as OCountry,
                dest.Region as DRegion, dest.City as DCity, dest.Country as DCountry
            FROM Account acc
            INNER JOIN Car car ON car.DriverId=acc.AccountId
            INNER JOIN Ride rid ON rid.DriverId=acc.AccountId
            INNER JOIN Location org ON org.LocationId=rid.OriginId
            INNER JOIN Location dest ON dest.LocationId=rid.DestinationId
            INNER JOIN Booking b ON b.RideId=rid.RideId
            WHERE DateTime >= GETDATE()
            AND b.PassengerId=@PassengerId
            ORDER BY DateTime";
        public List<RideViewModel> GetRidesByPassengerId(int id)
        {
            var rides = new List<RideViewModel>();
            RideViewModel ride;

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@PassengerId", id));

            var dt = DBCommand.GetDataWithCondition(GetRidesByPassegerIdQuery,parameters);

            foreach (DataRow row in dt.Rows)
            {
                ride = new RideViewModel();

                var car = new CarModel();
                car.PlateNumber = row["PlateNumber"].ToString();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Seat = int.Parse(row["Seat"].ToString());
                car.Color = row["Color"].ToString();

                var driver = new AccountModel();
                driver.AccountId = int.Parse(row["AccountId"].ToString());
                driver.FirstName = row["FirstName"].ToString();
                driver.LastName = row["LastName"].ToString();
                driver.Email = row["Email"].ToString();
                driver.Phone = row["Phone"].ToString();
                driver.Car = car;

                var origin = new LocationModel();
                origin.Region = row["ORegion"].ToString();
                origin.City = row["OCity"].ToString();
                origin.Country = row["OCountry"].ToString();

                var destination = new LocationModel();
                destination.Region = row["DRegion"].ToString();
                destination.City = row["DCity"].ToString();
                destination.Country = row["DCountry"].ToString();

                ride.RideId = int.Parse(row["RideId"].ToString());
                ride.Driver = driver;
                ride.Car = car;
                ride.Fare = Convert.ToInt32(row["Fare"]);
                ride.DateTime = DateTime.Parse(row["DateTime"].ToString());
                ride.Origin = origin;
                ride.Destination = destination;
                ride.Comment = row["Comment"].ToString();

                rides.Add(ride);
            }

            return rides;
        }

    }
}