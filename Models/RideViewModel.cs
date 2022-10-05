﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class RideViewModel
    {
        public AccountModel Driver { get; set; }
        public CarModel Car { get; set; }
        public LocationModel Origin { get; set; }
        public LocationModel Destination { get; set; }

        public DateTime DateTime { get; set; }
        public int Fare {get; set; }
    }
}