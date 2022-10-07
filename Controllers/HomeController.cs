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
    public class HomeController : Controller
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

        public ActionResult Logout()
        {
            Session.Clear();
            ViewData.Clear();
            return RedirectToAction("Index");
        }

    }
}