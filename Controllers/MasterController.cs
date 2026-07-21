using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;
using Regis.Models;
using Regis.Services;
using Regis.Views.Master;

namespace Regis.Controllers
{
    public class MasterController : Controller
    {
        // ============================================================
        // Create an object of the Service class.
        // The controller will use this object to communicate
        // with the database through the Service layer.
        // ============================================================

        private readonly UniversityTypeService universityTypeService = new UniversityTypeService();
        private readonly CategoryService categoryService = new CategoryService();
        private readonly CampusCategoryService campusCategoryService = new CampusCategoryService();
        private readonly SessionTypeService sessionTypeService = new SessionTypeService();
        private readonly MasterService MasterService = new MasterService();
        // ============================================================
        // Dashboard / Master Home Page
        // URL : /Master/Index
        // ============================================================

        public ActionResult Index()
        {
            return View();
        }

        // ============================================================
        // University Type Master
        // URL : /Master/UniversityType
        //
        // Workflow:
        // Browser
        //     ↓
        // Controller
        //     ↓
        // UniversityTypeService
        //     ↓
        // Stored Procedure
        //     ↓
        // SQL Server
        //     ↓
        // List<UniversityTypeModel>
        //     ↓
        // Controller
        //     ↓
        // View
        // ============================================================

        public ActionResult UniversityType()
        {
            List<UniversityTypeModel> universityTypeList =
                universityTypeService.GetAllUniversityTypes();

            return View(universityTypeList);
        }

      

        public ActionResult DeleteUniversityType(int id)
        {
            bool result = universityTypeService.DeleteUniversityType(id);
            TempData[result ? "Success" : "Error"] =
                result ? "University Type Deleted Successfully." : "Unable to Delete University Type.";

            return RedirectToAction("UniversityType");
        }

        // ============================================================
        // University Type Master (POST)
        //
        // Purpose:
        // Receives the form data from UniversityType.cshtml
        // Saves it into the database
        // ============================================================

        [HttpPost]
        public ActionResult UniversityType(UniversityTypeModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;

                if (model.UniversityTypeId > 0)
                {
                    result = universityTypeService.UpdateUniversityType(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "University Type Updated Successfully." : "Unable to Update University Type.";
                }
                else
                {
                    result = universityTypeService.InsertUniversityType(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "University Type Saved Successfully." : "Unable to Save University Type.";
                }
            }

            return RedirectToAction("UniversityType");
        }

        // ============================================================
        // Category Master
        // URL : /Master/CategoryMaster
        // ============================================================

        public ActionResult CategoryMaster()
        {
            List<CategoryModel> categoryList = categoryService.GetAllCategories();
            return View(categoryList);
        }
        [HttpPost]
        public ActionResult CategoryMaster(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;

                if (model.CategoryId > 0)
                {
                    result = categoryService.UpdateCategory(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Category Updated Successfully." : "Unable to Update Category.";
                }
                else
                {
                    result = categoryService.InsertCategory(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Category Saved Successfully." : "Unable to Save Category.";
                }
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                TempData["Error"] = "Validation Failed: " + errors;
            }

            return RedirectToAction("CategoryMaster");
        }
        public ActionResult DeleteCategory(int id)
        {
            bool result = categoryService.DeleteCategory(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Category Deleted Successfully." : "Unable to Delete Category.";

            return RedirectToAction("CategoryMaster");
        }
        // ============================================================
        // Nationality Master
        // ============================================================

        public ActionResult NationalityMaster()
        {
            return View();
        }

        // ============================================================
        // Board / University Master
        // ============================================================

        public ActionResult BoardUniversityMaster()
        {
            return View();
        }

        // ============================================================
        // Country / State / City Master
        // ============================================================

        public ActionResult CountryStateCityMaster()
        {
            return View();
        }

        // ============================================================
        // Fee Head Master
        // ============================================================

        public ActionResult FeeheadMaster()
        {
            return View();
        }

        // ============================================================
        // Discount / Scholarship Master
        // ============================================================

        public ActionResult DiscountScholarshipMaster()
        {
            return View();
        }

        // ============================================================
        // Designation Master
        // ============================================================

        public ActionResult DesignationMaster()
        {
            return View();
        }

        // ============================================================
        // Religion Master
        // ============================================================

        public ActionResult ReligionMaster()
        {
            return View();
        }

        // ============================================================
        // Blood Group Master
        // ============================================================

        public ActionResult BloodGroupMaster()
        {
            return View();
        }

        // ============================================================
        // Marital Status Master
        // ============================================================

        public ActionResult MaritalStatusMaster()
        {
            return View();
        }

        // ============================================================
        // Document Type Master
        // ============================================================

        public ActionResult DocumentTypeMaster()
        {
            return View();
        }



        // ============================================================
        // Program Type Master
        // URL : /Master/ProgramTypeMaster
        //
        // Feeds the "Program Type" dropdown on Program Management
        // (AcademicSetupController.ProgramManagement). Same GETALL /
        // INSERT / UPDATE / DELETE flag pattern as every other master.
        // ============================================================

        // ============================================================
        // FACULTY MASTER
        // URL : /Master/FacultyMaster
        // ============================================================

        public ActionResult FacultyMaster()
        {
            List<FacultyMasterModel> list = MasterService.GetAllFacultyMaster();
            return View(list);
        }

        [HttpPost]
        public ActionResult FacultyMaster(FacultyMasterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.FacultyId > 0)
                {
                    result = MasterService.UpdateFacultyMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Faculty Master Updated Successfully." : "Unable to Update Faculty Master.";
                }
                else
                {
                    result = MasterService.InsertFacultyMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Faculty Master Saved Successfully." : "Unable to Save Faculty Master.";
                }
            }
            return RedirectToAction("FacultyMaster");
        }

        public ActionResult DeleteFacultyMaster(int id)
        {
            bool result = MasterService.DeleteFacultyMaster(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Faculty Master Deleted Successfully." : "Unable to Delete Faculty Master.";
            return RedirectToAction("FacultyMaster");
        }

        // ============================================================
        // DEPARTMENT MASTER
        // URL : /Master/DepartmentMaster
        // ============================================================

        public ActionResult DepartmentMaster()
        {
            List<DepartmentMasterModel> list = MasterService.GetAllDepartmentMaster();
            ViewBag.Faculties = MasterService.GetActiveFacultyMaster();
            return View(list);
        }

        [HttpPost]
        public ActionResult DepartmentMaster(DepartmentMasterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.DepartmentId > 0)
                {
                    result = MasterService.UpdateDepartmentMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Department Master Updated Successfully." : "Unable to Update Department Master.";
                }
                else
                {
                    result = MasterService.InsertDepartmentMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Department Master Saved Successfully." : "Unable to Save Department Master.";
                }
            }
            return RedirectToAction("DepartmentMaster");
        }

        public ActionResult DeleteDepartmentMaster(int id)
        {
            bool result = MasterService.DeleteDepartmentMaster(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Department Master Deleted Successfully." : "Unable to Delete Department Master.";
            return RedirectToAction("DepartmentMaster");
        }

        // Cascading dropdown: Faculty selected -> only that Faculty's Departments
        public JsonResult GetDepartmentMasterByFaculty(int facultyId)
        {
            var list = MasterService.GetDepartmentMasterByFaculty(facultyId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // ============================================================
        // PROGRAM MASTER
        // URL : /Master/ProgramMaster
        // ============================================================

        public ActionResult ProgramMaster()
        {
            List<ProgramMasterModel> list = MasterService.GetAllProgramMaster();
            return View(list);
        }

        [HttpPost]
        public ActionResult ProgramMaster(ProgramMasterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.ProgramId > 0)
                {
                    result = MasterService.UpdateProgramMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Program Master Updated Successfully." : "Unable to Update Program Master.";
                }
                else
                {
                    result = MasterService.InsertProgramMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Program Master Saved Successfully." : "Unable to Save Program Master.";
                }
            }
            return RedirectToAction("ProgramMaster");
        }

        public ActionResult DeleteProgramMaster(int id)
        {
            bool result = MasterService.DeleteProgramMaster(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Program Master Deleted Successfully." : "Unable to Delete Program Master.";
            return RedirectToAction("ProgramMaster");
        }

        // ============================================================
        // COURSE MASTER
        // URL : /Master/CourseMaster
        // ============================================================

        public ActionResult CourseMaster()
        {
            List<CourseMasterModel> list = MasterService.GetAllCourseMaster();
            ViewBag.Programs = MasterService.GetActiveProgramMaster();
            ViewBag.Departments = MasterService.GetActiveDepartmentMaster();
            return View(list);
        }
        [HttpPost]
        public ActionResult CourseMaster(CourseMasterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.CourseId > 0)
                {
                    result = MasterService.UpdateCourseMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Course Master Updated Successfully." : "Unable to Update Course Master.";
                }
                else
                {
                    result = MasterService.InsertCourseMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Course Master Saved Successfully." : "Unable to Save Course Master.";
                }
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["Error"] = "Validation Failed: " + errors;
            }

            return RedirectToAction("CourseMaster");
        }

        public ActionResult DeleteCourseMaster(int id)
        {
            bool result = MasterService.DeleteCourseMaster(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Course Master Deleted Successfully." : "Unable to Delete Course Master.";
            return RedirectToAction("CourseMaster");
        }

        // Cascading dropdown: Department selected -> only that Department's Courses
        public JsonResult GetCourseMasterByDepartment(int departmentId)
        {
            var list = MasterService.GetCourseMasterByDepartment(departmentId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // ============================================================
        // SEMESTER MASTER
        // URL : /Master/SemesterMaster
        // ============================================================

        public ActionResult SemesterMaster()
        {
            List<SemesterMasterModel> list = MasterService.GetAllSemesterMaster();
            ViewBag.Courses = MasterService.GetActiveCourseMaster();
            ViewBag.SessionSuggestions = MasterService.GetDistinctAcademicSessionNames();  // NEW
            return View(list);
        }

        [HttpPost]
        public ActionResult SemesterMaster(SemesterMasterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.SemesterId > 0)
                {
                    result = MasterService.UpdateSemesterMaster(model);
                    TempData[result ? "Success" : "Error"] = result ? "Semester Master Updated Successfully." : "Unable to Update Semester Master.";
                }
                else
                {
                    result = MasterService.InsertSemesterMaster(model);
                    TempData[result ? "Success" : "Error"] = result ? "Semester Master Saved Successfully." : "Unable to Save Semester Master.";
                }
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = "Validation Failed: " + errors;
            }
            return RedirectToAction("SemesterMaster");
        }

        public ActionResult DeleteSemesterMaster(int id)
        {
            bool result = MasterService.DeleteSemesterMaster(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Semester Master Deleted Successfully." : "Unable to Delete Semester Master.";
            return RedirectToAction("SemesterMaster");
        }

        // Cascading dropdown: Course selected -> only that Course's Semesters
        public JsonResult GetSemesterMasterByCourse(int courseId)
        {
            var list = MasterService.GetSemesterMasterByCourse(courseId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // ============================================================
        // SUBJECT MASTER
        // URL : /Master/SubjectMaster
        // ============================================================

        public ActionResult SubjectMaster()
        {
            List<SubjectMasterModel> list = MasterService.GetAllSubjectMaster();
            ViewBag.Courses = MasterService.GetActiveCourseMaster();
            return View(list);
        }

        [HttpPost]
        public ActionResult SubjectMaster(SubjectMasterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;
                if (model.SubjectId > 0)
                {
                    result = MasterService.UpdateSubjectMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Subject Master Updated Successfully." : "Unable to Update Subject Master.";
                }
                else
                {
                    result = MasterService.InsertSubjectMaster(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Subject Master Saved Successfully." : "Unable to Save Subject Master.";
                }
            }
            return RedirectToAction("SubjectMaster");
        }

        public ActionResult DeleteSubjectMaster(int id)
        {
            bool result = MasterService.DeleteSubjectMaster(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Subject Master Deleted Successfully." : "Unable to Delete Subject Master.";
            return RedirectToAction("SubjectMaster");
        }

        // ============================================================
        // System Settings
        // ============================================================
        public ActionResult CampusCategoryMaster()
        {
            List<CampusCategoryModel> list = campusCategoryService.GetAllCampusCategories();
            return View(list);
        }

        [HttpPost]
        public ActionResult CampusCategoryMaster(CampusCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;

                if (model.CampusCategoryId > 0)
                {
                    result = campusCategoryService.UpdateCampusCategory(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Campus Category Updated Successfully." : "Unable to Update Campus Category.";
                }
                else
                {
                    result = campusCategoryService.InsertCampusCategory(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Campus Category Saved Successfully." : "Unable to Save Campus Category.";
                }
            }

            return RedirectToAction("CampusCategoryMaster");
        }

        public ActionResult DeleteCampusCategory(int id)
        {
            bool result = campusCategoryService.DeleteCampusCategory(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Campus Category Deleted Successfully." : "Unable to Delete Campus Category.";

            return RedirectToAction("CampusCategoryMaster");
        }
        // ============================================================
        // Session Type Master
        // URL : /Master/SessionTypeMaster
        //
        // This master feeds the "Session Type" dropdown on the
        // Academic Session page (AcademicController.AcademicSession).
        // Same GETALL/INSERT/UPDATE/DELETE flag pattern as every
        // other master above.
        // ============================================================

        public ActionResult SessionTypeMaster()
        {
            List<SessionTypeModel> list = sessionTypeService.GetAllSessionTypes();
            return View(list);
        }

        [HttpPost]
        public ActionResult SessionTypeMaster(SessionTypeModel model)
        {
            if (ModelState.IsValid)
            {
                bool result;

                if (model.SessionTypeId > 0)
                {
                    result = sessionTypeService.UpdateSessionType(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Session Type Updated Successfully." : "Unable to Update Session Type.";
                }
                else
                {
                    result = sessionTypeService.InsertSessionType(model);
                    TempData[result ? "Success" : "Error"] =
                        result ? "Session Type Saved Successfully." : "Unable to Save Session Type.";
                }
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                TempData["Error"] = "Validation Failed: " + errors;
            }

            return RedirectToAction("SessionTypeMaster");
        }

        public ActionResult DeleteSessionType(int id)
        {
            bool result = sessionTypeService.DeleteSessionType(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Session Type Deleted Successfully." : "Unable to Delete Session Type.";

            return RedirectToAction("SessionTypeMaster");
        }
    
        public ActionResult SystemSettings()
        {
            return View();
        }
    }
}