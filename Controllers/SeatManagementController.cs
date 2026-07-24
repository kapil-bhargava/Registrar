using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Regis.Models;
using Regis.Services;

namespace Regis.Controllers
{
    public class SeatManagementController : Controller
    {
        private readonly SeatMatrixService seatMatrixService = new SeatMatrixService();
        private readonly MasterService MasterService = new MasterService();
        private readonly AcademicSetupService academicSetupService = new AcademicSetupService();

        // GET: SeatManagement
        public ActionResult Index()
        {
            return View();
        }

        // ============================================================
        // SEAT MATRIX
        // URL : /SeatManagement/SeatMatrix
        // Maps Course + AcademicSession -> Total Seats
        // ============================================================

        public ActionResult SeatMatrix()
        {
            List<SeatMatrixModel> list = seatMatrixService.GetAllSeatMatrix();

            ViewBag.Courses = MasterService.GetActiveCourseMaster();
            ViewBag.Sessions = academicSetupService.GetAllSessions().Where(s => s.Status == "Active").ToList();

            return View(list);
        }

        [HttpPost]
        public ActionResult SeatMatrix(SeatMatrixModel model)
        {
            bool result;
            if (model.SeatMatrixId > 0)
            {
                result = seatMatrixService.UpdateSeatMatrix(model);
                TempData[result ? "Success" : "Error"] =
                    result ? "Seat Matrix Updated Successfully." : "Unable to Update Seat Matrix.";
            }
            else
            {
                result = seatMatrixService.InsertSeatMatrix(model);
                TempData[result ? "Success" : "Error"] =
                    result ? "Seat Matrix Saved Successfully." : "Unable to Save Seat Matrix.";
            }

            return RedirectToAction("SeatMatrix");
        }

        public JsonResult GetSeatMatrixById(int id)
        {
            var model = seatMatrixService.GetSeatMatrixById(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSeatMatrix(int id)
        {
            bool result = seatMatrixService.DeleteSeatMatrix(id);
            TempData[result ? "Success" : "Error"] =
                result ? "Seat Matrix Deleted Successfully." : "Unable to Delete Seat Matrix.";
            return RedirectToAction("SeatMatrix");
        }

        //===============================================
        //            CategorywiseSeats
        //===============================================
        public ActionResult CategorywiseSeats()
        {
            List<SeatCategoryMappingModel> list = seatMatrixService.GetAllSeatCategoryMappings();
            ViewBag.SeatMatrixList = seatMatrixService.GetAllSeatMatrix();
            ViewBag.Categories = new CategoryService().GetAllCategories();
            return View(list);
        }

        [HttpPost]
        public ActionResult CategorywiseSeats(int SeatMatrixId, string CategoryIdsCsv, string SeatsCsv)
        {
            var model = new SeatCategoryMappingModel
            {
                SeatMatrixId = SeatMatrixId,
                CategoryIdsCsv = CategoryIdsCsv,
                SeatsCsv = SeatsCsv
            };

            bool result = seatMatrixService.SaveSeatCategoryMapping(model);
            TempData[result ? "Success" : "Error"] =
                result ? "Category-wise Seats Saved Successfully." : "Unable to Save Category-wise Seats.";

            return RedirectToAction("CategorywiseSeats");
        }

        public JsonResult GetSeatCategoryMappingBySeatMatrix(int id)
        {
            var list = seatMatrixService.GetSeatCategoryMappingBySeatMatrix(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SeatAllotment()
        {
            return View();
        }

    }
}