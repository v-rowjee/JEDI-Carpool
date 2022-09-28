using JEDI_Carpool.BLL;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace JEDI_Carpool.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(RegisterViewModel model)
        {
            var IsUserRegistered = AppUserBL.RegisterUser(model);
            return Json(new { result = IsUserRegistered, url = Url.Action("Index", "Login") });
        }



    }
}