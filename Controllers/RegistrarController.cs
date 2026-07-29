using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Regis.Models;
using Regis.Services;

namespace Regis.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly RegistrarService service = new RegistrarService();
        private readonly MasterService masterService = new MasterService();
        private readonly CategoryService categoryService = new CategoryService();   // add this field

        // GET: Registrar
        public ActionResult Index()
        {
            return View();
        }

        // ============================================================
        // REGISTRAR STUDENT LIST + FORM
        // URL : /Registrar/RegistrarStudentList
        // Single page, List <-> Form toggle (same pattern as
        // BranchMaster / SemesterMaster / RequiredDocumentMaster).
        // Course dropdown comes from Academic Setup (Course Master).
        // Documents-required-for-course comes from Required Document
        // Master, resolved by CourseId in the service layer.
        // ============================================================

        public ActionResult RegistrarStudentList()
        {
            List<RegistrarStudentModel> list = service.GetAllRegistrarStudents();

            ViewBag.Courses = masterService.GetActiveCourseMaster();
            ViewBag.Branches = masterService.GetActiveBranchMaster();
            ViewBag.Semesters = masterService.GetAllSemesterMaster();
            ViewBag.Categories = categoryService.GetActiveCategories();

            return View(list);
        }

        [HttpPost]
        public ActionResult RegistrarStudentList(RegistrarStudentModel model, string SelectedDocumentIds, string RequiredDocumentIdsCsv)
        {
            // These two come from the "Proceed to Documents" popup:
            // - RequiredDocumentIdsCsv: every document that WAS shown in the checklist
            // - SelectedDocumentIds   : only the ones the Registrar ticked (i.e. submitted)
            model.SubmittedDocumentIdsCsv = SelectedDocumentIds ?? "";
            model.RequiredDocumentIdsCsv = RequiredDocumentIdsCsv ?? "";

            if (ModelState.IsValid)
            {
                bool result;

                try
                {
                    if (model.RegistrarId > 0)
                    {
                        result = service.UpdateRegistrarStudent(model);
                        TempData[result ? "Success" : "Error"] =
                            result ? "Student record Updated Successfully." : "Unable to Update Student record.";
                    }
                    else
                    {
                        result = service.InsertRegistrarStudent(model);
                        TempData[result ? "Success" : "Error"] =
                            result ? "Student record Saved Successfully." : "Unable to Save Student record.";
                    }
                }
                catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                {
                    // 2627/2601 = SQL Server unique-constraint / unique-index violation
                    TempData["Error"] = "Duplicate Email or Mobile Number is not allowed. A student with this Email or Mobile already exists.";
                }
                catch (SqlException)
                {
                    TempData["Error"] = "A database error occurred while saving the student record. Please try again.";
                }
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["Error"] = "Validation Failed: " + errors;
            }

            return RedirectToAction("RegistrarStudentList");
        }

        public ActionResult DeleteRegistrarStudent(int id)
        {
            bool result = service.DeleteRegistrarStudent(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Student record Deleted Successfully." : "Unable to Delete Student record.";
            return RedirectToAction("RegistrarStudentList");
        }

        // Used by the "Edit" button to prefill the form (and the doc popup, if reopened)
        public JsonResult GetRegistrarStudentById(int id)
        {
            var model = service.GetRegistrarStudentById(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Course selected in the form -> which documents are required for it.
        // Called by JS when "Proceed to Documents" is clicked.
        // Course + Category selected in the form -> which documents are
        // required for that combination. Called by JS when "Proceed to
        // Documents" is clicked.
        public JsonResult GetRequiredDocumentsByCourse(int courseId, int categoryId)
        {
            var list = service.GetRequiredDocumentsByCourseAndCategory(courseId, categoryId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // ============================================================
        // REGISTRAR LIST (search/filter view)
        // URL : /Registrar/RegistrarList
        // Read-only search screen. Pulls from the SAME RegistrarStudent
        // data that the RegistrarStudentList form saves into — so a
        // record saved there shows up here immediately, no separate
        // data source. Editing still happens on RegistrarStudentList.
        // ============================================================
        public ActionResult RegistrarList()
        {
            List<RegistrarStudentModel> list = service.GetAllRegistrarStudents();

            ViewBag.Courses = masterService.GetActiveCourseMaster();
            ViewBag.Branches = masterService.GetActiveBranchMaster();
            ViewBag.Semesters = masterService.GetAllSemesterMaster();


            return View(list);
        }


        // ============================================================
        // STUDENT DETAILS (read-only profile page)
        // URL : /Registrar/StudentDetails/5
        // Reached by clicking a student's name in RegistrarStudentList.
        // ============================================================
        public ActionResult StudentDetails(int id)
        {
            var model = service.GetRegistrarStudentById(id);

            if (model == null)
            {
                TempData["Error"] = "Student record not found.";
                return RedirectToAction("RegistrarStudentList");
            }

            return View(model);
        }

    }
}