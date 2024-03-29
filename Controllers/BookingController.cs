﻿using JEDI_Carpool.BLL;
using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

                var passengerRides = RideBL.GetRidesByPassengerId(account.AccountId);
                ViewBag.PassengerRides = passengerRides;

                var passengerBookings = BookingBL.GetBookings()
                    .Where(b => b.Passenger.AccountId == account.AccountId);
                ViewBag.PassengerBookings = passengerBookings;


                return View();
            }
            return Redirect("/");

        }

        // POST: Booking/Create
        [HttpPost]
        public JsonResult Create(BookingModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                var ride = RideBL.GetRide(model.Ride.RideId);

                model.Passenger = account;
                model.Ride = ride;

                var result = BookingBL.BookRide(model);

                if (result == "Success")
                {
                    return Json(new { result = result, url = Url.Action("Index", "Booking") });
                }
                else if (result == "NoSeat")
                {
                    return Json(new { result = result });
                }
                else if (result == "LessSeat")
                {
                    return Json(new { result = result });
                }
                else return Json(new { result = "Error" });
            }
            else return Json(new { result = "NoUser", url = Url.Action("Index", "Login") });

        }

        // POST: Booking/Delete
        [HttpPost]
        public ActionResult Delete(BookingModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;

            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);

                var booking = BookingBL.GetBookingsByRideId(model.Ride.RideId)
                    .First(b => b.Passenger.AccountId == account.AccountId);

                var result = BookingBL.DeleteBooking(booking.BookingId);

                return Json(new { result = result, url = Url.Action("Search", "Ride") });
            }
            else return Json(new { result = "NoUser", url = Url.Action("Index", "Login") });
        }
    }
}