using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class SearchRideViewModel
    {
        public string OAddress { get; set; }
        public string OCity { get; set; }
        public string DAddress { get; set; }
        public string DCity { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public int Seats { get; set; }
    }
}