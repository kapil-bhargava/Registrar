using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdmissionReports()
        {
            return View();
        }

        // Seat Allotment Report
        public ActionResult SeatAllotmentReport()
        {
            return View();
        }

        // Student Master Report
        public ActionResult StudentMasterReport()
        {
            return View();
        }

        // Academic Reports
        public ActionResult AcademicReports()
        {
            return View();
        }
    }
}