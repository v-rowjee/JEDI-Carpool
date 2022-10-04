using JEDI_Carpool.DAL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.BLL
{
    public class RideBL
    {
        public static bool Share(ShareRideViewModel model)
        {
            return RideDAL.Share(model);
        }
    }
}