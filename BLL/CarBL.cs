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
        bool Delete(int DriverId);
    }
    public class CarBL : ICarBL
    {
        public ICarDAL CarDAL;
        public CarBL(ICarDAL CarDAL)
        {
            this.CarDAL = CarDAL;
        }
        public CarBL()
        {
            this.CarDAL = new CarDAL();
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

        public bool Delete(int DriverId)
        {
            return CarDAL.Delete(DriverId);
        }

    }
}