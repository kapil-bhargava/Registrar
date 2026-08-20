using Regis.Models;
using Regis.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Regis.Controllers
{
    public class MasterMappingController : Controller
    {
        private readonly MasterService MasterService = new MasterService();

        // GET: NewAdmission
        //public ActionResult Index()
        //{
        //    return RedirectToAction("PersonalInformation");
        //}

        // ============================================================
        // COURSE - BRANCH - SEMESTER MAPPING
        // URL : /MasterMapping/CourseBranchMappingSemeter
        // ============================================================

        public ActionResult CourseBranchMappingSemeter()
        {
            ViewBag.Courses = MasterService.GetActiveCourseMaster();
            List<CourseBranchSemesterMappingModel> list = MasterService.GetAllCourseBranchSemesterMapping();
            return View(list);
        }

        [HttpPost]
        public ActionResult CourseBranchMappingSemeter(int CourseId, int BranchId, List<int> SemesterIds)
        {
            bool result = MasterService.SaveCourseBranchSemesterMapping(CourseId, BranchId, SemesterIds);
            TempData[result ? "Success" : "Error"] =
                result ? "Course-Branch-Semester Mapping Saved Successfully." : "Unable to Save Mapping.";

            return RedirectToAction("CourseBranchMappingSemeter");
        }

        public ActionResult DeleteCourseBranchSemesterMapping(int id)
        {
            bool result = MasterService.DeleteCourseBranchSemesterMapping(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Mapping Deleted Successfully." : "Unable to Delete Mapping.";

            return RedirectToAction("CourseBranchMappingSemeter");
        }

        // Cascading: Course select -> uske Branches (already MasterService mein hai)
        [HttpGet]
        public JsonResult GetBranchesByCourse(int courseId)
        {
            var list = MasterService.GetBranchesByCourse(courseId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // Cascading: Course select -> uske Semesters (checkbox list ke liye)
        [HttpGet]
        public JsonResult GetSemestersByCourse(int courseId)
        {
            var list = MasterService.GetSemesterMasterByCourse(courseId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // Course+Branch select hone par already-checked semesters (edit case)
        [HttpGet]
        public JsonResult GetMappedSemesters(int courseId, int branchId)
        {
            var list = MasterService.GetMappedSemesterIdsByCourseBranch(courseId, branchId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // AllStudents jaise pages ke Course filter ke liye — sirf mapped Branches
        [HttpGet]
        public JsonResult GetBranchesByCourseMapping(int courseId)
        {
            var list = MasterService.GetMappedBranchesByCourse(courseId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // AllStudents jaise pages ke Branch filter ke liye — sirf mapped Semesters
        [HttpGet]
        public JsonResult GetSemestersByCourseBranch(int courseId, int branchId)
        {
            var list = MasterService.GetMappedSemestersByCourseBranch(courseId, branchId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}