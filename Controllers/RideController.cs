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
        // GET: Ride
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var homeView = View();

            if (loggeduser != null)
            {
                var data = AccountBL.GetAccountDetails(loggeduser);
                ViewBag.Account = data;

                homeView.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                homeView.MasterName = "~/Views/Shared/_GuestLayout.cshtml";
            }

            return homeView;
        }

        public ViewResult Share()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var homeView = View();

            if (loggeduser != null)
            {
                var data = AccountBL.GetAccountDetails(loggeduser);
                ViewBag.Account = data;

                homeView.MasterName = "~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                homeView.MasterName = "~/Views/Shared/_GuestLayout.cshtml";
            }

            return homeView;
        }

        [HttpPost]
        public JsonResult Share(ShareRideViewModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
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

    }
}