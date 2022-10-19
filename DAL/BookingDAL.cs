using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.BLL;

namespace JEDI_Carpool.DAL
{
    public interface IBookingDAL
    {
        string BookRide(BookingModel model);
        List<BookingModel> GetBookingsByRideId(int? RideId);
        List<BookingModel> GetBookingsByDriverId(int AccountId);
        List<BookingModel> GetBookings();
        bool DeleteBooking(int id);
    }
    public class BookingDAL: IBookingDAL
    {
        private const string BookRideQuery = @"
            IF NOT EXISTS (SELECT BookingId FROM Booking 
                            WHERE PassengerId=@PassengerId 
                            AND RideId=@RideId)
                BEGIN
                    INSERT INTO Booking VALUES (@RideId, @PassengerId, @Seat)
                END
            ELSE
                BEGIN
                    UPDATE Booking SET Seat=Seat+@Seat WHERE PassengerId=@PassengerId
                END";
        public string BookRide(BookingModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@RideId", model.Ride.RideId));
            parameters.Add(new SqlParameter("@PassengerId", model.Passenger.AccountId));
            parameters.Add(new SqlParameter("@Seat", model.Seat));

            var result = DBCommand.InsertUpdateData(BookRideQuery, parameters);

            return result ? "Success" : "Error";
        }


        private const string GetBookingsQuery = @"
            SELECT b.BookingId, b.PassengerId, b.Seat AS SeatsTaken,
                rid.RideId, rid.Fare, rid.DateTime, rid.Comment,
                p.AccountId, p.FirstName, p.LastName, p.Email, p.Phone,
                d.AccountId AS DAccountId, d.FirstName AS DFirstName, d.LastName AS DLastName, d.Email AS DEmail, d.Phone AS DPhone,
                car.CarId, car.Model, car.PlateNumber, car.Seat, car.Year, car.Color,
                org.Region as ORegion, org.City as OCity, org.Country as OCountry,
                dest.Region as DRegion, dest.City as DCity, dest.Country as DCountry
            FROM Account d
            INNER JOIN Car car ON car.DriverId=d.AccountId
            INNER JOIN Ride rid ON rid.DriverId=d.AccountId
            INNER JOIN Location org ON org.LocationId=rid.OriginId
            INNER JOIN Location dest ON dest.LocationId=rid.DestinationId
            INNER JOIN Booking b ON b.RideId = rid.RideId
            INNER JOIN Account p ON p.AccountId=b.PassengerId
            WHERE DateTime >= GETDATE()
            ORDER BY DateTime";
        public List<BookingModel> GetBookings()
        {
            List<BookingModel> bookings = new List<BookingModel>();
            BookingModel booking;

            var dt = DBCommand.GetData(GetBookingsQuery);

            foreach (DataRow row in dt.Rows)
            {
                booking = new BookingModel();

                booking.BookingId = int.Parse(row["BookingId"].ToString());

                var passenger = new AccountModel();
                passenger.AccountId = int.Parse(row["AccountId"].ToString());
                passenger.FirstName = row["FirstName"].ToString();
                passenger.LastName = row["LastName"].ToString();
                passenger.Email = row["Email"].ToString();
                passenger.Phone = row["Phone"].ToString();

                // Ride
                var car = new CarModel();
                car.CarId = int.Parse(row["CarId"].ToString());
                car.PlateNumber = row["PlateNumber"].ToString();
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

                var driver = new AccountModel();
                driver.AccountId = int.Parse(row["DAccountId"].ToString());
                driver.FirstName = row["DFirstName"].ToString();
                driver.LastName = row["DLastName"].ToString();
                driver.Email = row["DEmail"].ToString();
                driver.Phone = row["DPhone"].ToString();
                driver.Car = car;

                var ride = new RideViewModel();
                ride.RideId = int.Parse(row["RideId"].ToString());
                ride.Driver = driver;
                ride.Car = car;
                ride.Fare = Convert.ToInt32(row["Fare"]);
                ride.DateTime = DateTime.Parse(row["DateTime"].ToString());
                ride.Origin = origin;
                ride.Destination = destination;
                ride.Comment = row["Comment"].ToString();



                booking.Passenger = passenger;
                booking.Ride = ride;
                booking.Seat = int.Parse(row["SeatsTaken"].ToString());
                bookings.Add(booking);
            }
            return bookings;
        }

        private const string GetBookingsByRideIdQuery = @"
            SELECT b.BookingId, b.Seat, a.AccountId, a.Email, a.FirstName, a.LastName, a.Phone  
            FROM Booking b INNER JOIN Account a ON b.PassengerId=a.AccountId
            INNER JOIN Ride r ON r.RideId=b.RideId
            WHERE b.RideId=@RideId";
        public List<BookingModel> GetBookingsByRideId(int? RideId)
        {
            List<BookingModel> bookings = new List<BookingModel>();
            BookingModel booking;

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@RideId", RideId));

            var dt = DBCommand.GetDataWithCondition(GetBookingsByRideIdQuery, parameters);

            foreach (DataRow row in dt.Rows)
            {
                booking = new BookingModel();

                booking.BookingId = int.Parse(row["BookingId"].ToString());

                var passenger = new AccountModel();
                passenger.AccountId = int.Parse(row["AccountId"].ToString());
                passenger.FirstName = row["FirstName"].ToString();
                passenger.LastName = row["LastName"].ToString();
                passenger.Email = row["Email"].ToString();
                passenger.Phone = row["Phone"].ToString();
                booking.Passenger = passenger;

                booking.Seat = int.Parse(row["Seat"].ToString());

                bookings.Add(booking);
            }
            return bookings;
        }


        private const string GetBookingsByDriverIdQuery = @"
            SELECT b.BookingId, b.Seat, a.AccountId, a.Email, a.FirstName, a.LastName, a.Phone  
            FROM Booking b INNER JOIN Account a ON b.PassengerId=a.AccountId
            INNER JOIN Ride r ON r.RideId=b.RideId
            INNER JOIN Account a ON a.AccountId=r.DriverId
            WHERE a.AccountId=@AccountId";
        public List<BookingModel> GetBookingsByDriverId(int AccountId)
        {
            List<BookingModel> bookings = new List<BookingModel>();
            BookingModel booking;

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@AccountId", AccountId));

            var dt = DBCommand.GetDataWithCondition(GetBookingsByDriverIdQuery, parameters);

            foreach (DataRow row in dt.Rows)
            {
                booking = new BookingModel();

                booking.BookingId = int.Parse(row["BookingId"].ToString());

                var passenger = new AccountModel();
                passenger.AccountId = int.Parse(row["AccountId"].ToString());
                passenger.FirstName = row["FirstName"].ToString();
                passenger.LastName = row["LastName"].ToString();
                passenger.Email = row["Email"].ToString();
                passenger.Phone = row["Phone"].ToString();
                booking.Passenger = passenger;

                booking.Seat = int.Parse(row["Seat"].ToString());

                bookings.Add(booking);
            }
            return bookings;
        }


        private const string DeleteBookingQuery = @"
            DELETE FROM Booking WHERE BookingId = @BookingId";
        public bool DeleteBooking(int id)
        {
            var parameter = new SqlParameter("@BookingId", id);

            return DBCommand.DeleteData(DeleteBookingQuery, parameter);
        }

    }
}