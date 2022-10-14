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
        public RideController(IAccountBL AccountBL, IRideBL RideBL, ICarBL CarBL)
        {
            this.AccountBL = AccountBL;
            this.RideBL = RideBL;
            this.CarBL = CarBL;
        }
        public RideController()
        {
            this.AccountBL = new AccountBL();
            this.RideBL = new RideBL();
            this.CarBL = new CarBL();
        }



        // GET: Ride
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

            var rides = RideBL.GetAllRides();
            ViewBag.Rides = rides;

            //var seatsTaken = 2;
            //rides.Where(r => r.Car.Seat > )

            // FILTER RIDES


            if (loggeduser != null)
            {
                var data = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = data;

                view.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                view.MasterName = "~/Views/Shared/_GuestLayout.cshtml";
            }

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
                    ViewBag.Ride = ride;

                    var passengers = RideBL.GetPassengers(id);
                    ViewBag.Passengers = passengers;

                    return view;
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: Ride/Book
        public JsonResult Book(BookingModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if(loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                var ride = RideBL.GetRide(model.Ride.RideId);

                model.Passenger = account;
                model.Ride = ride;

                var result = RideBL.BookRide(model);

                return Json(new { result = "Success", url = Url.Action("Index", "Home") });
            }
            else return Json(new { result = "NoUser", url = Url.Action("Index", "Login") });
            
        }

        //[HttpPost]
        //public JsonResult Search(SearchRideViewModel model)
        //{
        //    var rides = RideBL.GetAllRides();

        //    // use of linq to filter

        //    ViewBag.Rides = rides;

        //    if (rides != null || rides.Count > 0)
        //    {
        //        return Json(new { result = true, count = rides.Count });
        //    }
        //    else
        //    {
        //        return Json(new { result = false });
        //    }
        //}


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
                    return Json(new { result = result, url = Url.Action("Index", "Home") });
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

    }
}