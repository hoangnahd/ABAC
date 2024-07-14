using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ABAC_Fe.Models;
using static ABAC_Fe.Controllers.AccountController;
using System.Text;

namespace ABAC_Fe.Controllers
{
    public class HomeController : Controller
    {
        
        // GET: Home
        public ActionResult Index()
        {
            if (Session["AuthToken"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult Admin()
        {
            if (Session["AuthToken"] == null)
                return RedirectToAction("Login", "Account");
            if (!isSysAdmin)
                return RedirectToAction("Index", "Home");
            return View();
        }

       
    }
}