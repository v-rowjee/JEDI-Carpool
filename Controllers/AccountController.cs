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
    public class AccountController : Controller
    {
        public IAccountBL AccountBL;
        public AccountController(IAccountBL AccountBL)
        {
            this.AccountBL = AccountBL;
        }
        public AccountController()
        {
            this.AccountBL = new AccountBL();
        }



        // GET: Account
        public ActionResult Index()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var data = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = data;
                return View();
            }
            else return Redirect("/");
        }

        // GET: Account/Edit
        public ActionResult Edit()
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            if (loggeduser != null)
            {
                var data = AccountBL.GetAccount(loggeduser);
                ViewBag.Account = data;
                return View();
            }
            else return Redirect("/");
        }

        // POST: Account/Edit
        [HttpPost]
        public JsonResult Edit(AccountModel model)
        {
            var loggeduser = Session["CurrentUser"] as LoginViewModel;
            var account = AccountBL.GetAccount(loggeduser);

            model.AccountId = account.AccountId;

            var result = AccountBL.UpdateAccount(model);

            if (result == "Success")
            {
                var newloggeduser = new LoginViewModel();
                newloggeduser.Email = model.Email;  // new email
                newloggeduser.Password = loggeduser.Password;   // old password
                this.Session["CurrentUser"] = newloggeduser;

                return Json(new { result = result, url = Url.Action("Index", "Account") });
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