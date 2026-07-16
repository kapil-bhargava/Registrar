using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class StudentMasterController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StudentRecords()
        {
            return View();
        }

        // Student Mapping
        public ActionResult StudentMapping()
        {
            return View();
        }

        // Identity Generation
        public ActionResult IdentityGeneration()
        {
            return View();
        }

        // Academic Progress
        public ActionResult AcademicProgress()
        {
            return View();
        }

        // Certificate Management
        public ActionResult CertificateManagement()
        {
            return View();
        }

        // Alumni
        public ActionResult Alumni()
        {
            return View();
        }
    }
}