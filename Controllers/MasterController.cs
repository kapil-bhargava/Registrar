using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Regis.Models;
using Regis.Services;

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