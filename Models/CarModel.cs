using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class CarModel
    {
        public int CarId { get; set; }
        public int DriverId { get; set; }
        public string PlateNumber { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Seat { get; set; }
        public string Color { get; set; }
    }
}