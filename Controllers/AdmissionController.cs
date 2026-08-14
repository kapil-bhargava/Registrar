    using Regis.Models;
    using Regis.Services;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    namespace Regis.Controllers
    {
        [Regis.Filters.AuthFilter]
        public class AdmissionController : Controller
        {
            // Single service for all 8 Admission steps — mirrors AcademicSetupController's.
            // "one service, one controller" pattern.
            private readonly AdmissionService admissionService = new AdmissionService();

            // Existing services reused for dropdown data (Course / Session / Category / Admission Mode)
            private readonly AcademicSetupService academicSetupService = new AcademicSetupService();
            private readonly MasterService masterService = new MasterService();
            private readonly CategoryService categoryService = new CategoryService();

            public ActionResult Index()
            {
                return RedirectToAction("PersonalInformation");
            }

            public ActionResult PersonalInformation(int? applicationId)
            {
                return View();
            }
            [HttpGet]
            public JsonResult GetAllPersonalInformation()
            {
                var list = admissionService.GetAllPersonalInformation();
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            [HttpGet]
            public JsonResult GetPersonalInformationById(int applicationId)
            {
                var model = admissionService.GetPersonalInformationById(applicationId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SavePersonalInformation(PersonalInformationModel model)
            {
                try
                {
                    // Display Name auto-build agar khaali chhoda gaya
                    if (string.IsNullOrEmpty(model.DisplayName))
                    {
                        model.DisplayName = string.Join(" ",
                            new[] { model.FirstName, model.MiddleName, model.LastName }
                            .Where(s => !string.IsNullOrWhiteSpace(s)));
                    }

                    if (model.ApplicationId > 0)
                    {
                        // -------- EDIT --------
                        bool updated = admissionService.UpdatePersonalInformation(model);
                        if (!updated)
                            return Json(new { success = false, message = "Unable to update. Record not found." });

                        return Json(new { success = true, message = "Personal Information updated successfully!", applicationId = model.ApplicationId });
                    }
                    else
                    {
                        // -------- NEW --------
                        var result = admissionService.InsertPersonalInformation(model);
                        if (result.applicationId == 0)
                            return Json(new { success = false, message = "Unable to create application." });

                        return Json(new
                        {
                            success = true,
                            message = "Application " + result.applicationNo + " created successfully!",
                            applicationId = result.applicationId,
                            applicationNo = result.applicationNo
                        });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }


        //[HttpPost]
        //public ActionResult PersonalInformation()
        //{
        //    return RedirectToAction("AdmissionInformation");
        //}
        // =========================================================
        // NEW ADMISSION → ADMISSION INFORMATION (Step 2 of 7)
        // purane "public ActionResult AdmissionInformation(int? applicationId)" ko isse REPLACE karo
        // =========================================================

        public ActionResult AdmissionInformation(int? applicationId)
        {
            ViewBag.ApplicationId = applicationId ?? 0;
            ViewBag.OpenSetups = admissionService.GetOpenAdmissionSetups();   // already exists in service
            ViewBag.Categories = categoryService.GetAllCategories();          // already used elsewhere
            return View();
        }

        [HttpGet]
            public JsonResult GetAdmissionInformationById(int applicationId)
            {
                var model = admissionService.GetAdmissionInformationById(applicationId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveAdmissionInformation(AdmissionInformationModel model)
            {
                try
                {
                    bool updated = admissionService.SaveAdmissionInformation(model);
                    if (!updated)
                        return Json(new { success = false, message = "Unable to save. Application not found." });

                    return Json(new { success = true, message = "Admission information saved!", applicationId = model.ApplicationId });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // =========================================================
            // NEW ADMISSION → ADDRESS INFORMATION (Step 2 of 7)
            // AdmissionController.cs class ke andar paste karo
            // (purani "public ActionResult AddressInformation()" ko isse REPLACE karo)
            // =========================================================

            public ActionResult AddressInformation(int? applicationId)
            {
                ViewBag.ApplicationId = applicationId ?? 0;
                return View();
            }

            [HttpGet]
            public JsonResult GetAddressInformationById(int applicationId)
            {
                var model = admissionService.GetAddressInformationById(applicationId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveAddressInformation(AddressInformationModel model)
            {
                try
                {
                    bool updated = admissionService.SaveAddressInformation(model);
                    if (!updated)
                        return Json(new { success = false, message = "Unable to save. Application not found." });

                    return Json(new { success = true, message = "Address saved successfully!", applicationId = model.ApplicationId });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }


            // REPLACE the old "public ActionResult ParentDetails() { return View(); }" with this block

            public ActionResult ParentDetails(int? applicationId)
            {
                ViewBag.ApplicationId = applicationId ?? 0;
                return View();
            }

            [HttpGet]
            public JsonResult GetParentDetailsById(int applicationId)
            {
                var model = admissionService.GetParentDetailsById(applicationId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveParentDetails(ParentDetailsModel model)
            {
                try
                {
                    bool updated = admissionService.SaveParentDetails(model);
                    if (!updated)
                        return Json(new { success = false, message = "Unable to save. Application not found." });

                    return Json(new { success = true, message = "Parent details saved successfully!", applicationId = model.ApplicationId });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // REPLACE the old "public ActionResult BankDetails() { return View(); }" with this block

            public ActionResult BankDetails(int? applicationId)
            {
                ViewBag.ApplicationId = applicationId ?? 0;
                return View();
            }

            [HttpGet]
            public JsonResult GetBankDetailsById(int applicationId)
            {
                var model = admissionService.GetBankDetailsById(applicationId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveBankDetails(BankDetailsModel model)
            {
                try
                {
                    bool updated = admissionService.SaveBankDetails(model);
                    if (!updated)
                        return Json(new { success = false, message = "Unable to save. Application not found." });

                    return Json(new { success = true, message = "Bank details saved successfully!", applicationId = model.ApplicationId });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            //[HttpPost]
            //public ActionResult BankDetails()
            //{
            //    return RedirectToAction("AcademicRecords");
            //}

           public ActionResult AcademicRecords(int? applicationId)
    {
        ViewBag.ApplicationId = applicationId ?? 0;
        return View();
    }

    [HttpGet]
    public JsonResult GetAcademicRecords(int applicationId)
    {
        var list = admissionService.GetAcademicRecords(applicationId);
        return Json(list, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult SaveAcademicRecord(AcademicRecordModel model)
    {
        try
        {
            int id = admissionService.InsertAcademicRecord(model);
            if (id == 0)
                return Json(new { success = false, message = "Unable to save record." });

            return Json(new { success = true, message = "Academic record saved!", academicRecordId = id });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error: " + ex.Message });
        }
    }

    public ActionResult AdditionalDetails(int? applicationId)
    {
        ViewBag.ApplicationId = applicationId ?? 0;
        return View();
    }

    [HttpGet]
    public JsonResult GetAdditionalDetails(int applicationId)
    {
        var list = admissionService.GetAdditionalDetails(applicationId);
        return Json(list, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult SaveAdditionalDetail(AdditionalDetailModel model)
    {
        try
        {
            int id = admissionService.InsertAdditionalDetail(model);
            if (id == 0)
                return Json(new { success = false, message = "Unable to save record." });

            return Json(new { success = true, message = "Additional detail saved!", additionalDetailId = id });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error: " + ex.Message });
        }
    }

            //[HttpPost]
            //public ActionResult EditPersonalInformation()
            //{
            //    return RedirectToAction("PersonalInformationList");
            //}

          
          

            //[HttpPost]
            // ============================================================
            // STEP 1 : ADMISSION SETUP
            // URL : /Admission/AdmissionSetup
            // ============================================================

            public ActionResult AdmissionSetup()
            {
                ViewBag.Setups = admissionService.GetAllAdmissionSetups();
                ViewBag.Courses = masterService.GetActiveCourseMaster();
                ViewBag.SeatMatrixList = new SeatMatrixService().GetAllSeatMatrix();
                ViewBag.Sessions = academicSetupService.GetAllSessions().Where(s => s.Status == "Active").ToList();
                return View();
            }

            [HttpPost]
            public JsonResult SaveAdmissionSetup(AdmissionSetupModel model)
            {
                try
                {
                    int newId = admissionService.InsertAdmissionSetup(model);
                    return Json(new { success = true, message = "Admission opened successfully! Now visible in Eligibility Check & Student Registration.", id = newId });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            [HttpPost]
            public JsonResult UpdateAdmissionSetup(AdmissionSetupModel model)
            {
                try
                {
                    bool result = admissionService.UpdateAdmissionSetup(model);
                    return Json(new { success = result, message = result ? "Admission Setup updated successfully!" : "Unable to update Admission Setup." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            public JsonResult GetAdmissionSetupById(int id)
            {
                var model = admissionService.GetAdmissionSetupById(id);
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult ToggleAdmissionSetupStatus(int id)
            {
                bool result = admissionService.ToggleAdmissionSetupStatus(id);
                return Json(new { success = result, message = result ? "Status updated!" : "Unable to update status." });
            }

            [HttpPost]
            public JsonResult DeleteAdmissionSetup(int id)
            {
                try
                {
                    bool result = admissionService.DeleteAdmissionSetup(id);
                    return Json(new { success = result, message = result ? "Admission Setup deleted." : "Unable to delete." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // ============================================================
            // STEP 2 : ELIGIBILITY CHECK
            // URL : /Admission/EligibilityCheck
            // ============================================================

            public ActionResult EligibilityCheck()
            {
                ViewBag.OpenSetups = admissionService.GetOpenAdmissionSetups();
                ViewBag.RecentChecks = admissionService.GetRecentEligibilityChecks();
                return View();
            }

            [HttpPost]
            public JsonResult CheckEligibility(string applicantName, int admissionSetupId, decimal percentage)
            {
                try
                {
                    var result = admissionService.CheckEligibility(applicantName, admissionSetupId, percentage);
                    if (result == null)
                        return Json(new { success = false, message = "Invalid Admission Setup selected." });

                    return Json(new
                    {
                        success = true,
                        applicantName = result.ApplicantName,
                        courseName = result.CourseName,
                        sessionName = result.SessionName,
                        criteria = result.EligibilityCriteria,
                        minPct = result.MinEligibilityPct,
                        pctObtained = result.PercentageObtained,
                        isEligible = result.IsEligible
                    });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            public JsonResult GetRecentEligibilityChecks()
            {
                var list = admissionService.GetRecentEligibilityChecks();
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            // ============================================================
            // STEP 3 : STUDENT REGISTRATION
            // URL : /Admission/StudentRegistration
            // ============================================================

            public ActionResult StudentRegistration()
            {
                ViewBag.AdmissionModes = masterService.GetAllAdmissionModes();
                ViewBag.Categories = categoryService.GetAllCategories();
                ViewBag.OpenSetups = admissionService.GetOpenAdmissionSetups();
                return View();
            }

            [HttpPost]
            public JsonResult RegisterApplication(ApplicationModel model)
            {
                try
                {
                    string appNo = admissionService.RegisterApplication(model);
                    if (appNo == null)
                        return Json(new { success = false, message = "Unable to create application." });

                    return Json(new { success = true, message = "Application " + appNo + " created! Now visible in Application Management.", applicationNo = appNo });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // ============================================================
            // STEP 4 : APPLICATION MANAGEMENT
            // URL : /Admission/Application
            // ============================================================

            public ActionResult Application()
            {
                ViewBag.Applications = admissionService.GetAllApplications();
                return View();
            }

            // ============================================================
            // STEP 5 : DOCUMENT VERIFICATION
            // URL : /Admission/DocumentVerification?appId=123
            // ============================================================

            public ActionResult DocumentVerification(int? appId)
            {
                if (appId.HasValue)
                {
                    ViewBag.Checklist = admissionService.GetDocumentChecklist(appId.Value);
                    ViewBag.App = admissionService.GetApplicationById(appId.Value);
                }
                else
                {
                    ViewBag.PendingDocs = admissionService.GetApplicationsPendingDocs();
                }
                ViewBag.AppId = appId;
                return View();
            }

            [HttpPost]
            public JsonResult SaveDocumentVerification(int applicationId, string submittedDocumentIdsCsv)
            {
                try
                {
                    bool allVerified = admissionService.VerifyDocuments(applicationId, submittedDocumentIdsCsv);
                    return Json(new
                    {
                        success = true,
                        allVerified = allVerified,
                        message = allVerified
                            ? "All mandatory documents verified! Now visible in Counselling."
                            : "Saved — but mandatory documents still missing."
                    });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // ============================================================
            // STEP 6 : COUNSELLING
            // URL : /Admission/Counselling?appId=123
            // ============================================================

            public ActionResult Counselling(int? appId)
            {
                if (appId.HasValue)
                {
                    ViewBag.App = admissionService.GetApplicationById(appId.Value);
                }
                else
                {
                    ViewBag.PendingCounselling = admissionService.GetApplicationsPendingCounselling();
                }
                ViewBag.AppId = appId;
                return View();
            }

            [HttpPost]
            public JsonResult SaveCounselling(int applicationId, DateTime counsellingDate, string counsellingTime, string counsellingMode)
            {
                try
                {
                    TimeSpan? time = string.IsNullOrEmpty(counsellingTime) ? (TimeSpan?)null : TimeSpan.Parse(counsellingTime);
                    string seatNo = admissionService.ScheduleCounselling(applicationId, counsellingDate, time, counsellingMode);

                    if (seatNo == null)
                        return Json(new { success = false, message = "Unable to schedule counselling." });

                    return Json(new { success = true, seatNumber = seatNo, message = "Counselling completed! Seat " + seatNo + " allotted. Now visible in Fee Payment." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // ============================================================
            // STEP 7 : FEE PAYMENT
            // URL : /Admission/FeePayment?appId=123
            // ============================================================

            public ActionResult FeePayment(int? appId)
            {
                if (appId.HasValue)
                {
                    ViewBag.App = admissionService.GetApplicationById(appId.Value);
                }
                else
                {
                    ViewBag.PendingFee = admissionService.GetApplicationsPendingFee();
                }
                ViewBag.AppId = appId;
                return View();
            }

            [HttpPost]
            public JsonResult SaveFeePayment(int applicationId, string feeMode, decimal feeAmount)
            {
                try
                {
                    string receiptNo = admissionService.CollectFee(applicationId, feeMode, feeAmount);
                    if (receiptNo == null)
                        return Json(new { success = false, message = "Unable to collect fee." });

                    return Json(new { success = true, receiptNo = receiptNo, message = "Fee collected! Receipt " + receiptNo + ". Now visible in Admission (Final)." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }

            // ============================================================
            // STEP 8 : ADMISSION FINAL
            // URL : /Admission/Final?appId=123
            // ============================================================

            public ActionResult Final(int? appId)
            {
                if (appId.HasValue)
                {
                    ViewBag.App = admissionService.GetApplicationById(appId.Value);
                }
                else
                {
                    ViewBag.PendingFinal = admissionService.GetApplicationsPendingFinal();
                }
                ViewBag.AppId = appId;
                return View();
            }

        [HttpPost]
        public JsonResult ConfirmAdmission(int applicationId)
        {
            try
            {
                string studentId = admissionService.ConfirmAdmission(applicationId);

                if (string.IsNullOrEmpty(studentId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to confirm admission."
                    });
                }

                return Json(new
                {
                    success = true,
                    studentId = studentId,
                    message = "Admission confirmed! Student ID: " + studentId
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.ToString()
                });
            }
        }


    }
    }