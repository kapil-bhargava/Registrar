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

        public ActionResult Index()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult UniversityForm()
        {
            return View();
        }



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
            if (ModelState.IsValid)
            {
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
        public ActionResult UniversityProfile()
        {
            return View();
        }
       
    }
}