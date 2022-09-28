using JEDI_Carpool.BLL;
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
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Authenticate(LoginViewModel model)
        {
            var IsUserValid = AppUserBL.AuthenticateUser(model);
            return Json(new { result = IsUserValid, url = Url.Action("Index", "Home") });
        }
    }
}