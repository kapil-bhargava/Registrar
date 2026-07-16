using Regis.Models;
using Regis.Services;
using System.Collections.Generic;
using System.Web.Mvc;

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
            // Call Service Method
            List<UniversityTypeModel> universityTypeList =
                universityTypeService.GetAllUniversityTypes();

            // Send data to View
            return View(universityTypeList);
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
            // Check whether the form data is valid
            if (ModelState.IsValid)
            {
                bool result = universityTypeService.InsertUniversityType(model);

                if (result)
                {
                    TempData["Success"] = "University Type Saved Successfully.";
                }
                else
                {
                    TempData["Error"] = "Unable to Save University Type.";
                }
            }

            // Reload the page with updated data
            return RedirectToAction("UniversityType");
        }


        // ============================================================
        // Category Master
        // ============================================================

        public ActionResult CategoryMaster()
        {
            return View();
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

        public ActionResult SystemSettings()
        {
            return View();
        }
    }
}