using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Regis.Models;
using Regis.Services;

namespace Regis.Controllers
{
    public class AcademicSetupController : Controller
    {
        // ============================================================
        // One service instance handles every table in this module —
        // Faculty, Department, HOD, Program, Course, Semester, Subject,
        // AcademicSession. No other Service class is needed here.
        // ============================================================
        private readonly AcademicSetupService service = new AcademicSetupService();
        private readonly CampusCategoryService campusCategoryService = new CampusCategoryService();

        // GET: AcademicSetup
        public ActionResult Index()
        {
            return View();
        }

        // ============================================================
        // ACADEMIC SESSION
        // URL : /AcademicSetup/AcademicSession
        // Page uses AJAX (fetch) for Save/Activate/Close/Delete since
        // it's a single-page List+Wizard — see the view's <script> block.
        // ============================================================

        public ActionResult AcademicSession()
        {
            ViewBag.Sessions = service.GetAllSessions();
            ViewBag.SessionTypes = new SessionTypeService().GetActiveSessionTypes();
            return View();
        }

        [HttpPost]
        public JsonResult SaveAcademicSession(AcademicSessionModel model)
        {
            try
            {
                int newId = service.InsertSession(model);
                return Json(new { success = true, message = "Session \"" + model.SessionName + "\" saved successfully!", id = newId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ActivateAcademicSession(int id)
        {
            bool result = service.ActivateSession(id);
            return Json(new { success = result, message = result ? "Session activated! Previous active session is now locked." : "Unable to activate session." });
        }

        [HttpPost]
        public JsonResult CloseAcademicSession(int id)
        {
            bool result = service.CloseSession(id);
            return Json(new { success = result, message = result ? "Session closed and archived!" : "Unable to close session." });
        }

        // Used by the "Edit" button to prefill the wizard with existing data
        public JsonResult GetAcademicSession(int id)
        {
            var session = service.GetSessionById(id);
            return Json(session, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAcademicSession(int id)
        {
            bool result = service.DeleteSession(id);
            return Json(new { success = result, message = result ? "Session deleted." : "Unable to delete session." });
        }

        [HttpPost]
        public JsonResult UpdateAcademicSession(AcademicSessionModel model)
        {
            try
            {
                bool result = service.UpdateSession(model);
                return Json(new { success = result, message = result ? "Session updated successfully!" : "Unable to update session." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        // ============================================================
        // FACULTY MANAGEMENT
        // URL : /AcademicSetup/FacultyManagement
        // ============================================================

        public ActionResult FacultyManagement()
        {
            ViewBag.Faculties = service.GetAllFaculties();
            ViewBag.Departments = service.GetAllDepartments();
            return View();
        }

        [HttpPost]
        public ActionResult FacultyManagement(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.FacultyId > 0)
                {
                    result = service.UpdateFaculty(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Faculty Updated Successfully." : "Unable to Update Faculty.";
                }
                else
                {
                    result = service.InsertFaculty(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Faculty Saved Successfully." : "Unable to Save Faculty.";
                }
            }
            return RedirectToAction("FacultyManagement");
        }

        public ActionResult DeleteFaculty(int id)
        {
            bool result = service.DeleteFaculty(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Faculty Deleted Successfully." : "Unable to Delete Faculty.";
            return RedirectToAction("FacultyManagement");
        }

        // ============================================================
        // DEPARTMENT MANAGEMENT
        // URL : /AcademicSetup/DepartmentManagement
        // ============================================================

        public ActionResult DepartmentManagement()
        {
            List<DepartmentModel> departmentList = service.GetAllDepartments();
            ViewBag.CampusList = new SelectList(campusCategoryService.GetAllCampuses(), "CampusId", "CampusName");
            ViewBag.Faculties = service.GetActiveFaculties();
            // Department Name / Code now come from Department Master (dropdown), not free text

            ViewBag.DepartmentMasterList = new MasterService().GetActiveDepartmentMaster();
            return View(departmentList);
        }

        [HttpPost]
        public ActionResult DepartmentManagement(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.DepartmentId > 0)
                {
                    result = service.UpdateDepartment(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Department Updated Successfully." : "Unable to Update Department.";
                }
                else
                {
                    result = service.InsertDepartment(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Department Saved Successfully." : "Unable to Save Department.";
                }
            }
            return RedirectToAction("DepartmentManagement");
        }

        public ActionResult DeleteDepartment(int id)
        {
            bool result = service.DeleteDepartment(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Department Deleted Successfully." : "Unable to Delete Department.";
            return RedirectToAction("DepartmentManagement");
        }

        // Cascading dropdown: Faculty selected -> only that Faculty's Departments
        public JsonResult GetDepartmentsByFaculty(int facultyId)
        {
            var list = service.GetDepartmentsByFaculty(facultyId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // ============================================================
        // HOD MANAGEMENT
        // URL : /AcademicSetup/HODManagement
        // The view's existing JS drives everything off a "departments"
        // array — we now seed that array with real data (via ViewBag,
        // JSON-encoded) instead of the old hardcoded array.
        // ============================================================

        public ActionResult HODManagement()
        {
            ViewBag.DepartmentHODOverview = service.GetDepartmentHODOverview();
            return View();
        }

        [HttpPost]
        public JsonResult AssignHOD(HODManagementModel model)
        {
            try
            {
                bool result = service.AssignHOD(model);
                return Json(new { success = result, message = result ? "HOD assigned successfully!" : "Unable to assign HOD." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // ============================================================
        // PROGRAM MANAGEMENT
        // URL : /AcademicSetup/ProgramManagement
        // ============================================================

        public ActionResult ProgramManagement()
        {
            List<ProgramModel> programList = service.GetAllPrograms();
            return View(programList);
        }

        [HttpPost]
        public ActionResult ProgramManagement(ProgramModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.ProgramId > 0)
                {
                    result = service.UpdateProgram(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Program Updated Successfully." : "Unable to Update Program.";
                }
                else
                {
                    result = service.InsertProgram(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Program Saved Successfully." : "Unable to Save Program.";
                }
            }
            return RedirectToAction("ProgramManagement");
        }

        public ActionResult DeleteProgram(int id)
        {
            bool result = service.DeleteProgram(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Program Deleted Successfully." : "Unable to Delete Program.";
            return RedirectToAction("ProgramManagement");
        }

        // ============================================================
        // COURSE MANAGEMENT
        // URL : /AcademicSetup/CourseManagement
        // ============================================================

        public ActionResult CourseManagement()
        {
            List<CourseModel> courseList = service.GetAllCourses();
            ViewBag.Programs = service.GetActivePrograms();
            ViewBag.Departments = service.GetActiveDepartments();
            return View(courseList);
        }

        [HttpPost]
        public ActionResult CourseManagement(CourseModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.CourseId > 0)
                {
                    result = service.UpdateCourse(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Course Updated Successfully." : "Unable to Update Course.";
                }
                else
                {
                    result = service.InsertCourse(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Course Saved Successfully." : "Unable to Save Course.";
                }
            }
            return RedirectToAction("CourseManagement");
        }

        public ActionResult DeleteCourse(int id)
        {
            bool result = service.DeleteCourse(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Course Deleted Successfully." : "Unable to Delete Course.";
            return RedirectToAction("CourseManagement");
        }

        // Cascading dropdown: Department selected -> only that Department's Courses
        public JsonResult GetCoursesByDepartment(int departmentId)
        {
            var list = service.GetCoursesByDepartment(departmentId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // ============================================================
        // SEMESTER MANAGEMENT
        // URL : /AcademicSetup/SemesterManagement
        // ============================================================

        public ActionResult SemesterManagement()
        {
            List<SemesterModel> semesterList = service.GetAllSemesters();
            ViewBag.Courses = service.GetActiveCourses();
            ViewBag.Sessions = service.GetActiveSessions();
            return View(semesterList);
        }

        [HttpPost]
        public ActionResult SemesterManagement(SemesterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.SemesterId > 0)
                {
                    result = service.UpdateSemester(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Semester Updated Successfully." : "Unable to Update Semester.";
                }
                else
                {
                    result = service.InsertSemester(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Semester Saved Successfully." : "Unable to Save Semester.";
                }
            }
            return RedirectToAction("SemesterManagement");
        }

        public ActionResult DeleteSemester(int id)
        {
            bool result = service.DeleteSemester(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Semester Deleted Successfully." : "Unable to Delete Semester.";
            return RedirectToAction("SemesterManagement");
        }

        // ============================================================
        // SUBJECT MANAGEMENT
        // URL : /AcademicSetup/SubjectManagement
        // ============================================================

        public ActionResult SubjectManagement()
        {
            List<SubjectModel> subjectList = service.GetAllSubjects();
            ViewBag.Courses = service.GetActiveCourses();
            return View(subjectList);
        }

        [HttpPost]
        public ActionResult SubjectManagement(SubjectModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result;
                    if (model.SubjectId > 0)
                    {
                        result = service.UpdateSubject(model);
                        TempData[result ? "Success" : "Error"] =
                            result ? "Subject Updated Successfully." : "Unable to Update Subject.";
                    }
                    else
                    {
                        result = service.InsertSubject(model);
                        TempData[result ? "Success" : "Error"] =
                            result ? "Subject Saved Successfully." : "Unable to Save Subject.";
                    }
                }
                catch (Exception ex)
                {
                    // Ab actual DB/SP error dikhega, chup-chaap fail nahi hoga
                    TempData["Error"] = "Error saving subject: " + ex.Message;
                }
            }
            else
            {
                // Yeh missing tha — ab pata chalega konsa field invalid hai
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                TempData["Error"] = "Validation failed: " + string.Join(" | ", errors);
            }
            return RedirectToAction("SubjectManagement");
        }

        public ActionResult DeleteSubject(int id)
        {
            bool result = service.DeleteSubject(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Subject Deleted Successfully." : "Unable to Delete Subject.";
            return RedirectToAction("SubjectManagement");
        }

        // Cascading dropdown: Course selected -> only that Course's Semesters
        public JsonResult GetSemestersByCourse(int courseId)
        {
            var list = service.GetSemestersByCourse(courseId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}