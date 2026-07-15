using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class SeatManagementController : Controller
    {
        // GET: SeatManagementt
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SeatMatrix()
        {
            return View();
        }
        public ActionResult CategorywiseSeats()
        {
            return View();
        }
        public ActionResult SeatAllotment()
        {
            return View();
        }
    }
}