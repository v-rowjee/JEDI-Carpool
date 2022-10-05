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
                var data = AccountBL.GetAccountDetails(loggeduser);
                ViewBag.Account = data;

                view.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                view.MasterName = "~/Views/Shared/_GuestLayout.cshtml";
            }

            return view;
        }

        public ViewResult Share()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var view = View();

            if (loggeduser != null)
            {
                var data = AccountBL.GetAccountDetails(loggeduser);
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
        public JsonResult Share(ShareRideViewModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var account = AccountBL.GetAccountDetails(loggeduser);
                model.DriverId = account.AccountId;

                var result = RideBL.Share(model);

                if (result)
                {
                    return Json(new { result = "Success", url = Url.Action("Index", "Home") });

                }
                else
                {
                    return Json(new { result = "Error" });
                }
            }
            else
            {
                return Json(new { result = "ToLogin", url = Url.Action("Index", "Login") });
            }
        }


        [HttpPost]
        public JsonResult Search(SearchRideViewModel model)
        {
            var rides = RideBL.Search(model);
            if(rides != null)
            {
                return Json(new { result = true, data = rides });
            }
            else
            {
                return Json(new { result = false });
            }
        }

    }
}