using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.BLL
{
    public interface ICarBL
    {
        CarModel GetCar(int DriverId);
        bool Create(CarModel model);
        bool Edit(CarModel model);
        string Delete(int DriverId);
    }
    public class CarBL : ICarBL
    {
        public ICarDAL CarDAL;
        public IRideDAL RideDAL;
        public IBookingDAL BookingDAL;

        public CarBL(ICarDAL CarDAL, IRideDAL RideDAL, IBookingDAL BookingDAL)
        {
            this.CarDAL = CarDAL;
            this.RideDAL = RideDAL;
            this.BookingDAL = BookingDAL;
        }
        public CarBL()
        {
            this.CarDAL = new CarDAL();
            this.RideDAL = new RideDAL();
            this.BookingDAL = new BookingDAL();
        }



        public CarModel GetCar(int DriverId)
        {
            return CarDAL.GetCar(DriverId);
        }

        public bool Create(CarModel model)
        {
            return CarDAL.Create(model);
        } 

        public bool Edit(CarModel model)
        {
            return CarDAL.Edit(model);
        }

        public string Delete(int DriverId)
        {
            if (ValidateDeleteCar(DriverId))
            {
                CarDAL.Delete(DriverId);
                return "Success";
            }
            return "HasRide";
        }

        private bool ValidateDeleteCar(int DriverId)
        {
            var ridesByCarOwner = RideDAL.GetAllRides().FirstOrDefault(r => r.Driver.AccountId == DriverId);

            return ridesByCarOwner == null;
        }

    }
}