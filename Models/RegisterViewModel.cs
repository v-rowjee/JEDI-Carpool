﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public LocationModel Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}