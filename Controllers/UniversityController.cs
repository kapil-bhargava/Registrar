using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class UniversityController : Controller
    {
      

        public ActionResult Index()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult UniversityForm()
        {
            return View();
        }
       
        public ActionResult AcademicSession()
        {
            return View();
        }
        public ActionResult UniversityProfile()
        {
            return View();
        }
       
    }
}