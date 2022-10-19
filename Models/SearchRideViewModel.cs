using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class SearchRideViewModel
    {
        public LocationModel Origin { get; set; }
        public LocationModel Destination { get; set; }
        public DateTime Date { get; set; }
        public int Seats { get; set; }
    }
}