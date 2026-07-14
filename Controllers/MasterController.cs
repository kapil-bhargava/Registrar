using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CategoryMaster()
        {
            return View();
        }
        public ActionResult NationalityMaster()
        {
            return View();
        }
        public ActionResult BoardUniversityMaster()
        {
            return View();
        }
        public ActionResult CountryStateCityMaster()
        {
            return View();
        }
        public ActionResult FeeheadMaster()
        {
            return View();
        }
        public ActionResult DiscountScholarshipMaster()
        {
            return View();
        }

        public ActionResult DesignationMaster()
        {
            return View();
        }

        public ActionResult SystemSettings()
        {
            return View();
        }
    }
}