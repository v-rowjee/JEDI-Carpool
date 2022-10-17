using JEDI_Carpool.BLL;
using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JEDI_Carpool.Controllers
{
    public class BookingController : Controller
    {
        public IAccountBL AccountBL;
        public IRideBL RideBL;
        public IBookingBL BookingBL;

        public BookingController(IAccountBL AccountBL, IRideBL RideBL, IBookingBL BookingBL)
        {
            this.AccountBL = AccountBL;
            this.RideBL = RideBL;
            this.BookingBL = BookingBL;
        }
        public BookingController()
        {
            this.AccountBL = new AccountBL();
            this.RideBL = new RideBL();
            this.BookingBL = new BookingBL();
        }



        // GET: Booking
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = account;

                var driverRides = RideBL.GetAllRides().Where(r => r.Driver.AccountId.Equals(account.AccountId)).ToList();
                ViewBag.Rides = driverRides;

                //var passengerRides = RideBL.GetAllRides().Where(r => );
                //ViewBag.Bookings = passengerRides;

                return View();
            }
            return Redirect("/");

        }
    }
}