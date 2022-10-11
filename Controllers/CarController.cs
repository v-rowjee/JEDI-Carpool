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
    public class CarController : Controller
    {
        // GET: Car
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var accountData = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = accountData;

                var carData = AccountBL.GetCar(loggeduser);
                if (carData == null)
                {
                    return RedirectToAction("Add");
                }
                else
                {
                    ViewBag.Account.Car = carData;
                    return View();
                }

            }
            else return Redirect("/");
        }

        // GET: Car/Add
        public ActionResult Add()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var accountData = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = accountData;

                var carData = AccountBL.GetCar(loggeduser);
                if (carData != null)
                {
                    return RedirectToAction("Index");
                }
                else return View();
                
            }
            else return Redirect("/");
        }

        //POST: Car/Add
        [HttpPost]
        public JsonResult Create(CarModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var account = AccountBL.GetAccount(loggeduser);

            model.DriverId = account.AccountId;

            var result = CarBL.Create(model);

            return Json(new { result = result, url = Url.Action("Index", "Car") });
        }

        // GET: Car/Edit
        public ViewResult Edit()
        {
            return View();
        }

        //POST: Car/Modify
        [HttpPost]
        public JsonResult Modify()
        {
            return Json(new { result = false });
        }

    }
}