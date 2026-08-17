using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    public class StudentMasterService
    {
        private readonly DBHelper db = new DBHelper();

        // =========================================================
        // 1) STUDENT RECORDS (master source)
        // =========================================================

        public List<StudentRecordModel> GetAllStudentRecords()
        {
            var list = new List<StudentRecordModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentRecords", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapStudentRecord(dr));
                }
            }
            return list;
        }

        public StudentRecordModel GetStudentRecordById(string studentId)
        {
            StudentRecordModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentRecords", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) model = MapStudentRecord(dr);
            }
            return model;
        }

        public bool UpdateStudentStatus(string studentId, string status)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentRecords", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATESTATUS");
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@Status", status);
                con.Open();
                return cmd.ExecuteNonQuery() != 0;
            }
        }

        private StudentRecordModel MapStudentRecord(SqlDataReader dr)
        {
            return new StudentRecordModel
            {
                StudentId = dr["StudentId"].ToString(),
                Name = dr["Name"] as string,
                CourseName = dr["CourseName"] as string,
                Category = dr["Category"] as string,
                Session = dr["Session"] as string,
                SeatNumber = dr["SeatNumber"] as string,
                AdmittedOn = Convert.ToDateTime(dr["AdmittedOn"]),
                Status = dr["Status"] as string
            };
        }


        // =========================================================
        // 2) STUDENT MAPPING (Section + Semester)
        // =========================================================

        public List<StudentMappingModel> GetStudentMappingList()
        {
            var list = new List<StudentMappingModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentMapping", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new StudentMappingModel
                        {
                            StudentId = dr["StudentId"].ToString(),
                            Name = dr["Name"] as string,
                            CourseName = dr["CourseName"] as string,
                            Section = dr["Section"] as string,
                            Semester = dr["Semester"] as int?,
                            IsMapped = Convert.ToInt32(dr["IsMapped"]) == 1
                        });
                    }
                }
            }
            return list;
        }

        public bool SaveStudentMapping(string studentId, string section, int semester)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentMapping", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "SAVE");
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@Section", section);
                cmd.Parameters.AddWithValue("@Semester", semester);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }


        // =========================================================
        // 3) IDENTITY GENERATION (Enrollment No / ID Card)
        // =========================================================

        public List<StudentIdentityListModel> GetIdentityList()
        {
            var list = new List<StudentIdentityListModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentIdentity", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new StudentIdentityListModel
                        {
                            StudentId = dr["StudentId"].ToString(),
                            Name = dr["Name"] as string,
                            CourseName = dr["CourseName"] as string,
                            EnrollmentNo = dr["EnrollmentNo"] as string,
                            IsGenerated = Convert.ToInt32(dr["IsGenerated"]) == 1
                        });
                    }
                }
            }
            return list;
        }

        public StudentIdentityDetailModel GetIdentityDetail(string studentId)
        {
            StudentIdentityDetailModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentIdentity", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETONE");
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new StudentIdentityDetailModel
                        {
                            StudentId = dr["StudentId"].ToString(),
                            Name = dr["Name"] as string,
                            CourseName = dr["CourseName"] as string,
                            Session = dr["Session"] as string,
                            EnrollmentNo = dr["EnrollmentNo"] as string
                        };
                    }
                }
            }
            return model;
        }

        public GenerateIdentityResult GenerateIdentity(string studentId)
        {
            GenerateIdentityResult result = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentIdentity", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GENERATE");
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        result = new GenerateIdentityResult
                        {
                            EnrollmentNo = dr["EnrollmentNo"].ToString(),
                            IsNew = Convert.ToInt32(dr["IsNew"]) == 1
                        };
                    }
                }
            }
            return result;
        }


        // =========================================================
        // 4) ACADEMIC PROGRESS
        // =========================================================

        public List<AcademicProgressModel> GetAcademicProgressList()
        {
            var list = new List<AcademicProgressModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AcademicProgress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new AcademicProgressModel
                        {
                            ProgressId = Convert.ToInt32(dr["ProgressId"]),
                            StudentId = dr["StudentId"].ToString(),
                            Name = dr["Name"] as string,
                            Semester = Convert.ToInt32(dr["Semester"]),
                            SGPA = Convert.ToDecimal(dr["SGPA"]),
                            Attendance = dr["Attendance"] as decimal?,
                            ResultStatus = dr["ResultStatus"] as string
                        });
                    }
                }
            }
            return list;
        }

        public List<StudentDropdownModel> GetStudentsForProgress()
        {
            return GetStudentDropdown("sp_AcademicProgress");
        }

        public int InsertAcademicProgress(AcademicProgressModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AcademicProgress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@Semester", model.Semester);
                cmd.Parameters.AddWithValue("@SGPA", model.SGPA);
                cmd.Parameters.AddWithValue("@Attendance", (object)model.Attendance ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ResultStatus", (object)model.ResultStatus ?? DBNull.Value);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }


        // =========================================================
        // 5) CERTIFICATE MANAGEMENT
        // =========================================================

        public List<CertificateIssuedModel> GetCertificateList()
        {
            var list = new List<CertificateIssuedModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_CertificateIssued", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new CertificateIssuedModel
                        {
                            CertificateId = Convert.ToInt32(dr["CertificateId"]),
                            CertNo = dr["CertNo"] as string,
                            StudentId = dr["StudentId"].ToString(),
                            Name = dr["Name"] as string,
                            CertificateType = dr["CertificateType"] as string,
                            Purpose = dr["Purpose"] as string,
                            IssuedOn = Convert.ToDateTime(dr["IssuedOn"])
                        });
                    }
                }
            }
            return list;
        }

        public List<StudentDropdownModel> GetStudentsForCertificate()
        {
            return GetStudentDropdown("sp_CertificateIssued");
        }

        public string IssueCertificate(CertificateIssuedModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_CertificateIssued", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@CertificateType", model.CertificateType);
                cmd.Parameters.AddWithValue("@Purpose", (object)model.Purpose ?? DBNull.Value);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read()) return dr["CertNo"].ToString();
            }
            return null;
        }


        // =========================================================
        // 6) ALUMNI
        // =========================================================

        public List<AlumniListItemModel> GetAlumniList()
        {
            var list = new List<AlumniListItemModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AlumniInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new AlumniListItemModel
                        {
                            StudentId = dr["StudentId"].ToString(),
                            Name = dr["Name"] as string,
                            CourseName = dr["CourseName"] as string,
                            Session = dr["Session"] as string,
                            Company = dr["Company"] as string,
                            Designation = dr["Designation"] as string
                        });
                    }
                }
            }
            return list;
        }

        public AlumniInfoModel GetAlumniInfoById(string studentId)
        {
            AlumniInfoModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AlumniInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new AlumniInfoModel
                        {
                            StudentId = dr["StudentId"].ToString(),
                            Company = dr["Company"] as string,
                            Designation = dr["Designation"] as string,
                            Email = dr["Email"] as string,
                            LinkedInUrl = dr["LinkedInUrl"] as string
                        };
                    }
                }
            }
            return model;
        }

        public bool SaveAlumniInfo(AlumniInfoModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_AlumniInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "SAVE");
                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@Company", (object)model.Company ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Designation", (object)model.Designation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LinkedInUrl", (object)model.LinkedInUrl ?? DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }


        // =========================================================
        // SHARED HELPER — "Select Student" dropdown (GETSTUDENTS flag)
        // =========================================================
        private List<StudentDropdownModel> GetStudentDropdown(string spName)
        {
            var list = new List<StudentDropdownModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETSTUDENTS");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new StudentDropdownModel
                        {
                            StudentId = dr["StudentId"].ToString(),
                            Name = dr["Name"] as string,
                            CourseName = dr["CourseName"] as string
                        });
                    }
                }
            }
            return list;
        }
        public List<DepartmentStudentCountModel> GetDepartmentWiseStudentCounts()//deparment dashboard graph
        {
            var list = new List<DepartmentStudentCountModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentRecords", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETDEPTWISE");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new DepartmentStudentCountModel
                        {
                            DepartmentName = dr["DepartmentName"].ToString(),
                            StudentCount = Convert.ToInt32(dr["StudentCount"])
                        });
                    }
                }
            }
            return list;
        }
    }
}