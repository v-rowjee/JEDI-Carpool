using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class BookingModel
    {
        public int BookingId { get; set; }
        public RideViewModel Ride { get; set; }
        public AccountModel Passenger { get; set; }
        public int Seat { get; set; }
    }
}