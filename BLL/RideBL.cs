using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace JEDI_Carpool.BLL
{
    public interface IRideBL
    {
        RideViewModel GetRide(int? id);
        List<RideViewModel> GetAllRides();
        string Share(ShareRideViewModel model);
        int GetSeatsLeft(RideViewModel model);
        List<RideViewModel> GetRidesByPassengerId(int id);
        List<RideViewModel> FilterRides(SearchRideViewModel search, List<RideViewModel> rides);
        bool DeleteRide(int id);
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

        public List<RideViewModel> GetRidesByPassengerId(int id)
        {
            return RideDAL.GetRidesByPassengerId(id);
        }


        public int GetSeatsLeft(RideViewModel ride)
        {
            var seatsMax = ride.Car.Seat;
            var bookings = BookingDAL.GetBookingsByRideId(ride.RideId);
            int seatsTaken = 0;
            foreach (var booking in bookings)
            {
                seatsTaken += booking.Seat;
            }
            return seatsMax - seatsTaken;
        }

        public List<RideViewModel> FilterRides(SearchRideViewModel search, List<RideViewModel> rides)
        {
            if (search != null)
            {
                if (search.RegionFrom != null)
                {
                    rides.RemoveAll(r => !string.Equals(r.Origin.Region, search.RegionFrom, StringComparison.CurrentCultureIgnoreCase));
                }
                if (search.CityFrom != null)
                {
                    rides.RemoveAll(r => !string.Equals(r.Origin.City, search.CityFrom, StringComparison.CurrentCultureIgnoreCase));
                }
                if (search.RegionTo != null)
                {
                    rides.RemoveAll(r => !string.Equals(r.Destination.Region, search.RegionTo, StringComparison.CurrentCultureIgnoreCase));
                }
                if (search.CityTo != null)
                {
                    rides.RemoveAll(r => !string.Equals(r.Destination.City, search.CityTo, StringComparison.CurrentCultureIgnoreCase));
                }
            }
            return rides;
        }

        public bool DeleteRide(int id)
        {
            return RideDAL.DeleteRide(id);
        }

        // Validations
        private bool ValidateSeatsLeft(RideViewModel model)
        {
            return GetSeatsLeft(model) > 0;
        }

    }
}