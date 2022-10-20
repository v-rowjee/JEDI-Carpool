using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class SearchRideViewModel
    {
        public string RegionFrom { get; set; }
        public string CityFrom { get; set; }
        public string RegionTo { get; set; }
        public string CityTo { get; set; }

    }
}