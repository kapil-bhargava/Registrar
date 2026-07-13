using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class AcademicSetupController : Controller
    {
        // GET: AcademicSetup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AcademicSession()
        {
            return View();
        }
        public ActionResult DepartmentManagement()
        {
            return View();
        }
        public ActionResult HODManagement()
        {
            return View();
        }
    }
}