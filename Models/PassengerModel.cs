using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class PassengerModel
    {
        public AccountModel Account { get; set; }
        public int Seat { get; set; }
    }
}