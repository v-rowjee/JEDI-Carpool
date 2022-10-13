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
        public IAccountBL AccountBL;
        public ProfileController(IAccountBL AccountBL)
        {
            this.AccountBL = AccountBL;
        }
        public ProfileController()
        {
            this.AccountBL = new AccountBL();
        }



        // GET: Profile
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var data = AccountBL.GetAccount(loggeduser);
                ViewBag.account = data;
                return View();
            }
            else return Redirect("/");
        }

        // GET: Profile/Edit
        public ActionResult Edit()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var data = AccountBL.GetAccount(loggeduser);
                ViewBag.account = data;
                return View();
            }
            else return Redirect("/");
        }

        // POST: Profile/Edit
        [HttpPost]
        public JsonResult Edit(AccountModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var account = AccountBL.GetAccount(loggeduser);

            model.AccountId = account.AccountId;

            var result = AccountBL.UpdateAccount(model);

            if (result == "Success")
            {
                return Json(new { result = result, url = Url.Action("Index", "Profile") });
            }
            else if (result == "DuplicatedEmail")
            {
                return Json(new { result = result });
            }
            else
            {
                return Json(new { result = "NoUpdate" });
            }

        }

    }
}