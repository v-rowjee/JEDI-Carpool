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
        List<BookingModel> GetBookings(int? RideId);
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
            return BookingDAL.BookRide(model);
        }

        public List<BookingModel> GetBookings(int? RideId)
        {
            return BookingDAL.GetBookings(RideId);
        }


    }
}