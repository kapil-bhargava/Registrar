using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    /// <summary>
    /// One combined service for the entire Admission module (Steps 1–8):
    /// Admission Setup, Eligibility Check, Student Registration, Application
    /// Management, Document Verification, Counselling, Fee Payment, Admission
    /// Final. Mirrors AcademicSetupService's Flag + DBHelper pattern.
    ///
    /// NOTE: stored procs use SET NOCOUNT ON, so "rows > 0" checks are written
    /// as "rows != 0" — a successful UPDATE/DELETE returns -1 there, and only
    /// 0 means nothing matched.
    /// </summary>
    public class AdmissionService
    {
        private readonly DBHelper db = new DBHelper();

        // =========================================================
        // STEP 1 : ADMISSION SETUP
        // =========================================================

        public List<AdmissionSetupModel> GetAllAdmissionSetups()
        {
            var list = new List<AdmissionSetupModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdmissionSetup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapSetup(dr));
                }
            }
            return list;
        }

        // Feeds Eligibility Check + Student Registration course dropdowns (Open only)
        public List<AdmissionSetupModel> GetOpenAdmissionSetups()
        {
            var list = new List<AdmissionSetupModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdmissionSetup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETOPEN");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new AdmissionSetupModel
                        {
                            AdmissionSetupId = Convert.ToInt32(dr["AdmissionSetupId"]),
                            CourseId = Convert.ToInt32(dr["CourseId"]),
                            CourseName = dr["CourseName"].ToString(),
                            AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                            SessionName = dr["SessionName"].ToString(),
                            TotalSeats = Convert.ToInt32(dr["TotalSeats"]),
                            MinEligibilityPct = Convert.ToDecimal(dr["MinEligibilityPct"]),
                            EligibilityCriteria = dr["EligibilityCriteria"] as string
                        });
                    }
                }
            }
            return list;
        }

        public AdmissionSetupModel GetAdmissionSetupById(int id)
        {
            AdmissionSetupModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdmissionSetup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@AdmissionSetupId", id);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) model = MapSetup(dr);
            }
            return model;
        }

        public int InsertAdmissionSetup(AdmissionSetupModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdmissionSetup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                AddSetupParams(cmd, model);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public bool UpdateAdmissionSetup(AdmissionSetupModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdmissionSetup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@AdmissionSetupId", model.AdmissionSetupId);
                AddSetupParams(cmd, model);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }

        public bool ToggleAdmissionSetupStatus(int id)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdmissionSetup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "TOGGLESTATUS");
                cmd.Parameters.AddWithValue("@AdmissionSetupId", id);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }

        public bool DeleteAdmissionSetup(int id)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdmissionSetup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@AdmissionSetupId", id);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }

        private void AddSetupParams(SqlCommand cmd, AdmissionSetupModel m)
        {
            cmd.Parameters.AddWithValue("@CourseId", m.CourseId);
            cmd.Parameters.AddWithValue("@AcademicSessionId", m.AcademicSessionId);
            cmd.Parameters.AddWithValue("@TotalSeats", m.TotalSeats);
            cmd.Parameters.AddWithValue("@MinEligibilityPct", m.MinEligibilityPct);
            cmd.Parameters.AddWithValue("@ApplicationStartDate", m.ApplicationStartDate);
            cmd.Parameters.AddWithValue("@ApplicationEndDate", m.ApplicationEndDate);
            cmd.Parameters.AddWithValue("@EligibilityCriteria", (object)m.EligibilityCriteria ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", m.Status ?? "Open");
        }

        private AdmissionSetupModel MapSetup(SqlDataReader dr)
        {
            return new AdmissionSetupModel
            {
                AdmissionSetupId = Convert.ToInt32(dr["AdmissionSetupId"]),
                CourseId = Convert.ToInt32(dr["CourseId"]),
                CourseName = dr["CourseName"].ToString(),
                AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                SessionName = dr["SessionName"].ToString(),
                TotalSeats = Convert.ToInt32(dr["TotalSeats"]),
                MinEligibilityPct = Convert.ToDecimal(dr["MinEligibilityPct"]),
                ApplicationStartDate = Convert.ToDateTime(dr["ApplicationStartDate"]),
                ApplicationEndDate = Convert.ToDateTime(dr["ApplicationEndDate"]),
                EligibilityCriteria = dr["EligibilityCriteria"] as string,
                Status = dr["Status"].ToString()
            };
        }

        // =========================================================
        // STEP 2 : ELIGIBILITY CHECK
        // =========================================================

        public EligibilityCheckModel CheckEligibility(string applicantName, int admissionSetupId, decimal percentage)
        {
            EligibilityCheckModel result = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_EligibilityCheck", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "CHECK");
                cmd.Parameters.AddWithValue("@ApplicantName", applicantName);
                cmd.Parameters.AddWithValue("@AdmissionSetupId", admissionSetupId);
                cmd.Parameters.AddWithValue("@PercentageObtained", percentage);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        result = new EligibilityCheckModel
                        {
                            ApplicantName = dr["ApplicantName"].ToString(),
                            CourseName = dr["CourseName"].ToString(),
                            SessionName = dr["SessionName"].ToString(),
                            EligibilityCriteria = dr["EligibilityCriteria"] as string,
                            MinEligibilityPct = Convert.ToDecimal(dr["MinEligibilityPct"]),
                            PercentageObtained = Convert.ToDecimal(dr["PercentageObtained"]),
                            IsEligible = Convert.ToBoolean(dr["IsEligible"])
                        };
                    }
                }
            }
            return result;
        }

        public List<EligibilityCheckModel> GetRecentEligibilityChecks()
        {
            var list = new List<EligibilityCheckModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_EligibilityCheck", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETRECENT");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new EligibilityCheckModel
                        {
                            ApplicantName = dr["ApplicantName"].ToString(),
                            CourseName = dr["CourseName"].ToString(),
                            PercentageObtained = Convert.ToDecimal(dr["PercentageObtained"]),
                            IsEligible = Convert.ToBoolean(dr["IsEligible"]),
                            CheckedOn = Convert.ToDateTime(dr["CheckedOn"])
                        });
                    }
                }
            }
            return list;
        }

        // =========================================================
        // STEP 3 : STUDENT REGISTRATION  (creates the Application)
        // =========================================================

        public string RegisterApplication(ApplicationModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@FullName", model.FullName);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Phone", model.Phone);
                cmd.Parameters.AddWithValue("@DOB", (object)model.DOB ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", (object)model.Gender ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@AdmissionModeId", model.AdmissionModeId);
                cmd.Parameters.AddWithValue("@AdmissionSetupId", model.AdmissionSetupId);
                cmd.Parameters.AddWithValue("@PreviousPercentage", (object)model.PreviousPercentage ?? DBNull.Value);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) return dr["ApplicationNo"].ToString();
            }
            return null;
        }

        // =========================================================
        // STEP 4 : APPLICATION MANAGEMENT
        // =========================================================

        public List<ApplicationModel> GetAllApplications()
        {
            var list = new List<ApplicationModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            CourseName = dr["CourseName"].ToString(),
                            CategoryName = dr["CategoryName"].ToString(),
                            RegisteredOn = Convert.ToDateTime(dr["RegisteredOn"]),
                            Stage = dr["Stage"].ToString(),
                            DocVerified = Convert.ToBoolean(dr["DocVerified"]),
                            CounsellingDone = Convert.ToBoolean(dr["CounsellingDone"]),
                            FeePaid = Convert.ToBoolean(dr["FeePaid"]),
                            StudentId = dr["StudentId"] as string
                        });
                    }
                }
            }
            return list;
        }

        // ---------------------------------------------------------
        // NEW: fetch ONE application by id — used by the detail views
        // (Document Verification / Counselling / Fee Payment / Final)
        // to show applicant name, course, category, seat no, receipt
        // no, etc. at the top of the page. Reuses sp_Application's
        // existing GETBYID flag — no new stored procedure needed.
        // ---------------------------------------------------------
        public ApplicationModel GetApplicationById(int id)
        {
            ApplicationModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@ApplicationId", id);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new ApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            CourseName = dr["CourseName"].ToString(),
                            CategoryName = dr["CategoryName"].ToString(),
                            Stage = dr["Stage"].ToString(),
                            DocVerified = Convert.ToBoolean(dr["DocVerified"]),
                            CounsellingDone = Convert.ToBoolean(dr["CounsellingDone"]),
                            FeePaid = Convert.ToBoolean(dr["FeePaid"]),
                            SeatNumber = dr["SeatNumber"] as string,
                            FeeReceiptNo = dr["FeeReceiptNo"] as string,
                            StudentId = dr["StudentId"] as string
                        };
                    }
                }
            }
            return model;
        }

        // =========================================================
        // STEP 5 : DOCUMENT VERIFICATION
        // =========================================================

        public List<ApplicationModel> GetApplicationsPendingDocs()
        {
            var list = new List<ApplicationModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETPENDINGDOCS");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            CourseName = dr["CourseName"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public List<DocumentChecklistItemModel> GetDocumentChecklist(int applicationId)
        {
            var list = new List<DocumentChecklistItemModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETDOCCHECKLIST");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new DocumentChecklistItemModel
                        {
                            DocumentEnclosureId = Convert.ToInt32(dr["DocumentEnclosureId"]),
                            DocumentName = dr["DocumentName"].ToString(),
                            IsMandatory = Convert.ToBoolean(dr["IsMandatory"]),
                            IsSubmitted = Convert.ToBoolean(dr["IsSubmitted"])
                        });
                    }
                }
            }
            return list;
        }

        // submittedDocumentIdsCsv = comma separated DocumentEnclosureIds that were ticked in the UI
        public bool VerifyDocuments(int applicationId, string submittedDocumentIdsCsv)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "VERIFYDOCS");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                cmd.Parameters.AddWithValue("@SubmittedDocumentIds", submittedDocumentIdsCsv ?? "");
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && Convert.ToBoolean(result);
            }
        }

        // =========================================================
        // STEP 6 : COUNSELLING & SEAT ALLOTMENT
        // =========================================================

        public List<ApplicationModel> GetApplicationsPendingCounselling()
        {
            var list = new List<ApplicationModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETPENDINGCOUNSELLING");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            CourseName = dr["CourseName"].ToString(),
                            CategoryName = dr["CategoryName"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public string ScheduleCounselling(int applicationId, DateTime date, TimeSpan? time, string mode)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "SCHEDULECOUNSELLING");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                cmd.Parameters.AddWithValue("@CounsellingDate", date);
                cmd.Parameters.AddWithValue("@CounsellingTime", (object)time ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CounsellingMode", (object)mode ?? DBNull.Value);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) return dr["SeatNumber"].ToString();
            }
            return null;
        }

        // =========================================================
        // STEP 7 : FEE PAYMENT
        // =========================================================

        public List<ApplicationModel> GetApplicationsPendingFee()
        {
            var list = new List<ApplicationModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETPENDINGFEE");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            CourseName = dr["CourseName"].ToString(),
                            SeatNumber = dr["SeatNumber"] as string
                        });
                    }
                }
            }
            return list;
        }

        public string CollectFee(int applicationId, string feeMode, decimal feeAmount)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "COLLECTFEE");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                cmd.Parameters.AddWithValue("@FeeMode", feeMode);
                cmd.Parameters.AddWithValue("@FeeAmount", feeAmount);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) return dr["ReceiptNo"].ToString();
            }
            return null;
        }

        // =========================================================
        // STEP 8 : ADMISSION FINAL
        // =========================================================

        public List<ApplicationModel> GetApplicationsPendingFinal()
        {
            var list = new List<ApplicationModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETPENDINGFINAL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            CourseName = dr["CourseName"].ToString(),
                            SeatNumber = dr["SeatNumber"] as string,
                            FeeReceiptNo = dr["FeeReceiptNo"] as string
                        });
                    }
                }
            }
            return list;
        }

        public string ConfirmAdmission(int applicationId)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "CONFIRMADMISSION");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) return dr["StudentId"].ToString();
            }
            return null;
        }
    }
}