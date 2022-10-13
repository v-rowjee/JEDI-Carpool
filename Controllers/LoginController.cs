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
    public class LoginController : Controller
    {
        public IAppUserBL AppUserBL;

        public LoginController(IAppUserBL AppUserBL)
        {
            this.AppUserBL = AppUserBL;
        }

        public LoginController()
        {
            this.AppUserBL = new AppUserBL();
        }


        // GET: Login
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return Redirect("Home/Index");
            }
            return View();
        }

        // POST: Login/Authenticate
        [HttpPost]
        public JsonResult Authenticate(LoginViewModel model)
        {
            var IsUserValid = AppUserBL.AuthenticateUser(model);
            if (IsUserValid)
            {
                this.Session["CurrentUser"] = model;
            }
            return Json(new { result = IsUserValid, url = Url.Action("Index", "Home") });

        }
    }
}