using JEDI_Carpool.BLL;
using JEDI_Carpool.DAL.Common;
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
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                return Redirect("Home/Index");
            }
            return View();
        }

        [HttpPost]
        public JsonResult Register(RegisterViewModel model)
        {
            var result = AppUserBL.RegisterUser(model);
            
            if (result == "Success")
            {
                return Json(new { result = result, url = Url.Action("Index", "Login") });
            }
            else if (result == "DuplicatedEmail")
            {
                return Json(new { result = result, url = Url.Action("Index", "Login") });
            }
            else
            {
                return Json(new { result = "Error" });
            }
        }



    }
}