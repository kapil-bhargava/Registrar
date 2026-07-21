using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    /// <summary>
    /// One combined service for all Master tables:
    /// FacultyMaster, DepartmentMaster, ProgramMaster, CourseMaster,
    /// SemesterMaster, SubjectMaster.
    /// Every SP has SET NOCOUNT ON, so successful UPDATE/DELETE returns -1
    /// instead of the real row count -> every check is "rows != 0"
    /// (0 = nothing matched = failure, -1 or positive = success).
    /// </summary>
    public class MasterService
    {
        private readonly DBHelper db = new DBHelper();

        // =========================================================
        // FACULTY MASTER
        // =========================================================

        public List<FacultyMasterModel> GetAllFacultyMaster()
        {
            var list = new List<FacultyMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_FacultyMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new FacultyMasterModel
                    {
                        FacultyId = Convert.ToInt32(dr["FacultyId"]),
                        FacultyCode = dr["FacultyCode"].ToString(),
                        FacultyName = dr["FacultyName"].ToString(),
                        Description = dr["Description"] as string,
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public List<FacultyMasterModel> GetActiveFacultyMaster()
        {
            var list = new List<FacultyMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_FacultyMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new FacultyMasterModel
                    {
                        FacultyId = Convert.ToInt32(dr["FacultyId"]),
                        FacultyCode = dr["FacultyCode"].ToString(),
                        FacultyName = dr["FacultyName"].ToString()
                    });
                }
            }
            return list;
        }

        public bool InsertFacultyMaster(FacultyMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_FacultyMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@FacultyCode", model.FacultyCode);
                cmd.Parameters.AddWithValue("@FacultyName", model.FacultyName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateFacultyMaster(FacultyMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_FacultyMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@FacultyId", model.FacultyId);
                cmd.Parameters.AddWithValue("@FacultyCode", model.FacultyCode);
                cmd.Parameters.AddWithValue("@FacultyName", model.FacultyName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteFacultyMaster(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_FacultyMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@FacultyId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // DEPARTMENT MASTER
        // FacultyId -> FK FacultyMaster
        // =========================================================

        public List<DepartmentMasterModel> GetAllDepartmentMaster()
        {
            var list = new List<DepartmentMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DepartmentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DepartmentMasterModel
                    {
                        DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                        DepartmentCode = dr["DepartmentCode"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        FacultyId = Convert.ToInt32(dr["FacultyId"]),
                        FacultyName = dr["FacultyName"].ToString(),
                        CampusName = dr["CampusName"] as string,
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public List<DepartmentMasterModel> GetActiveDepartmentMaster()
        {
            var list = new List<DepartmentMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DepartmentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DepartmentMasterModel
                    {
                        DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                        DepartmentCode = dr["DepartmentCode"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        FacultyId = Convert.ToInt32(dr["FacultyId"])
                    });
                }
            }
            return list;
        }

        // Cascading dropdown: departments under one Faculty
        public List<DepartmentMasterModel> GetDepartmentMasterByFaculty(int facultyId)
        {
            var list = new List<DepartmentMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DepartmentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYFACULTY");
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DepartmentMasterModel
                    {
                        DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                        DepartmentCode = dr["DepartmentCode"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString()
                    });
                }
            }
            return list;
        }

        public bool InsertDepartmentMaster(DepartmentMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DepartmentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@DepartmentCode", model.DepartmentCode);
                cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                cmd.Parameters.AddWithValue("@FacultyId", model.FacultyId);
                cmd.Parameters.AddWithValue("@CampusName", (object)model.CampusName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateDepartmentMaster(DepartmentMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DepartmentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@DepartmentId", model.DepartmentId);
                cmd.Parameters.AddWithValue("@DepartmentCode", model.DepartmentCode);
                cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                cmd.Parameters.AddWithValue("@FacultyId", model.FacultyId);
                cmd.Parameters.AddWithValue("@CampusName", (object)model.CampusName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteDepartmentMaster(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DepartmentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@DepartmentId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // PROGRAM MASTER
        // =========================================================

        public List<ProgramMasterModel> GetAllProgramMaster()
        {
            var list = new List<ProgramMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ProgramMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new ProgramMasterModel
                    {
                        ProgramId = Convert.ToInt32(dr["ProgramId"]),
                        ProgramCode = dr["ProgramCode"].ToString(),
                        ProgramName = dr["ProgramName"].ToString(),
                        ProgramType = dr["ProgramType"].ToString(),
                        TypicalDuration = dr["TypicalDuration"] as string,
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public List<ProgramMasterModel> GetActiveProgramMaster()
        {
            var list = new List<ProgramMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ProgramMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new ProgramMasterModel
                    {
                        ProgramId = Convert.ToInt32(dr["ProgramId"]),
                        ProgramCode = dr["ProgramCode"].ToString(),
                        ProgramName = dr["ProgramName"].ToString(),
                        ProgramType = dr["ProgramType"].ToString()
                    });
                }
            }
            return list;
        }

        public bool InsertProgramMaster(ProgramMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ProgramMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@ProgramCode", model.ProgramCode);
                cmd.Parameters.AddWithValue("@ProgramName", model.ProgramName);
                cmd.Parameters.AddWithValue("@ProgramType", model.ProgramType);
                cmd.Parameters.AddWithValue("@TypicalDuration", (object)model.TypicalDuration ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateProgramMaster(ProgramMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ProgramMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@ProgramId", model.ProgramId);
                cmd.Parameters.AddWithValue("@ProgramCode", model.ProgramCode);
                cmd.Parameters.AddWithValue("@ProgramName", model.ProgramName);
                cmd.Parameters.AddWithValue("@ProgramType", model.ProgramType);
                cmd.Parameters.AddWithValue("@TypicalDuration", (object)model.TypicalDuration ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteProgramMaster(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ProgramMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@ProgramId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // COURSE MASTER
        // ProgramId -> FK ProgramMaster, DepartmentId -> FK DepartmentMaster
        // =========================================================

        public List<CourseMasterModel> GetAllCourseMaster()
        {
            var list = new List<CourseMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CourseMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CourseMasterModel
                    {
                        CourseId = Convert.ToInt32(dr["CourseId"]),
                        CourseCode = dr["CourseCode"].ToString(),
                        CourseName = dr["CourseName"].ToString(),
                        ProgramId = Convert.ToInt32(dr["ProgramId"]),
                        ProgramName = dr["ProgramName"].ToString(),
                        DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        DurationYears = dr["DurationYears"] != DBNull.Value ? Convert.ToInt32(dr["DurationYears"]) : (int?)null,
                        TotalSemesters = dr["TotalSemesters"] != DBNull.Value ? Convert.ToInt32(dr["TotalSemesters"]) : (int?)null,
                        TotalCredits = dr["TotalCredits"] != DBNull.Value ? Convert.ToInt32(dr["TotalCredits"]) : (int?)null,
                        IntakeCapacity = dr["IntakeCapacity"] != DBNull.Value ? Convert.ToInt32(dr["IntakeCapacity"]) : (int?)null,
                        Status = dr["Status"].ToString(),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public List<CourseMasterModel> GetActiveCourseMaster()
        {
            var list = new List<CourseMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CourseMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CourseMasterModel
                    {
                        CourseId = Convert.ToInt32(dr["CourseId"]),
                        CourseCode = dr["CourseCode"].ToString(),
                        CourseName = dr["CourseName"].ToString(),
                        TotalSemesters = dr["TotalSemesters"] != DBNull.Value ? Convert.ToInt32(dr["TotalSemesters"]) : (int?)null
                    });
                }
            }
            return list;
        }

        // Cascading dropdown: courses under one Department
        public List<CourseMasterModel> GetCourseMasterByDepartment(int departmentId)
        {
            var list = new List<CourseMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CourseMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYDEPARTMENT");
                cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CourseMasterModel
                    {
                        CourseId = Convert.ToInt32(dr["CourseId"]),
                        CourseCode = dr["CourseCode"].ToString(),
                        CourseName = dr["CourseName"].ToString()
                    });
                }
            }
            return list;
        }

        public bool InsertCourseMaster(CourseMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CourseMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@CourseCode", model.CourseCode);
                cmd.Parameters.AddWithValue("@CourseName", model.CourseName);
                cmd.Parameters.AddWithValue("@ProgramId", model.ProgramId);
                cmd.Parameters.AddWithValue("@DepartmentId", model.DepartmentId);
                cmd.Parameters.AddWithValue("@DurationYears", (object)model.DurationYears ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalSemesters", (object)model.TotalSemesters ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalCredits", (object)model.TotalCredits ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IntakeCapacity", (object)model.IntakeCapacity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status ?? "Active");
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateCourseMaster(CourseMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CourseMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@CourseCode", model.CourseCode);
                cmd.Parameters.AddWithValue("@CourseName", model.CourseName);
                cmd.Parameters.AddWithValue("@ProgramId", model.ProgramId);
                cmd.Parameters.AddWithValue("@DepartmentId", model.DepartmentId);
                cmd.Parameters.AddWithValue("@DurationYears", (object)model.DurationYears ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalSemesters", (object)model.TotalSemesters ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalCredits", (object)model.TotalCredits ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IntakeCapacity", (object)model.IntakeCapacity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteCourseMaster(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CourseMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@CourseId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // SEMESTER MASTER
        // CourseId -> FK CourseMaster, AcademicSessionId -> FK AcademicSession
        // =========================================================

        public List<SemesterMasterModel> GetAllSemesterMaster()
        {
            var list = new List<SemesterMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SemesterMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SemesterMasterModel
                    {
                        SemesterId = Convert.ToInt32(dr["SemesterId"]),
                        CourseId = Convert.ToInt32(dr["CourseId"]),
                        CourseName = dr["CourseName"].ToString(),
                        SessionName = dr["AcademicSessionName"] as string,
                        SemesterNumber = Convert.ToInt32(dr["SemesterNumber"]),
                        SemesterName = dr["SemesterName"] as string,
                        StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : (DateTime?)null,
                        EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : (DateTime?)null,
                        CreditLimit = dr["CreditLimit"] != DBNull.Value ? Convert.ToInt32(dr["CreditLimit"]) : (int?)null,
                        Status = dr["Status"].ToString(),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }
        public List<string> GetDistinctAcademicSessionNames()
        {
            var list = new List<string>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SemesterMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETDISTINCTSESSIONS");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(dr["AcademicSessionName"].ToString());
                }
            }
            return list;
        }
        // Cascading dropdown: semesters under one Course — used by Subject
        public List<SemesterMasterModel> GetSemesterMasterByCourse(int courseId)
        {
            var list = new List<SemesterMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SemesterMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYCOURSE");
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SemesterMasterModel
                    {
                        SemesterId = Convert.ToInt32(dr["SemesterId"]),
                        SemesterNumber = Convert.ToInt32(dr["SemesterNumber"]),
                        SemesterName = dr["SemesterName"] as string
                    });
                }
            }
            return list;
        }

        public bool InsertSemesterMaster(SemesterMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SemesterMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@AcademicSessionName", (object)model.AcademicSessionName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SemesterNumber", model.SemesterNumber);
                cmd.Parameters.AddWithValue("@SemesterName", (object)model.SemesterName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", (object)model.StartDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", (object)model.EndDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreditLimit", (object)model.CreditLimit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status ?? "Upcoming");
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateSemesterMaster(SemesterMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SemesterMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@SemesterId", model.SemesterId);
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@AcademicSessionName", (object)model.AcademicSessionName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SemesterNumber", model.SemesterNumber);
                cmd.Parameters.AddWithValue("@SemesterName", (object)model.SemesterName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", (object)model.StartDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", (object)model.EndDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreditLimit", (object)model.CreditLimit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }
        public bool DeleteSemesterMaster(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SemesterMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@SemesterId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // ACADEMIC SESSION (existing table — not part of this "Master"
        // rename, but SemesterMaster's form still needs the dropdown)
        // =========================================================

        public List<Regis.Models.AcademicSessionModel> GetActiveAcademicSessionsForDropdown()
        {
            var list = new List<Regis.Models.AcademicSessionModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Regis.Models.AcademicSessionModel
                    {
                        AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                        SessionName = dr["SessionName"].ToString()
                    });
                }
            }
            return list;
        }

        // =========================================================
        // SUBJECT MASTER
        // SemesterId -> FK SemesterMaster, CourseId -> FK CourseMaster
        // =========================================================

        public List<SubjectMasterModel> GetAllSubjectMaster()
        {
            var list = new List<SubjectMasterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SubjectMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SubjectMasterModel
                    {
                        SubjectId = Convert.ToInt32(dr["SubjectId"]),
                        SubjectCode = dr["SubjectCode"].ToString(),
                        SubjectName = dr["SubjectName"].ToString(),
                        SubjectType = dr["SubjectType"].ToString(),
                        SemesterId = Convert.ToInt32(dr["SemesterId"]),
                        SemesterName = dr["SemesterName"] as string,
                        CourseId = Convert.ToInt32(dr["CourseId"]),
                        CourseName = dr["CourseName"].ToString(),
                        Credits = dr["Credits"] != DBNull.Value ? Convert.ToInt32(dr["Credits"]) : (int?)null,
                        Status = dr["Status"].ToString(),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public bool InsertSubjectMaster(SubjectMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SubjectMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@SubjectCode", model.SubjectCode);
                cmd.Parameters.AddWithValue("@SubjectName", model.SubjectName);
                cmd.Parameters.AddWithValue("@SubjectType", model.SubjectType);
                cmd.Parameters.AddWithValue("@SemesterId", model.SemesterId);
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@Credits", (object)model.Credits ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status ?? "Active");
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateSubjectMaster(SubjectMasterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SubjectMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@SubjectId", model.SubjectId);
                cmd.Parameters.AddWithValue("@SubjectCode", model.SubjectCode);
                cmd.Parameters.AddWithValue("@SubjectName", model.SubjectName);
                cmd.Parameters.AddWithValue("@SubjectType", model.SubjectType);
                cmd.Parameters.AddWithValue("@SemesterId", model.SemesterId);
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@Credits", (object)model.Credits ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteSubjectMaster(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SubjectMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@SubjectId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }
    }
}