using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class ShareRideViewModel
    {
        public int DriverId { get; set; }

        public LocationModel Origin { get; set; }   
        public LocationModel Destination { get; set; }

        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Fare { get; set; }
        public string Comment { get; set; }

    }
}