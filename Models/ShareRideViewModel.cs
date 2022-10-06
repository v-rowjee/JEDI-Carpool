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
        //public string OAddress { get; set; }
        //public string OCity { get; set; }
        //public string OCountry { get; set; }

        //public string DAddress { get; set; }
        //public string DCity { get; set; }
        //public string DCountry { get; set; }

        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Fare { get; set; }
        public string Comment { get; set; }

    }
}