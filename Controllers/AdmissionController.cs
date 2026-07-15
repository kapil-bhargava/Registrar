using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class AdmissionController : Controller
    {
        // GET: Admission
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdmissionSetup()
        {
            return View();
        }

        public ActionResult EligibilityCheck()
        {
            return View();
        }

        public ActionResult StudentRegistration()
        {
            return View();
        }

        public ActionResult Application()
        {
            return View();
        }

        public ActionResult DocumentVerification()
        {
            return View();
        }

        public ActionResult Counselling()
        {
            return View();
        }

        public ActionResult FeePayment()
        {
            return View();
        }

        public ActionResult Final()
        {
            return View();
        }
    }
}