using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Regis.Controllers
{
    [Regis.Filters.AuthFilter]
    public class DashboardController : Controller//
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
    
        public ActionResult GenerateStudentLogins()
        {
            var service = new Regis.Services.StudentLoginService();
            int count = service.GenerateMissingLoginsForConfirmedStudents();
            return Content($"{count} student logins generated successfully! Username & Password = StudentId for each.");
        }
    }
}