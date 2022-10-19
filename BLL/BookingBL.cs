using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.BLL
{
    public interface IBookingBL
    {
        string BookRide(BookingModel model);
        List<BookingModel> GetBookingsByRideId(int? RideId);
        List<BookingModel> GetBookingsByDriverId(int AccountId);
        List<BookingModel> GetBookings();
        bool DeleteBooking(int id);
    }
    public class BookingBL :IBookingBL
    {
        public IBookingDAL BookingDAL;
        public BookingBL(IBookingDAL BookingDAL)
        {
            this.BookingDAL = BookingDAL;
        }
        public BookingBL()
        {
            this.BookingDAL = new BookingDAL();
        }



        public string BookRide(BookingModel model)
        {
            if (ValidateInputSeats(model))
            {
                return BookingDAL.BookRide(model);
            }
            return "NoSeat";
        }

        public List<BookingModel> GetBookings()
        {
            return BookingDAL.GetBookings();
        }

        public List<BookingModel> GetBookingsByRideId(int? RideId)
        {
            return BookingDAL.GetBookingsByRideId(RideId);
        }

        public List<BookingModel> GetBookingsByDriverId(int AccountId)
        {
            return BookingDAL.GetBookingsByDriverId(AccountId);
        }


        public bool DeleteBooking(int id)
        {
            return BookingDAL.DeleteBooking(id);
        }


        // Validations
        private bool ValidateInputSeats(BookingModel model)
        {
            var seatsMax = model.Ride.Car.Seat;
            var seatsWanted = model.Seat;
            var bookings = BookingDAL.GetBookingsByRideId(model.Ride.RideId);
            int seatsTaken = 0;
            foreach (var booking in bookings)
            {
                seatsTaken += booking.Seat;
            }
            var seatsLeft = seatsMax - seatsTaken;
            return seatsLeft >= seatsWanted;
        }
    }
}