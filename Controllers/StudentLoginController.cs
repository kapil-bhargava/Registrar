// Controllers/StudentController.cs
using System;
using System.Web.Mvc;
using Regis.Services;

namespace Regis.Controllers
{
    // GlobalRoleAuthFilter automatically protect karta hai — alag attribute ki zaroorat nahi.
    public class StudentLoginController : Controller
    {
        private readonly AdmissionService admissionService = new AdmissionService();

        // URL: /Student/Dashboard
        public ActionResult StudentDashboard()
        {
            int applicationId = Convert.ToInt32(Session["StudentApplicationId"]);
            var model = admissionService.GetStudentProfile(applicationId);
            return View(model);
        }

        // URL: /Student/Profile
        public ActionResult Profile()
        {
            int applicationId = Convert.ToInt32(Session["StudentApplicationId"]);
            var model = admissionService.GetStudentProfile(applicationId);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetMyProfile()
        {
            int applicationId = Convert.ToInt32(Session["StudentApplicationId"]);
            var model = admissionService.GetStudentProfile(applicationId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMyDocuments()
        {
            int applicationId = Convert.ToInt32(Session["StudentApplicationId"]);
            var list = admissionService.GetSubmittedDocuments(applicationId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}