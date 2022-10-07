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
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var data = AccountBL.GetAccount(loggeduser);
                ViewBag.account = data;
                return View();
            }
            else return Redirect("/");
        }

        public ActionResult Car()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var accountData = AccountBL.GetAccount(loggeduser);

                var carData = AccountBL.GetCar(loggeduser);
                if(carData != null) accountData.Car = carData;
                else accountData.Car = null;

                ViewBag.Account = accountData;
                
                if(ViewBag.Account.Car != null) return View();
                else return RedirectToAction("New_Car");
            }
            else return Redirect("/");
            
        }

        public ActionResult New_Car()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var data = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = data;

                if (ViewBag.Account.Car != null) return RedirectToAction("Car");
                else return View();
            }
            else return Redirect("/");
        }

    }
}