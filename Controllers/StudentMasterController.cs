using Regis.Models;
using Regis.Services;
using System;
using System.Web.Mvc;

namespace Regis.Controllers
{
    [Regis.Filters.AuthFilter]
    public class StudentMasterController : Controller
    {
        private readonly StudentMasterService studentMasterService = new StudentMasterService();

        // GET: Default
        public ActionResult Index()
        {
            return RedirectToAction("StudentRecords");
        }

        // =========================================================
        // 1) STUDENT RECORDS (master source)
        // URL : /StudentMaster/StudentRecords
        // =========================================================

        public ActionResult StudentRecords()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllStudentRecords()
        {
            var list = studentMasterService.GetAllStudentRecords();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStudentRecordById(string studentId)
        {
            var model = studentMasterService.GetStudentRecordById(studentId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStudentStatus(string studentId, string status)
        {
            try
            {
                bool result = studentMasterService.UpdateStudentStatus(studentId, status);
                string msg = status == "Graduated"
                    ? "Status updated — student now visible in Alumni."
                    : "Status updated successfully!";
                return Json(new { success = result, message = result ? msg : "Unable to update status." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        // =========================================================
        // 2) STUDENT MAPPING (Section + Semester)
        // URL : /StudentMaster/StudentMapping
        // =========================================================

        public ActionResult StudentMapping()
        {
            ViewBag.ActiveMenu = "StudentMaster";
            ViewData["ActiveSubMenu"] = "StudentMapping";
            return View();
        }

        [HttpGet]
        public JsonResult GetStudentMappingList()
        {
            var list = studentMasterService.GetStudentMappingList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveStudentMapping(string studentId, string section, int semester)
        {
            try
            {
                bool result = studentMasterService.SaveStudentMapping(studentId, section, semester);
                return Json(new { success = result, message = result ? "Mapping saved successfully!" : "Unable to save mapping." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        // =========================================================
        // 3) IDENTITY GENERATION (Enrollment No / ID Card)
        // URL : /StudentMaster/IdentityGeneration
        // =========================================================

        public ActionResult IdentityGeneration()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetIdentityList()
        {
            var list = studentMasterService.GetIdentityList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetIdentityDetail(string studentId)
        {
            var model = studentMasterService.GetIdentityDetail(studentId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerateIdentity(string studentId)
        {
            try
            {
                var result = studentMasterService.GenerateIdentity(studentId);
                if (result == null)
                    return Json(new { success = false, message = "Unable to generate identity." });

                return Json(new
                {
                    success = true,
                    enrollmentNo = result.EnrollmentNo,
                    isNew = result.IsNew,
                    message = result.IsNew
                        ? "ID Card generated! Enrollment No: " + result.EnrollmentNo
                        : "ID Card already exists. Enrollment No: " + result.EnrollmentNo
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        // =========================================================
        // 4) ACADEMIC PROGRESS
        // URL : /StudentMaster/AcademicProgress
        // =========================================================

        public ActionResult AcademicProgress()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAcademicProgressList()
        {
            var list = studentMasterService.GetAcademicProgressList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStudentsForProgress()
        {
            var list = studentMasterService.GetStudentsForProgress();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAcademicProgress(AcademicProgressModel model)
        {
            try
            {
                int id = studentMasterService.InsertAcademicProgress(model);
                if (id == 0)
                    return Json(new { success = false, message = "Unable to save progress record." });

                return Json(new { success = true, message = "Progress record saved!", progressId = id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        // =========================================================
        // 5) CERTIFICATE MANAGEMENT
        // URL : /StudentMaster/CertificateManagement
        // =========================================================

        public ActionResult CertificateManagement()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCertificateList()
        {
            var list = studentMasterService.GetCertificateList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStudentsForCertificate()
        {
            var list = studentMasterService.GetStudentsForCertificate();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IssueCertificate(CertificateIssuedModel model)
        {
            try
            {
                string certNo = studentMasterService.IssueCertificate(model);
                if (certNo == null)
                    return Json(new { success = false, message = "Unable to issue certificate." });

                return Json(new { success = true, message = "Certificate " + certNo + " generated!", certNo = certNo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        // =========================================================
        // 6) ALUMNI
        // URL : /StudentMaster/Alumni
        // =========================================================

        public ActionResult Alumni()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAlumniList()
        {
            var list = studentMasterService.GetAlumniList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAlumniInfoById(string studentId)
        {
            var model = studentMasterService.GetAlumniInfoById(studentId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAlumniInfo(AlumniInfoModel model)
        {
            try
            {
                bool result = studentMasterService.SaveAlumniInfo(model);
                return Json(new { success = result, message = result ? "Alumni info updated!" : "Unable to save alumni info." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}