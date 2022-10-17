using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;

namespace JEDI_Carpool.BLL
{
    public interface IRideBL
    {
        RideViewModel GetRide(int? id);
        List<RideViewModel> GetAllRides();
        string Share(ShareRideViewModel model);
        List<RideViewModel> Search(SearchRideViewModel model);
        int GetSeatsLeft(RideViewModel model);
    }
    public class RideBL : IRideBL
    {
        public IRideDAL RideDAL;
        public ICarDAL CarDAL;
        public IBookingDAL BookingDAL;
        
        public RideBL(IRideDAL RideDAL, ICarDAL CarDAL, IBookingDAL BookingDAL)
        {
            this.RideDAL = RideDAL;
            this.CarDAL = CarDAL;
            this.BookingDAL = BookingDAL;
        }

        public RideBL()
        {
            this.RideDAL = new RideDAL();
            this.CarDAL = new CarDAL();
            this.BookingDAL = new BookingDAL();
        }



        public RideViewModel GetRide(int? id)
        {
            return id != null ? RideDAL.GetRide(id) : null;
        }

        public List<RideViewModel> GetAllRides()
        {
            var rides = RideDAL.GetAllRides();

            // remove all rides that have no seats remaining
            foreach (var ride in rides.ToList())
            {
                ride.SeatsLeft = GetSeatsLeft(ride);
                if (!ValidateSeatsLeft(ride))
                {
                    rides.RemoveAll(r => r.RideId.Equals(ride.RideId));
                }
            }
            return rides;
        }

        public string Share(ShareRideViewModel model)
        {
            if (HasCar(model.DriverId))
            {
                return RideDAL.Share(model);
            }
            return "NoCar";
        }

        private bool HasCar(int DriverId)
        {
            var car = CarDAL.GetCar(DriverId);
            return car != null;
        }

        public List<RideViewModel> Search(SearchRideViewModel model)
        {
            return RideDAL.SearchRide(model);
        }

        public int GetSeatsLeft(RideViewModel ride)
        {
            var seatsMax = ride.Car.Seat;
            var bookings = BookingDAL.GetBookings(ride.RideId);
            int seatsTaken = 0;
            foreach (var booking in bookings)
            {
                seatsTaken += booking.Seat;
            }
            return seatsMax - seatsTaken;
        }


        // Validations
        private bool ValidateSeatsLeft(RideViewModel model)
        {
            return GetSeatsLeft(model) > 0;
        }
        private bool ValidateInputSeats(BookingModel model)
        {
            var seatsWanted = model.Seat;
            var seatsLeft = GetSeatsLeft(model.Ride);
            return seatsLeft >= seatsWanted;
        }

    }
}