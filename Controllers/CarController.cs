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
                var account = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = account;

                var car = CarBL.GetCar(account.AccountId);
                if (car == null)
                {
                    return RedirectToAction("Add");
                }
                else
                {
                    ViewBag.Account.Car = car;
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
                var account = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = account;

                var car = CarBL.GetCar(account.AccountId);
                if (car != null)
                {
                    return RedirectToAction("Index");
                }
                else return View();
                
            }
            else return Redirect("/");
        }

        //POST: Car/Create
        [HttpPost]
        public JsonResult Create(CarModel carModel) // Cannot assign object name to model as ther already is a property in class CarModel called Model => conflict
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var account = AccountBL.GetAccount(loggeduser);

            carModel.DriverId = account.AccountId;

            var result = CarBL.Create(carModel);

            return Json(new { result = result, url = Url.Action("Index", "Car") });
        }

        // GET: Car/Edit
        public ActionResult Edit()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var account = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = account;

                var car = CarBL.GetCar(account.AccountId);
                if (car == null)
                {
                    return RedirectToAction("Add");
                }
                else
                {
                    ViewBag.Account.Car = car;
                    return View();
                }

            }
            else return Redirect("/");
        }

        //POST: Car/Edit
        [HttpPost]
        public JsonResult Edit(CarModel carModel)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var account = AccountBL.GetAccount(loggeduser);

            carModel.DriverId = account.AccountId;

            var result = CarBL.Edit(carModel);

            return Json(new { result = result, url = Url.Action("Index", "Car") });
        }


        [HttpPost]
        public JsonResult Delete()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var account = AccountBL.GetAccount(loggeduser);

            var result = CarBL.Delete(account.AccountId);

            return Json(new { result = result, url = Url.Action("Add", "Car") });
        }


    }
}