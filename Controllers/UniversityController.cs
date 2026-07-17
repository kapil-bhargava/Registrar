using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Regis.Models;
using Regis.Services;

namespace Regis.Controllers
{
    public class UniversityController : Controller
    {
        private readonly CampusCategoryService campusCategoryService = new CampusCategoryService();
        private readonly UniversityProfileService universityProfileService = new UniversityProfileService();
        private readonly UniversityTypeService universityTypeService = new UniversityTypeService();

        public ActionResult Index()
        {
            UniversityProfileModel model = universityProfileService.GetUniversityProfile();

            if (model == null)
            {
                // Koi university nahi -> Empty state dikhao (Image 1)
                return View(model);
            }

            // University already configured hai -> seedha Profile page pe bhej do (Image 2)
            return RedirectToAction("UniversityProfile");
        }
        public ActionResult UniversityForm()
        {
            UniversityProfileModel model = universityProfileService.GetUniversityProfile() ?? new UniversityProfileModel { IsActive = true };

            ViewBag.UniversityTypeList = new SelectList(
                universityTypeService.GetActiveUniversityTypes(), "UniversityTypeId", "UniversityTypeName", model.UniversityTypeId);

            return View(model);
        }

        [HttpPost]
        public ActionResult UniversityForm(UniversityProfileModel model)
        {
            // Naya logo upload hua ho to save karke path model mein daal do, warna purana path retain (null bhej denge, SP khud sambhal lega)
            string uploadFolder = Server.MapPath("~/Content/Uploads/University/");
            if (!System.IO.Directory.Exists(uploadFolder))
                System.IO.Directory.CreateDirectory(uploadFolder);

            model.LogoMainPath = SaveFile(model.LogoMainFile, uploadFolder, "logo-main");
            model.LogoDarkPath = SaveFile(model.LogoDarkFile, uploadFolder, "logo-dark");
            model.LogoLightPath = SaveFile(model.LogoLightFile, uploadFolder, "logo-light");
            model.LogoSidebarPath = SaveFile(model.LogoSidebarFile, uploadFolder, "logo-sidebar");
            model.FaviconPath = SaveFile(model.FaviconFile, uploadFolder, "favicon");

            bool result = universityProfileService.SaveUniversityProfile(model);

            TempData[result ? "Success" : "Error"] =
                result ? "University Profile Saved Successfully." : "Unable to Save University Profile.";

            return RedirectToAction("UniversityProfile");
        }

        // Helper: file save karo aur relative path return karo; agar file nahi aayi to null return (purana path retain hoga)
        private string SaveFile(HttpPostedFileBase file, string folderPath, string prefix)
        {
            if (file == null || file.ContentLength == 0)
                return null;

            string fileName = prefix + "_" + DateTime.Now.Ticks + System.IO.Path.GetExtension(file.FileName);
            string fullPath = System.IO.Path.Combine(folderPath, fileName);
            file.SaveAs(fullPath);

            return "/Content/Uploads/University/" + fileName;
        }

        // ============================================================
        // University Profile Preview (read-only)
        // URL : /University/UniversityProfile
        // ============================================================

        public ActionResult UniversityProfile()
        {
            UniversityProfileModel model = universityProfileService.GetUniversityProfile();

            if (model == null)
            {
                TempData["Error"] = "University Profile is not set up yet. Please fill the form first.";
                return RedirectToAction("UniversityForm");
            }

            // University Type ka naam bhi dikhana hai, isliye Master se le aao
            if (model.UniversityTypeId.HasValue)
            {
                var type = universityTypeService.GetUniversityTypeById(model.UniversityTypeId.Value);
                ViewBag.UniversityTypeName = type != null ? type.UniversityTypeName : "";
            }

            return View(model);
        }


        //==================================================
        //campus managemnt
        //===================================================
        public ActionResult CampusManagement()
        {
            List<CampusModel> list = campusCategoryService.GetAllCampuses();

            // Dropdown always pulled fresh from Campus Master (MasterController manages it)
            ViewBag.CampusTypeList = new SelectList(
                campusCategoryService.GetActiveCampusCategories(), "CampusCategoryId", "CampusCategoryName");

            return View(list);
        }

        [HttpPost]
     
        public ActionResult CampusManagement(CampusModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields correctly.";

                List<CampusModel> list = campusCategoryService.GetAllCampuses();
                ViewBag.CampusTypeList = new SelectList(
                    campusCategoryService.GetActiveCampusCategories(), "CampusCategoryId", "CampusCategoryName");
                ViewBag.ReopenForm = true;

                return View(list);
            }

            bool result;

            if (model.CampusId > 0)
            {
                result = campusCategoryService.UpdateCampus(model);
                TempData[result ? "Success" : "Error"] =
                    result ? "Campus Updated Successfully." : "Unable to Update Campus.";
            }
            else
            {
                result = campusCategoryService.InsertCampus(model);
                TempData[result ? "Success" : "Error"] =
                    result ? "Campus Saved Successfully." : "Unable to Save Campus.";
            }

            return RedirectToAction("CampusManagement");
        }
        public ActionResult DeleteCampus(int id)
        {
            bool result = campusCategoryService.DeleteCampus(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Campus Deleted Successfully." : "Unable to Delete Campus.";

            return RedirectToAction("CampusManagement");
        }
        public ActionResult FacultyManagement()
        {
            return View();
        }
      
       
    }
}