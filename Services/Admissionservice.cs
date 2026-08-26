using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    /// <summary>
    /// One combined service for the entire Admission module (Steps 1–8)://
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

        // AdmissionService.cs — sirf "ConfirmAdmission" method REPLACE karo isse
        // (baaki poori file same rehne do)

        public string ConfirmAdmission(int applicationId)
        {
            string studentId = null;

            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "CONFIRMADMISSION");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) studentId = dr["StudentId"].ToString();
            }

            //  StudentId mil gaya — ab Email + Phone nikaal ke login ban jayega (regisrar se add hua login create hui bs
            if (!string.IsNullOrEmpty(studentId))
            {
                string email = null;
                string phone = null;

                using (SqlConnection con = db.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT Email, Phone FROM Application WHERE ApplicationId = @ApplicationId", con))
                {
                    cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            email = dr["Email"] as string;
                            phone = dr["Phone"] as string;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(phone))
                {
                    new StudentLoginService().CreateLogin(
                        applicationId,
                        email.Trim().ToLower(),   // Username = Email
                        phone.Trim());            // Password = Phone
                }
            }

            return studentId;
        }
        // ==========================================================================
        // APNI AdmissionService.cs class me ye poora region add/REPLACE karo
        // (agar pehle wala already paste kar chuke ho to use isse overwrite karo)
        // ==========================================================================

        // =========================================================
        // NEW ADMISSION → PERSONAL INFORMATION (Step 1 of 7)
        // =========================================================

        public List<ApplicationListItemModel> GetAllPersonalInformation()
        {
            var list = new List<ApplicationListItemModel>();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["erpdb"].ConnectionString))
            using (var cmd = new SqlCommand("sp_NewAdmissionPersonalInfo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ApplicationListItemModel
                        {
                            ApplicationId = reader["ApplicationId"] != DBNull.Value ? Convert.ToInt32(reader["ApplicationId"]) : 0,
                            ApplicationNo = reader["ApplicationNo"] as string,
                            FullName = reader["FullName"] as string,
                            Email = reader["Email"] as string,
                            Phone = reader["Phone"] as string,
                            DOB = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : (DateTime?)null,
                            Gender = reader["Gender"] as string,
                            RegisteredOn = reader["RegisteredOn"] != DBNull.Value ? Convert.ToDateTime(reader["RegisteredOn"]) : (DateTime?)null,
                            Stage = reader["Stage"] as string,
                            Citizenship = reader["Citizenship"] as string,
                            BloodGroup = reader["BloodGroup"] as string,

                            AcademicSessionName = reader["AcademicSessionName"] as string,
                            Degree = reader["Degree"] as string,
                            Branch = reader["Branch"] as string,
                            DateOfAdmission = reader["DateOfAdmission"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfAdmission"]) : (DateTime?)null,

                            PermanentCity = reader["PermanentCity"] as string,
                            PermanentState = reader["PermanentState"] as string,
                            LocalCity = reader["LocalCity"] as string,
                            LocalState = reader["LocalState"] as string,

                            FatherFirstName = reader["FatherFirstName"] as string,
                            FatherLastName = reader["FatherLastName"] as string,
                            MotherFirstName = reader["MotherFirstName"] as string,
                            MotherLastName = reader["MotherLastName"] as string,

                            BankName = reader["BankName"] as string,
                            AccountNumber = reader["AccountNumber"] as string
                        });
                    }
                }
            }

            return list;
        }

        public PersonalInformationModel GetPersonalInformationById(int applicationId)
        {
            PersonalInformationModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionPersonalInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new PersonalInformationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"] as string,
                            Email = dr["Email"] as string,
                            Phone = dr["Phone"] as string,
                            DOB = dr["DOB"] as DateTime?,
                            Gender = dr["Gender"] as string,
                            Title = dr["Title"] as string,
                            FirstName = dr["FirstName"] as string,
                            MiddleName = dr["MiddleName"] as string,
                            LastName = dr["LastName"] as string,
                            DisplayNameFormat = dr["DisplayNameFormat"] as string,
                            DisplayName = dr["DisplayName"] as string,
                            MaritalStatus = dr["MaritalStatus"] as string,
                            BirthState = dr["BirthState"] as string,
                            BirthPlace = dr["BirthPlace"] as string,
                            WhatsAppNumber = dr["WhatsAppNumber"] as string,
                            ReferralSource = dr["ReferralSource"] as string,
                            PhysicallyChallenged = Convert.ToBoolean(dr["PhysicallyChallenged"]),
                            BloodGroup = dr["BloodGroup"] as string,
                            IdentityMark = dr["IdentityMark"] as string,
                            MotherTongue = dr["MotherTongue"] as string,
                            AlternateMobileNumber = dr["AlternateMobileNumber"] as string,
                            Citizenship = dr["Citizenship"] as string,
                            DomicileCountry = dr["DomicileCountry"] as string,
                            DomicileState = dr["DomicileState"] as string,
                            Nationality = dr["Nationality"] as string,
                            Religion = dr["Religion"] as string,
                            Caste = dr["Caste"] as string,
                            ABCId = dr["ABCId"] as string,
                            AntiRaggingId = dr["AntiRaggingId"] as string
                        };
                    }
                }
            }
            return model;
        }

        public (int applicationId, string applicationNo) InsertPersonalInformation(PersonalInformationModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionPersonalInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                AddPersonalInfoParams(cmd, model);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        return (Convert.ToInt32(dr["ApplicationId"]), dr["ApplicationNo"].ToString());
                }
            }
            return (0, null);
        }

        public bool UpdatePersonalInformation(PersonalInformationModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionPersonalInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@ApplicationId", model.ApplicationId);
                AddPersonalInfoParams(cmd, model);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }

        private void AddPersonalInfoParams(SqlCommand cmd, PersonalInformationModel m)
        {
            cmd.Parameters.AddWithValue("@Email", (object)m.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object)m.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DOB", (object)m.DOB ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", (object)m.Gender ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@Title", (object)m.Title ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FirstName", (object)m.FirstName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MiddleName", (object)m.MiddleName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastName", (object)m.LastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DisplayNameFormat", (object)m.DisplayNameFormat ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DisplayName", (object)m.DisplayName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaritalStatus", (object)m.MaritalStatus ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@BirthState", (object)m.BirthState ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BirthPlace", (object)m.BirthPlace ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WhatsAppNumber", (object)m.WhatsAppNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ReferralSource", (object)m.ReferralSource ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhysicallyChallenged", m.PhysicallyChallenged);
            cmd.Parameters.AddWithValue("@BloodGroup", (object)m.BloodGroup ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdentityMark", (object)m.IdentityMark ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MotherTongue", (object)m.MotherTongue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AlternateMobileNumber", (object)m.AlternateMobileNumber ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@Citizenship", (object)m.Citizenship ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DomicileCountry", (object)m.DomicileCountry ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DomicileState", (object)m.DomicileState ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Nationality", (object)m.Nationality ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Religion", (object)m.Religion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Caste", (object)m.Caste ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ABCId", (object)m.ABCId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AntiRaggingId", (object)m.AntiRaggingId ?? DBNull.Value);
        }
        // =========================================================
        // NEW ADMISSION → ADDRESS INFORMATION (Step 2 of 7)
        // AdmissionService.cs class ke andar paste karo
        // =========================================================

        public AddressInformationModel GetAddressInformationById(int applicationId)
        {
            AddressInformationModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionAddress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new AddressInformationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"] as string,
                            PermanentAddress = dr["PermanentAddress"] as string,
                            PermanentCountry = dr["PermanentCountry"] as string,
                            PermanentState = dr["PermanentState"] as string,
                            PermanentDistrict = dr["PermanentDistrict"] as string,
                            PermanentCity = dr["PermanentCity"] as string,
                            PermanentPinCode = dr["PermanentPinCode"] as string,
                            LocalSameAsPermanent = dr["LocalSameAsPermanent"] != DBNull.Value && Convert.ToBoolean(dr["LocalSameAsPermanent"]),
                            LocalAddress = dr["LocalAddress"] as string,
                            LocalCountry = dr["LocalCountry"] as string,
                            LocalState = dr["LocalState"] as string,
                            LocalDistrict = dr["LocalDistrict"] as string,
                            LocalCity = dr["LocalCity"] as string,
                            LocalPinCode = dr["LocalPinCode"] as string
                        };
                    }
                }
            }
            return model;
        }

        public bool SaveAddressInformation(AddressInformationModel m)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionAddress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@ApplicationId", m.ApplicationId);
                cmd.Parameters.AddWithValue("@PermanentAddress", (object)m.PermanentAddress ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PermanentCountry", (object)m.PermanentCountry ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PermanentState", (object)m.PermanentState ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PermanentDistrict", (object)m.PermanentDistrict ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PermanentCity", (object)m.PermanentCity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PermanentPinCode", (object)m.PermanentPinCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LocalSameAsPermanent", m.LocalSameAsPermanent);
                cmd.Parameters.AddWithValue("@LocalAddress", (object)m.LocalAddress ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LocalCountry", (object)m.LocalCountry ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LocalState", (object)m.LocalState ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LocalDistrict", (object)m.LocalDistrict ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LocalCity", (object)m.LocalCity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LocalPinCode", (object)m.LocalPinCode ?? DBNull.Value);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }
        // =========================================================
        // NEW ADMISSION → ADMISSION INFORMATION (Step 2 of 7)
        // AdmissionService.cs class ke andar paste karo
        // =========================================================

        public AdmissionInformationModel GetAdmissionInformationById(int applicationId)
        {
            AdmissionInformationModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionAdmissionInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new AdmissionInformationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"] as string,
                            AcademicStream = dr["AcademicStream"] as string,
                            AcademicSessionName = dr["AcademicSessionName"] as string,
                            AdmissionSetupId = dr["AdmissionSetupId"] != DBNull.Value ? Convert.ToInt32(dr["AdmissionSetupId"]) : 0,
                            CategoryId = dr["CategoryId"] != DBNull.Value ? Convert.ToInt32(dr["CategoryId"]) : 0,
                            Degree = dr["Degree"] as string,
                            Branch = dr["Branch"] as string,
                            AcademicBatch = dr["AcademicBatch"] as string,
                            Enrollment = dr["Enrollment"] as string,
                            AcademicYear = dr["AcademicYear"] as string,
                            Semester = dr["Semester"] as string,
                            Scheme = dr["Scheme"] as string,
                            ClassSection = dr["ClassSection"] as string,
                            RollNumber = dr["RollNumber"] as string,
                            DateOfAdmission = dr["DateOfAdmission"] as DateTime?,
                            AdmissionCategory = dr["AdmissionCategory"] as string,
                            FeesCategory = dr["FeesCategory"] as string,
                            Shift = dr["Shift"] as string,
                            EntranceExamRegNo = dr["EntranceExamRegNo"] as string,
                            EntranceExamMeritNo = dr["EntranceExamMeritNo"] as string,
                            ReferenceName = dr["ReferenceName"] as string
                        };
                    }
                }
            }
            return model;
        }

        public bool SaveAdmissionInformation(AdmissionInformationModel m)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionAdmissionInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@AdmissionSetupId", m.AdmissionSetupId);
                cmd.Parameters.AddWithValue("@CategoryId", m.CategoryId);
                cmd.Parameters.AddWithValue("@ApplicationId", m.ApplicationId);
                cmd.Parameters.AddWithValue("@AcademicStream", (object)m.AcademicStream ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AcademicSessionName", (object)m.AcademicSessionName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Degree", (object)m.Degree ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Branch", (object)m.Branch ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AcademicBatch", (object)m.AcademicBatch ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Enrollment", (object)m.Enrollment ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AcademicYear", (object)m.AcademicYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Semester", (object)m.Semester ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Scheme", (object)m.Scheme ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ClassSection", (object)m.ClassSection ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RollNumber", (object)m.RollNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateOfAdmission", (object)m.DateOfAdmission ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AdmissionCategory", (object)m.AdmissionCategory ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FeesCategory", (object)m.FeesCategory ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Shift", (object)m.Shift ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EntranceExamRegNo", (object)m.EntranceExamRegNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EntranceExamMeritNo", (object)m.EntranceExamMeritNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ReferenceName", (object)m.ReferenceName ?? DBNull.Value);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }
        // =========================================================
        // NEW ADMISSION → PARENT DETAILS
        // AdmissionService.cs class ke andar paste karo
        // =========================================================

        public ParentDetailsModel GetParentDetailsById(int applicationId)
        {
            ParentDetailsModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionParentDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new ParentDetailsModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"] as string,

                            FatherTitle = dr["FatherTitle"] as string,
                            FatherFirstName = dr["FatherFirstName"] as string,
                            FatherLastName = dr["FatherLastName"] as string,
                            FatherEmail = dr["FatherEmail"] as string,
                            FatherMobile = dr["FatherMobile"] as string,
                            FatherOccupation = dr["FatherOccupation"] as string,
                            FatherOrganization = dr["FatherOrganization"] as string,
                            FatherDesignation = dr["FatherDesignation"] as string,
                            FatherAnnualIncome = dr["FatherAnnualIncome"] as decimal?,

                            MotherTitle = dr["MotherTitle"] as string,
                            MotherFirstName = dr["MotherFirstName"] as string,
                            MotherLastName = dr["MotherLastName"] as string,
                            MotherEmail = dr["MotherEmail"] as string,
                            MotherMobile = dr["MotherMobile"] as string,
                            MotherOccupation = dr["MotherOccupation"] as string,
                            MotherOrganization = dr["MotherOrganization"] as string,
                            MotherDesignation = dr["MotherDesignation"] as string,
                            MotherAnnualIncome = dr["MotherAnnualIncome"] as decimal?,

                            GuardianTitle = dr["GuardianTitle"] as string,
                            GuardianFirstName = dr["GuardianFirstName"] as string,
                            GuardianLastName = dr["GuardianLastName"] as string,
                            GuardianEmail = dr["GuardianEmail"] as string,
                            GuardianMobile = dr["GuardianMobile"] as string,
                            GuardianOccupation = dr["GuardianOccupation"] as string,
                            GuardianOrganization = dr["GuardianOrganization"] as string,
                            GuardianDesignation = dr["GuardianDesignation"] as string,
                            GuardianAnnualIncome = dr["GuardianAnnualIncome"] as decimal?,
                            GuardianFamilyIncome = dr["GuardianFamilyIncome"] as string,
                            GuardianRelationship = dr["GuardianRelationship"] as string
                        };
                    }
                }
            }
            return model;
        }

        public bool SaveParentDetails(ParentDetailsModel m)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionParentDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@ApplicationId", m.ApplicationId);

                cmd.Parameters.AddWithValue("@FatherTitle", (object)m.FatherTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherFirstName", (object)m.FatherFirstName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherLastName", (object)m.FatherLastName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherEmail", (object)m.FatherEmail ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherMobile", (object)m.FatherMobile ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherOccupation", (object)m.FatherOccupation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherOrganization", (object)m.FatherOrganization ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherDesignation", (object)m.FatherDesignation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherAnnualIncome", (object)m.FatherAnnualIncome ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@MotherTitle", (object)m.MotherTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherFirstName", (object)m.MotherFirstName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherLastName", (object)m.MotherLastName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherEmail", (object)m.MotherEmail ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherMobile", (object)m.MotherMobile ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherOccupation", (object)m.MotherOccupation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherOrganization", (object)m.MotherOrganization ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherDesignation", (object)m.MotherDesignation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherAnnualIncome", (object)m.MotherAnnualIncome ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@GuardianTitle", (object)m.GuardianTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianFirstName", (object)m.GuardianFirstName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianLastName", (object)m.GuardianLastName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianEmail", (object)m.GuardianEmail ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianMobile", (object)m.GuardianMobile ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianOccupation", (object)m.GuardianOccupation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianOrganization", (object)m.GuardianOrganization ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianDesignation", (object)m.GuardianDesignation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianAnnualIncome", (object)m.GuardianAnnualIncome ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianFamilyIncome", (object)m.GuardianFamilyIncome ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GuardianRelationship", (object)m.GuardianRelationship ?? DBNull.Value);

                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }

        public BankDetailsModel GetBankDetailsById(int applicationId)
        {
            BankDetailsModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionBankDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new BankDetailsModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"] as string,
                            BankName = dr["BankName"] as string,
                            BranchName = dr["BranchName"] as string,
                            IFSCCode = dr["IFSCCode"] as string,
                            AccountHolderName = dr["AccountHolderName"] as string,
                            AccountNumber = dr["AccountNumber"] as string,
                            PANNumber = dr["PANNumber"] as string
                        };
                    }
                }
            }
            return model;
        }

        public bool SaveBankDetails(BankDetailsModel m)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NewAdmissionBankDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@ApplicationId", m.ApplicationId);
                cmd.Parameters.AddWithValue("@BankName", (object)m.BankName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BranchName", (object)m.BranchName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IFSCCode", (object)m.IFSCCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AccountHolderName", (object)m.AccountHolderName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AccountNumber", (object)m.AccountNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PANNumber", (object)m.PANNumber ?? DBNull.Value);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }
        // =========================================================
        // NEW ADMISSION → ACADEMIC RECORDS (multi-row per Application)
        // =========================================================

        public List<AcademicRecordModel> GetAcademicRecords(int applicationId)
        {
            var list = new List<AcademicRecordModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AcademicRecord", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new AcademicRecordModel
                        {
                            AcademicRecordId = Convert.ToInt32(dr["AcademicRecordId"]),
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ExamPassed = dr["ExamPassed"] as string,
                            Board = dr["Board"] as string,
                            Institute = dr["Institute"] as string,
                            Rank = dr["Rank"] as string,
                            RollNumber = dr["RollNumber"] as string,
                            PassingYear = dr["PassingYear"] as string,
                            ResultType = dr["ResultType"] as string,
                            Percentage = dr["Percentage"] as string,
                            Stream = dr["Stream"] as string,
                            EnrollmentNumber = dr["EnrollmentNumber"] as string,
                            MarksObtained = dr["MarksObtained"] as string,
                            MarksOutOf = dr["MarksOutOf"] as string,
                            Medium = dr["Medium"] as string,
                            Mode = dr["Mode"] as string,
                            GapYear = dr["GapYear"] as string,
                            GapReason = dr["GapReason"] as string,
                            ResultStatus = dr["ResultStatus"] as string
                        });
                    }
                }
            }
            return list;
        }

        public int InsertAcademicRecord(AcademicRecordModel m)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AcademicRecord", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@ApplicationId", m.ApplicationId);
                cmd.Parameters.AddWithValue("@ExamPassed", (object)m.ExamPassed ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Board", (object)m.Board ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Institute", (object)m.Institute ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rank", (object)m.Rank ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RollNumber", (object)m.RollNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PassingYear", (object)m.PassingYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ResultType", (object)m.ResultType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Percentage", (object)m.Percentage ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Stream", (object)m.Stream ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EnrollmentNumber", (object)m.EnrollmentNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MarksObtained", (object)m.MarksObtained ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MarksOutOf", (object)m.MarksOutOf ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Medium", (object)m.Medium ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Mode", (object)m.Mode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GapYear", (object)m.GapYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GapReason", (object)m.GapReason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ResultStatus", (object)m.ResultStatus ?? DBNull.Value);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // =========================================================
        // NEW ADMISSION → ADDITIONAL DETAILS (multi-row per Application)
        // =========================================================

        public List<AdditionalDetailModel> GetAdditionalDetails(int applicationId)
        {
            var list = new List<AdditionalDetailModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdditionalDetail", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new AdditionalDetailModel
                        {
                            AdditionalDetailId = Convert.ToInt32(dr["AdditionalDetailId"]),
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            Level = dr["Level"] as string,
                            ParticipationLevel = dr["ParticipationLevel"] as string,
                            Category = dr["Category"] as string,
                            AwardingInstitution = dr["AwardingInstitution"] as string,
                            AwardName = dr["AwardName"] as string,
                            ReceivedWhen = dr["ReceivedWhen"] as string,
                            Reason = dr["Reason"] as string
                        });
                    }
                }
            }
            return list;
        }

        public int InsertAdditionalDetail(AdditionalDetailModel m)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AdditionalDetail", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@ApplicationId", m.ApplicationId);
                cmd.Parameters.AddWithValue("@Level", (object)m.Level ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ParticipationLevel", (object)m.ParticipationLevel ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Category", (object)m.Category ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AwardingInstitution", (object)m.AwardingInstitution ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AwardName", (object)m.AwardName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ReceivedWhen", (object)m.ReceivedWhen ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Reason", (object)m.Reason ?? DBNull.Value);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // =========================================================
        // DASHBOARD : FEE COLLECTION SUMMARY
        // =========================================================

        public FeeSummaryModel GetFeeCollectionSummary()
        {
            var summary = new FeeSummaryModel();

            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETFEESUMMARY");
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // Result set 1 : totals
                    if (dr.Read())
                    {
                        summary.TotalCollected = Convert.ToDecimal(dr["TotalCollected"]);
                        summary.PendingCount = Convert.ToInt32(dr["PendingCount"]);
                        summary.EstimatedPendingAmount = Convert.ToDecimal(dr["EstimatedPendingAmount"]);
                    }

                    // Result set 2 : month-wise collected
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            summary.MonthlyCollection.Add(new FeeMonthlyModel
                            {
                                MonthName = dr["MonthName"].ToString(),
                                Collected = Convert.ToDecimal(dr["Collected"])
                            });
                        }
                    }
                }
            }
            return summary;
        }
        // =========================================================
        // REGISTRAR "ALL STUDENTS" OVERVIEW — Admission data se, dynamic
        // Registrar ke purane RegistrarStudentList/sp_RegistrarStudent se
        // bilkul alag hai, unhe touch nahi kiya.
        // =========================================================

        public List<RegistrarStudentOverviewModel> GetStudentOverview(
            int? courseId, string branch, int? sessionId, string semester, string searchText)
        {
            var list = new List<RegistrarStudentOverviewModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETSTUDENTOVERVIEW");
                cmd.Parameters.AddWithValue("@FilterCourseId", (object)courseId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterBranch", (object)branch ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterSessionId", (object)sessionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterSemester", (object)semester ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SearchText", (object)searchText ?? DBNull.Value);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var m = new RegistrarStudentOverviewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            RegistrationNumber = dr["RegistrationNumber"] as string,
                            UniversityEnrollmentNumber = dr["UniversityEnrollmentNumber"] as string,
                            CourseId = Convert.ToInt32(dr["CourseId"]),
                            CourseName = dr["CourseName"].ToString(),
                            Branch = dr["Branch"] as string,
                            AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                            SessionName = dr["SessionName"].ToString(),
                            Semester = dr["Semester"] as string,
                            Stage = dr["Stage"].ToString(),
                            DocVerified = Convert.ToBoolean(dr["DocVerified"]),
                            RequiredDocCount = Convert.ToInt32(dr["RequiredDocCount"]),
                            SubmittedDocCount = Convert.ToInt32(dr["SubmittedDocCount"])
                        };

                        m.DocumentStatus = m.RequiredDocCount == 0
                            ? "No Requirement"
                            : m.SubmittedDocCount >= m.RequiredDocCount
                                ? "Complete"
                                : m.SubmittedDocCount == 0
                                    ? "Pending"
                                    : "Deficient";

                        m.VerificationStatus = m.DocVerified ? "Verified" : "Verification Pending";
                        m.RegistrationStatus = m.Stage == "Admitted" || m.DocVerified ? "Active" : "Incomplete";

                        list.Add(m);
                    }
                }
            }
            return list;
        }
        public List<DocumentChecklistItemModel> GetSubmittedDocuments(int applicationId)
        {
            var list = new List<DocumentChecklistItemModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETSUBMITTEDDOCS");
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
                            IsSubmitted = true
                        });
                    }
                }
            }
            return list;
        }

        public StudentProfileModel GetStudentProfile(int applicationId)
        {
            StudentProfileModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Application", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETSTUDENTPROFILE");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new StudentProfileModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = dr["ApplicationNo"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            Email = dr["Email"] as string,
                            Phone = dr["Phone"] as string,
                            CourseName = dr["CourseName"].ToString(),
                            Branch = dr["Branch"] as string,
                            Semester = dr["Semester"] as string,
                            CategoryName = dr["CategoryName"] as string,
                            RegisteredOn = dr["RegisteredOn"] != DBNull.Value ? Convert.ToDateTime(dr["RegisteredOn"]) : (DateTime?)null,
                            Stage = dr["Stage"].ToString(),
                            DocVerified = Convert.ToBoolean(dr["DocVerified"]),
                            RequiredDocCount = Convert.ToInt32(dr["RequiredDocCount"]),
                            SubmittedDocCount = Convert.ToInt32(dr["SubmittedDocCount"])
                        };
                    }

                    if (model != null && dr.NextResult() && dr.Read())
                    {
                        model.FeeAmount = dr["FeeAmount"] as decimal?;
                        model.FeePaid = Convert.ToBoolean(dr["FeePaid"]);
                        model.FeeReceiptNo = dr["FeeReceiptNo"] as string;
                        model.FeeMode = dr["FeeMode"] as string;
                        model.FeePaymentDate = dr["FeePaymentDate"] as DateTime?;
                        model.AdmissionModeName = dr["AdmissionModeName"] as string;
                        model.AdmissionStatus = dr["AdmissionStatus"] as string;
                    }

                    if (model != null && dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            model.Documents.Add(new DocumentChecklistItemModel
                            {
                                DocumentEnclosureId = Convert.ToInt32(dr["DocumentEnclosureId"]),
                                DocumentName = dr["DocumentName"].ToString(),
                                IsMandatory = Convert.ToBoolean(dr["IsMandatory"]),
                                IsSubmitted = Convert.ToBoolean(dr["IsSubmitted"])
                            });
                        }
                    }
                }
            }
            return model;
        }
    }
}