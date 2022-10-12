using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.BLL
{
    public class CarBL
    {
        public static CarModel GetCar(int DriverId)
        {
            return CarDAL.GetCar(DriverId);
        }
        public static bool Create(CarModel model)
        {
            return CarDAL.Create(model);
        } 

        public static bool Edit(CarModel model)
        {
            return CarDAL.Edit(model);
        }

        public static bool Delete(int DriverId)
        {
            return CarDAL.Delete(DriverId);
        }

    }
}