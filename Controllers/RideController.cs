using JEDI_Carpool.BLL;
using JEDI_Carpool.DAL;
using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace JEDI_Carpool.Controllers
{
    public class RideController : Controller
    {
        public IAccountBL AccountBL;
        public IRideBL RideBL;
        public ICarBL CarBL;
        public IBookingBL BookingBL;
        public RideController(IAccountBL AccountBL, IRideBL RideBL, ICarBL CarBL, IBookingBL BookingBL)
        {
            this.AccountBL = AccountBL;
            this.RideBL = RideBL;
            this.CarBL = CarBL;
            this.BookingBL = BookingBL;
        }
        public RideController()
        {
            this.AccountBL = new AccountBL();
            this.RideBL = new RideBL();
            this.CarBL = new CarBL();
            this.BookingBL = new BookingBL();
        }



        // GET: Ride
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = account;

                var driverRides = RideBL.GetAllRides().Where(r => r.Driver.AccountId == account.AccountId);
                ViewBag.DriverRides = driverRides;

                return View();
            }
            return Redirect("/");
        }


        // GET: Ride/Search
        public ActionResult Search(SearchRideViewModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

            var rides = RideBL.GetAllRides();

            // FILTER RIDES
            foreach (var ride in rides.ToList())
            {
                var seatsLeft = RideBL.GetSeatsLeft(ride);
                ride.SeatsLeft = seatsLeft;
            }
            rides.RemoveAll(r => r.SeatsLeft < 1);


            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = account;

                rides.Where(r => r.Driver.Address.Country == account.Address.Country);

                view.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                view.MasterName = "~/Views/Shared/_GuestLayout.cshtml";
            }

            // Store filtered rides in viewbag
            ViewBag.Rides = rides;

            return view;
        }

        // GET: Ride/View/#
        public ActionResult View(int? id)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

            if(loggeduser != null)
            {
                ViewBag.Account = AccountBL.GetAccount(loggeduser);
                view.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else view.MasterName = "~/Views/Shared/_GuestLayout.cshtml";

            if (id != null)
            {
                var ride = RideBL.GetRide(id);
                if (ride != null)
                {
                    ride.SeatsLeft = RideBL.GetSeatsLeft(ride);
                    ViewBag.Ride = ride;

                    var bookings = BookingBL.GetBookingsByRideId(ride.RideId);
                    ViewBag.Bookings = bookings;

                    return view;
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        // GET: Ride/Share
        public ViewResult Share()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                var car = CarBL.GetCar(account.AccountId);
                ViewBag.Account = account;
                ViewBag.Car = car;

                if(car == null)
                {
                    ViewBag.Message = "You do not have a registered car. Please add your car details.";
                }

                view.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                ViewBag.Message = "You are not currently logged in. Please sign in to share ride.";
                view.MasterName = "~/Views/Shared/_GuestLayout.cshtml";
            }

            return view;
        }


        // POST: Ride/Share
        [HttpPost]
        public JsonResult Share(ShareRideViewModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                model.DriverId = account.AccountId;

                var result = RideBL.Share(model);

                if (result == "Success")
                {
                    return Json(new { result = result, url = Url.Action("Shared", "Booking") });
                }
                else if (result == "NoCar") 
                { 
                    return Json(new { result = result, url = Url.Action("Create", "Car") }); 
                }
                else return Json(new { result = "Error" });
            }
            else
            {
                return Json(new { result = "NoUser", url = Url.Action("Index", "Login") });
            }
        }


        // GET: Ride/Edit
        public ActionResult Edit(int id)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                var ride = RideBL.GetRide(id);

                if(account.AccountId == ride.Driver.AccountId)
                {
                    ViewBag.Account = account;
                    ViewBag.Ride = ride;
                }
                else RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}