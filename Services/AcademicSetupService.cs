using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Regis.Services
{
    /// <summary>
    /// One combined service for the entire Academic Setup module.
    /// Every table (Faculty, Department, HOD, Program, Course, Semester,
    /// Subject, AcademicSession) is handled here so AcademicSetupController
    /// only ever needs to new-up this single class.
    ///
    /// FIX (applied throughout): stored procedures have "SET NOCOUNT ON",
    /// which makes ExecuteNonQuery() return -1 on a successful
    /// UPDATE/DELETE/ACTIVATE/CLOSE instead of the actual row count.
    /// So every "rows > 0" check has been changed to "rows != 0" —
    /// -1 (NOCOUNT case) and any positive row count both count as success,
    /// only 0 (nothing matched/changed) counts as failure.
    /// </summary>
    public class AcademicSetupService
    {
        private readonly DBHelper db = new DBHelper();

        // =========================================================
        // FACULTY  (Faculty of Engineering, Science...)
        // =========================================================

        // =========================================================
        // FACULTY ASSIGNMENT MASTER
        // =========================================================

        public List<FacultyModel> GetAllFacultyAssignments()
        {
            List<FacultyModel> list =
                new List<FacultyModel>();

            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand("sp_FacultyAssignment", con))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Flag", "GETALL");

                con.Open();

                using (SqlDataReader dr =
                    cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new FacultyModel
                        {
                            FacultyId =
                                Convert.ToInt32(dr["FacultyId"]),

                            EmployeeId =
                                Convert.ToInt32(dr["EmployeeId"]),

                            FacultyName =
                                dr["FacultyName"].ToString(),

                            DepartmentId =
                                Convert.ToInt32(dr["DepartmentId"]),

                            DepartmentName =
                                dr["DepartmentName"].ToString(),

                            BranchId =
                                Convert.ToInt32(dr["BranchId"]),

                            BranchName =
                                dr["BranchName"].ToString(),

                            SemesterId =
                                Convert.ToInt32(dr["SemesterId"]),

                            SemesterName =
                                dr["SemesterName"].ToString(),

                            DesignationId =
                                Convert.ToInt32(dr["DesignationId"]),

                            DesignationName =
                                dr["DesignationName"].ToString(),

                            Status =
                                dr["Status"].ToString(),

                            CreatedDate =
                                dr["CreatedDate"] != DBNull.Value
                                ? Convert.ToDateTime(
                                    dr["CreatedDate"])
                                : (DateTime?)null
                        });
                    }
                }
            }

            return list;
        }


        public bool InsertFacultyAssignment(FacultyModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand("sp_FacultyAssignment", con))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Flag", "INSERT");

                cmd.Parameters.AddWithValue(
                    "@EmployeeId", model.EmployeeId);

                cmd.Parameters.AddWithValue(
                    "@DepartmentId", model.DepartmentId);

                cmd.Parameters.AddWithValue(
                    "@BranchId", model.BranchId);

                cmd.Parameters.AddWithValue(
                    "@SemesterId", model.SemesterId);

                cmd.Parameters.AddWithValue(
                    "@DesignationId", model.DesignationId);

                cmd.Parameters.AddWithValue(
                    "@Status", model.Status ?? "Active");

                con.Open();

                // SP has SET NOCOUNT ON -> successful INSERT returns -1, not the row count.
                // != 0 treats both -1 (NOCOUNT case) and any positive count as success.
                return cmd.ExecuteNonQuery() != 0;
            }
        }


        public bool UpdateFacultyAssignment(FacultyModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand("sp_FacultyAssignment", con))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Flag", "UPDATE");

                cmd.Parameters.AddWithValue(
                    "@FacultyId", model.FacultyId);

                cmd.Parameters.AddWithValue(
                    "@EmployeeId", model.EmployeeId);

                cmd.Parameters.AddWithValue(
                    "@DepartmentId", model.DepartmentId);

                cmd.Parameters.AddWithValue(
                    "@BranchId", model.BranchId);

                cmd.Parameters.AddWithValue(
                    "@SemesterId", model.SemesterId);

                cmd.Parameters.AddWithValue(
                    "@DesignationId", model.DesignationId);

                cmd.Parameters.AddWithValue(
                    "@Status", model.Status);

                con.Open();

                // /SP has SET NOCOUNT ON -> successful UPDATE returns -1, not the row count.
                // != 0 treats both -1 (NOCOUNT case) and any positive count as success.
                return cmd.ExecuteNonQuery() != 0;
            }
        }


        public bool DeleteFacultyAssignment(int id)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd =
                new SqlCommand("sp_FacultyAssignment", con))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Flag", "DELETE");

                cmd.Parameters.AddWithValue(
                    "@FacultyId", id);

                con.Open();

                // SP has SET NOCOUNT ON -> successful DELETE returns -1, not the row count.
                // != 0 treats both -1 (NOCOUNT case) and any positive count as success.
                return cmd.ExecuteNonQuery() != 0;
            }
        }
        // =========================================================
        // DEPARTMENT
        // FacultyId -> FK to Faculty
        // =========================================================

        public List<DepartmentModel> GetAllDepartments()
        {
            var list = new List<DepartmentModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Department", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DepartmentModel
                    {
                        DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                        DepartmentCode = dr["DepartmentCode"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        FacultyId = Convert.ToInt32(dr["FacultyId"]),
                        FacultyName = dr["FacultyName"].ToString(),
                        CampusName = dr["CampusName"].ToString(),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public List<DepartmentModel> GetActiveDepartments()
        {
            var list = new List<DepartmentModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Department", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DepartmentModel
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
        public List<DepartmentModel> GetDepartmentsByFaculty(int facultyId)
        {
            var list = new List<DepartmentModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Department", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYFACULTY");
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DepartmentModel
                    {
                        DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                        DepartmentCode = dr["DepartmentCode"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString()
                    });
                }
            }
            return list;
        }

        public bool InsertDepartment(DepartmentModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Department", con);
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

        public bool UpdateDepartment(DepartmentModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Department", con);
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

        public bool DeleteDepartment(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Department", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@DepartmentId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // HOD MANAGEMENT
        // DepartmentId -> FK to Department
        // =========================================================

        public List<HODManagementModel> GetAllHODRows()
        {
            var list = new List<HODManagementModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_HODManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new HODManagementModel
                    {
                        HODId = Convert.ToInt32(dr["HODId"]),
                        DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        FacultyName = dr["FacultyName"].ToString(),
                        HODName = dr["HODName"].ToString(),
                        EmployeeId = dr["EmployeeId"] as string,
                        Designation = dr["Designation"] as string,
                        Qualification = dr["Qualification"] as string,
                        Email = dr["Email"] as string,
                        Phone = dr["Phone"] as string,
                        EffectiveDate = dr["EffectiveDate"] != DBNull.Value ? Convert.ToDateTime(dr["EffectiveDate"]) : (DateTime?)null,
                        TenureEndDate = dr["TenureEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["TenureEndDate"]) : (DateTime?)null,
                        ReasonForChange = dr["ReasonForChange"] as string,
                        Status = dr["Status"].ToString()
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Builds the exact "departments" shape the HODManagement.cshtml JS
        /// array already expects: one entry per Department, with current
        /// HOD info + full history — so the existing front-end JS keeps working
        /// unchanged, just fed from real data instead of a hardcoded array.
        /// </summary>
        public List<DepartmentHODOverviewModel> GetDepartmentHODOverview()
        {
            var departments = GetAllDepartments();
            var allHodRows = GetAllHODRows();

            var result = new List<DepartmentHODOverviewModel>();

            foreach (var dept in departments)
            {
                var rowsForDept = allHodRows.Where(h => h.DepartmentId == dept.DepartmentId)
                                             .OrderByDescending(h => h.EffectiveDate)
                                             .ToList();

                var current = rowsForDept.FirstOrDefault(h => h.Status == "Active");
                var history = rowsForDept.Where(h => h.Status != "Active")
                    .Select(h => new HODHistoryEntry
                    {
                        Name = h.HODName,
                        From = h.EffectiveDate?.ToString("yyyy-MM-dd"),
                        To = h.TenureEndDate?.ToString("yyyy-MM-dd") ?? "-",
                        Reason = h.ReasonForChange ?? "Reassigned",
                        Status = "Completed"
                    }).ToList();

                result.Add(new DepartmentHODOverviewModel
                {
                    DeptId = dept.DepartmentId,
                    Dept = dept.DepartmentName,
                    Faculty = dept.FacultyName,
                    Hod = current?.HODName ?? "-",
                    EmpId = current?.EmployeeId ?? "-",
                    Email = current?.Email ?? "-",
                    Phone = current?.Phone ?? "-",
                    Qualification = current?.Qualification ?? "-",
                    Designation = current?.Designation ?? "-",
                    Date = current?.EffectiveDate?.ToString("yyyy-MM-dd") ?? "-",
                    TenureEnd = current?.TenureEndDate?.ToString("yyyy-MM-dd") ?? "",
                    Status = current != null ? current.Status : "Vacant",
                    History = history
                });
            }

            return result;
        }

        // INSERT here automatically relieves the previous HOD of that department (handled inside the SP)
        public bool AssignHOD(HODManagementModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_HODManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@DepartmentId", model.DepartmentId);
                cmd.Parameters.AddWithValue("@HODName", model.HODName);
                cmd.Parameters.AddWithValue("@EmployeeId", (object)model.EmployeeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Designation", (object)model.Designation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Qualification", (object)model.Qualification ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object)model.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EffectiveDate", (object)model.EffectiveDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TenureEndDate", (object)model.TenureEndDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ReasonForChange", (object)model.ReasonForChange ?? DBNull.Value);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // PROGRAM  (UG / PG / Diploma / Certificate)
        // =========================================================

        public List<ProgramModel> GetAllPrograms()
        {
            var list = new List<ProgramModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Program", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new ProgramModel
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

        public List<ProgramModel> GetActivePrograms()
        {
            var list = new List<ProgramModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Program", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new ProgramModel
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

        public bool InsertProgram(ProgramModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Program", con);
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

        public bool UpdateProgram(ProgramModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Program", con);
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

        public bool DeleteProgram(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Program", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@ProgramId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // COURSE
        // ProgramId -> FK to Program, DepartmentId -> FK to Department
        // =========================================================

        public List<CourseModel> GetAllCourses()
        {
            var list = new List<CourseModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Course", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CourseModel
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

        public List<CourseModel> GetActiveCourses()
        {
            var list = new List<CourseModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Course", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CourseModel
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
        public List<CourseModel> GetCoursesByDepartment(int departmentId)
        {
            var list = new List<CourseModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Course", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYDEPARTMENT");
                cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CourseModel
                    {
                        CourseId = Convert.ToInt32(dr["CourseId"]),
                        CourseCode = dr["CourseCode"].ToString(),
                        CourseName = dr["CourseName"].ToString()
                    });
                }
            }
            return list;
        }

        public bool InsertCourse(CourseModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Course", con);
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

        public bool UpdateCourse(CourseModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Course", con);
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

        public bool DeleteCourse(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Course", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@CourseId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // ACADEMIC SESSION
        // SessionTypeId -> FK to SessionType (Masters)
        // =========================================================

        public List<AcademicSessionModel> GetAllSessions()
        {
            var list = new List<AcademicSessionModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new AcademicSessionModel
                    {
                        AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                        SessionName = dr["SessionName"].ToString(),
                        SessionCode = dr["SessionCode"].ToString(),
                        SessionTypeId = Convert.ToInt32(dr["SessionTypeId"]),
                        SessionTypeName = dr["SessionTypeName"].ToString(),
                        StartDate = Convert.ToDateTime(dr["StartDate"]),
                        EndDate = Convert.ToDateTime(dr["EndDate"]),
                        AcademicYear = dr["AcademicYear"] as string,
                        Status = dr["Status"].ToString(),
                        MaxCredits = dr["MaxCredits"] != DBNull.Value ? Convert.ToInt32(dr["MaxCredits"]) : (int?)null,
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public List<AcademicSessionModel> GetActiveSessions()
        {
            var list = new List<AcademicSessionModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new AcademicSessionModel
                    {
                        AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                        SessionName = dr["SessionName"].ToString()
                    });
                }
            }
            return list;
        }

        public int InsertSession(AcademicSessionModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@SessionName", model.SessionName);
                cmd.Parameters.AddWithValue("@SessionCode", model.SessionCode);
                cmd.Parameters.AddWithValue("@SessionTypeId", model.SessionTypeId);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@AcademicYear", (object)model.AcademicYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaxCredits", (object)model.MaxCredits ?? DBNull.Value);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public AcademicSessionModel GetSessionById(int id)
        {
            AcademicSessionModel model = null;
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@AcademicSessionId", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    model = new AcademicSessionModel
                    {
                        AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                        SessionName = dr["SessionName"].ToString(),
                        SessionCode = dr["SessionCode"].ToString(),
                        SessionTypeId = Convert.ToInt32(dr["SessionTypeId"]),
                        StartDate = Convert.ToDateTime(dr["StartDate"]),
                        EndDate = Convert.ToDateTime(dr["EndDate"]),
                        AcademicYear = dr["AcademicYear"] as string,
                        Status = dr["Status"].ToString(),
                        MaxCredits = dr["MaxCredits"] != DBNull.Value ? Convert.ToInt32(dr["MaxCredits"]) : (int?)null
                    };
                }
            }
            return model;
        }

        public bool UpdateSession(AcademicSessionModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@AcademicSessionId", model.AcademicSessionId);
                cmd.Parameters.AddWithValue("@SessionName", model.SessionName);
                cmd.Parameters.AddWithValue("@SessionCode", model.SessionCode);
                cmd.Parameters.AddWithValue("@SessionTypeId", model.SessionTypeId);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@AcademicYear", (object)model.AcademicYear ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaxCredits", (object)model.MaxCredits ?? DBNull.Value);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                // SP has SET NOCOUNT ON -> successful UPDATE returns -1, not the actual row count.
                // != 0 treats both -1 (NOCOUNT case) and any positive count as success.
                return rows != 0;
            }
        }

        public bool DeleteSession(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@AcademicSessionId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool ActivateSession(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "ACTIVATE");
                cmd.Parameters.AddWithValue("@AcademicSessionId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool CloseSession(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AcademicSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "CLOSE");
                cmd.Parameters.AddWithValue("@AcademicSessionId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // SEMESTER
        // CourseId -> FK to Course, AcademicSessionId -> FK to AcademicSession
        // =========================================================

        public List<SemesterModel> GetAllSemesters()
        {
            var list = new List<SemesterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Semester", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SemesterModel
                    {
                        SemesterId = Convert.ToInt32(dr["SemesterId"]),
                        CourseId = Convert.ToInt32(dr["CourseId"]),
                        CourseName = dr["CourseName"].ToString(),
                        AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                        SessionName = dr["SessionName"].ToString(),
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

        // Cascading dropdown: semesters under one Course — used by Subject
        public List<SemesterModel> GetSemestersByCourse(int courseId)
        {
            var list = new List<SemesterModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Semester", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYCOURSE");
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SemesterModel
                    {
                        SemesterId = Convert.ToInt32(dr["SemesterId"]),
                        SemesterNumber = Convert.ToInt32(dr["SemesterNumber"]),
                        SemesterName = dr["SemesterName"] as string
                    });
                }
            }
            return list;
        }

        public bool InsertSemester(SemesterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Semester", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@AcademicSessionId", model.AcademicSessionId);
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

        public bool UpdateSemester(SemesterModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Semester", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@SemesterId", model.SemesterId);
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@AcademicSessionId", model.AcademicSessionId);
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

        public bool DeleteSemester(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Semester", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@SemesterId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // =========================================================
        // SUBJECT
        // SemesterId -> FK to Semester, CourseId -> FK to Course
        // =========================================================

        public List<SubjectModel> GetAllSubjects()
        {
            var list = new List<SubjectModel>();
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Subject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SubjectModel
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

        public bool InsertSubject(SubjectModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Subject", con);
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

        public bool UpdateSubject(SubjectModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Subject", con);
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

        public bool DeleteSubject(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Subject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@SubjectId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }
        // =====================================================    required document
        //============================================================================
        //=================================================================
        //             Required Document Master
        //=================================================================

        public List<RequiredDocumentModel> GetAllRequiredDocuments()
        {
            var list = new List<RequiredDocumentModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RequiredDocumentMaster", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new RequiredDocumentModel
                        {
                            RequiredDocumentId = Convert.ToInt32(dr["RequiredDocumentId"]),
                            AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                            SessionName = dr["SessionName"].ToString(),
                            AdmissionModeId = Convert.ToInt32(dr["AdmissionModeId"]),
                            AdmissionModeName = dr["AdmissionModeName"].ToString(),
                            ProgramId = Convert.ToInt32(dr["ProgramId"]),
                            ProgramName = dr["ProgramName"].ToString(),
                            CourseId = Convert.ToInt32(dr["CourseId"]),
                            CourseName = dr["CourseName"].ToString(),
                            CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            CategoryName = dr["CategoryName"].ToString(),
                            IsActive = Convert.ToBoolean(dr["IsActive"]),
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null,
                            DocumentEnclosureIdsCsv = dr["DocumentEnclosureIdsCsv"] as string ?? "",
                            RequiredDocumentNames = dr["RequiredDocumentNames"] as string ?? ""
                        });
                    }
                }
            }
            return list;
        }

        public RequiredDocumentModel GetRequiredDocumentById(int id)
        {
            RequiredDocumentModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RequiredDocumentMaster", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@RequiredDocumentId", id);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new RequiredDocumentModel
                        {
                            RequiredDocumentId = Convert.ToInt32(dr["RequiredDocumentId"]),
                            AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                            AdmissionModeId = Convert.ToInt32(dr["AdmissionModeId"]),
                            ProgramId = Convert.ToInt32(dr["ProgramId"]),
                            CourseId = Convert.ToInt32(dr["CourseId"]),
                            CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            IsActive = Convert.ToBoolean(dr["IsActive"])
                        };
                    }

                    // second result set -> selected document ids
                    if (model != null && dr.NextResult())
                    {
                        var ids = new List<int>();
                        while (dr.Read())
                            ids.Add(Convert.ToInt32(dr["DocumentEnclosureId"]));
                        model.DocumentEnclosureIds = ids;
                    }
                }
            }
            return model;
        }

        public bool InsertRequiredDocument(RequiredDocumentModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RequiredDocumentMaster", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@AcademicSessionId", model.AcademicSessionId);
                cmd.Parameters.AddWithValue("@AdmissionModeId", model.AdmissionModeId);
                cmd.Parameters.AddWithValue("@ProgramId", model.ProgramId);
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@IsActive", true);
                cmd.Parameters.AddWithValue("@DocumentEnclosureIds", model.DocumentEnclosureIdsCsv);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateRequiredDocument(RequiredDocumentModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RequiredDocumentMaster", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@RequiredDocumentId", model.RequiredDocumentId);
                cmd.Parameters.AddWithValue("@AcademicSessionId", model.AcademicSessionId);
                cmd.Parameters.AddWithValue("@AdmissionModeId", model.AdmissionModeId);
                cmd.Parameters.AddWithValue("@ProgramId", model.ProgramId);
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@DocumentEnclosureIds", model.DocumentEnclosureIdsCsv);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteRequiredDocument(int id)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RequiredDocumentMaster", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@RequiredDocumentId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }
    }
}