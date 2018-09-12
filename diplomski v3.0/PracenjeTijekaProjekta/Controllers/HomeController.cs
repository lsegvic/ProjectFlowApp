using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracenjeTijekaProjekta.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*if (User.IsInRole("Admin"))
                return RedirectToAction("Index","Admin",new { area="Admin"});
            if (User.IsInRole("Student"))
                return RedirectToAction("Index", "UsersProjects", new { area = "Student" });
            else*/
                return View();

        }

       
    }
}