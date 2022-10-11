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
        public static bool Create(CarModel model)
        {
            return CarDAL.Create(model);
        } 
    }
}