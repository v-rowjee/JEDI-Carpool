using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        List<PassengerModel> GetPassengers(int? id);
        bool BookRide(BookingModel model);
    }
    public class RideBL : IRideBL
    {
        public IRideDAL RideDAL;
        public ICarDAL CarDAL;
        
        public RideBL(IRideDAL RideDAL, ICarDAL CarDAL)
        {
            this.RideDAL = RideDAL;
            this.CarDAL = CarDAL;
        }

        public RideBL()
        {
            this.RideDAL = new RideDAL();
            this.CarDAL = new CarDAL();
        }



        public RideViewModel GetRide(int? id)
        {
            return id != null ? RideDAL.GetRide(id) : null;
        }

        public List<RideViewModel> GetAllRides()
        {
            return RideDAL.GetAllRides();
        }

        public List<PassengerModel> GetPassengers(int? id)
        {
            return RideDAL.GetPassengers(id);
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
            return RideDAL.GetRidesWithCondition(model);
        }

        public bool BookRide(BookingModel model)
        {
            return RideDAL.BookRide(model);
        }

    }
}