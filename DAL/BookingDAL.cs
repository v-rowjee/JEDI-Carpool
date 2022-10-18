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
    public interface IBookingDAL
    {
        string BookRide(BookingModel model);
        List<BookingModel> GetBookingsByRideId(int? RideId);
        List<BookingModel> GetBookingsByDriverId(int AccountId);
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

            var dt = DBCommand.GetDataWithCondition(GetBookingsQuery, parameters);

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


        private const string GetBookingsByAccountIdQuery = @"
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

            var dt = DBCommand.GetDataWithCondition(GetBookingsByAccountIdQuery, parameters);

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