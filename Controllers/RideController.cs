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
    public class RideController : Controller
    {
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

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

        [HttpPost]
        public JsonResult Search(SearchRideViewModel model)
        {
            var rides = RideBL.GetAllRides();
            if (rides != null)
            {
                return Json(new { result = true, data = rides });
            }
            else
            {
                return Json(new { result = false });
            }
        }

        public ViewResult Share()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                var car = CarBL.GetCar(account.AccountId);
                ViewBag.Account = account;
                ViewBag.Account.Car = car;

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