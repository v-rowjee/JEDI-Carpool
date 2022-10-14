using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        public CarModel Car { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public LocationModel Address { get; set; }
    }
}