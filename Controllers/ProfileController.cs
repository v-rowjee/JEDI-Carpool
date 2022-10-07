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
            return View();
        }

        public ActionResult Car()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var data = AccountBL.GetAccountDetails(loggeduser);
                ViewBag.account = data;
                return View();
            }
            else return Redirect("/");
            
        }
    }
}