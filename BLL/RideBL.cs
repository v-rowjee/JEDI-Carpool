using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace JEDI_Carpool.BLL
{
    public class RideBL
    {
        public static string Share(ShareRideViewModel model)
        {
            if (HasCar(model.DriverId))
            {
                return RideDAL.Share(model);
            }
            return "NoCar";
        }

        private static bool HasCar(int DriverId)
        {
            var car = CarDAL.GetCar(DriverId);
            return car != null;
        }

        public static List<RideViewModel> Search(SearchRideViewModel model)
        {
            return RideDAL.GetRidesWithCondition(model);
        }

        public static List<RideViewModel> GetAllRides()
        {
            return RideDAL.GetAllRides();
        }

    }
}