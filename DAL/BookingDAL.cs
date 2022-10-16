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
        List<BookingModel> GetBookings(int? RideId);
    }
    public class BookingDAL
    {
        private const string BookRideQuery = @"
            INSERT INTO Booking VALUES (@RideId, @PassengerId, @Seat)";
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
        public List<BookingModel> GetBookings(int? RideId)
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
    }
}