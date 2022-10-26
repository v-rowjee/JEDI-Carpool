using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JEDI_Carpool.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return RedirectToAction("NotFound");
        }

        // GET: Error/NotFound
        public ActionResult NotFound()
        {
            return View();
        }

    }
}