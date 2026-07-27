using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    // ============================================================
    // REGISTRAR STUDENT SERVICE
    // Same DBHelper + single-SP-with-@Flag pattern as every other
    // module in this project (see AcademicSetupService / MasterService).
    // Talks to sp_RegistrarStudent — see the accompanying .sql script.
    // ============================================================
    public class RegistrarService
    {
        private readonly DBHelper db = new DBHelper();

        public List<RegistrarStudentModel> GetAllRegistrarStudents()
        {
            var list = new List<RegistrarStudentModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapReader(dr));
                }
            }
            return list;
        }

        public RegistrarStudentModel GetRegistrarStudentById(int id)
        {
            RegistrarStudentModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@RegistrarId", id);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        model = MapReader(dr);
                }
            }
            return model;
        }

        public bool InsertRegistrarStudent(RegistrarStudentModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                AddCommonParams(cmd, model);
                con.Open();
                // SP has SET NOCOUNT ON -> successful INSERT returns -1, not the row count.
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateRegistrarStudent(RegistrarStudentModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@RegistrarId", model.RegistrarId);
                AddCommonParams(cmd, model);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteRegistrarStudent(int id)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@RegistrarId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        // Feeds the "Proceed to Documents" popup: which documents are
        // required for the Course currently selected in the form.
        // Reuses the existing sp_RequiredDocumentMaster @Flag =
        // 'GETDOCSFORCOURSE' (RequiredDocumentMaster -> RequiredDocumentDetail
        // -> DocumentEnclosureMaster, filtered by CourseId + IsActive) —
        // same one already used by Document Verification — instead of
        // duplicating that join here.
        public List<DocumentEnclosureModel> GetRequiredDocumentsByCourse(int courseId)
        {
            var list = new List<DocumentEnclosureModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RequiredDocumentMaster", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETDOCSFORCOURSE");
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new DocumentEnclosureModel
                        {
                            DocumentEnclosureId = Convert.ToInt32(dr["DocumentEnclosureId"]),
                            DocumentName = dr["DocumentName"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        private void AddCommonParams(SqlCommand cmd, RegistrarStudentModel model)
        {
            cmd.Parameters.AddWithValue("@StudentName", model.StudentName);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Mobile", (object)model.Mobile ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
            cmd.Parameters.AddWithValue("@BranchId", (object)model.BranchId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SemesterId", (object)model.SemesterId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RequiredDocumentIdsCsv", (object)model.RequiredDocumentIdsCsv ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubmittedDocumentIdsCsv", (object)model.SubmittedDocumentIdsCsv ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
        }

        private RegistrarStudentModel MapReader(SqlDataReader dr)
        {
            var m = new RegistrarStudentModel
            {
                RegistrarId = Convert.ToInt32(dr["RegistrarId"]),
                StudentName = dr["StudentName"].ToString(),
                Email = dr["Email"].ToString(),
                Mobile = dr["Mobile"] as string,
                CourseId = Convert.ToInt32(dr["CourseId"]),
                CourseName = dr["CourseName"].ToString(),
                BranchId = dr["BranchId"] != DBNull.Value ? Convert.ToInt32(dr["BranchId"]) : (int?)null,
                BranchName = dr["BranchName"] as string,
                SemesterId = dr["SemesterId"] != DBNull.Value ? Convert.ToInt32(dr["SemesterId"]) : (int?)null,
                SemesterName = dr["SemesterName"] as string,
                RequiredDocumentIdsCsv = dr["RequiredDocumentIdsCsv"] as string ?? "",
                RequiredDocumentNames = dr["RequiredDocumentNames"] as string ?? "",
                SubmittedDocumentIdsCsv = dr["SubmittedDocumentIdsCsv"] as string ?? "",
                SubmittedDocumentNames = dr["SubmittedDocumentNames"] as string ?? "",
                RequiredDocumentCount = dr["RequiredDocumentCount"] != DBNull.Value ? Convert.ToInt32(dr["RequiredDocumentCount"]) : 0,
                SubmittedDocumentCount = dr["SubmittedDocumentCount"] != DBNull.Value ? Convert.ToInt32(dr["SubmittedDocumentCount"]) : 0,
                IsActive = Convert.ToBoolean(dr["IsActive"]),
                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
            };

            m.DocumentStatus = m.RequiredDocumentCount == 0
                ? "No Requirement"
                : m.SubmittedDocumentCount == 0
                    ? "No"
                    : m.SubmittedDocumentCount >= m.RequiredDocumentCount
                        ? "Yes"
                        : "Partial";

            return m;
        }
    }
}