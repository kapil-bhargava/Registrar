using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    public class UniversityTypeService
    {
        // Database Helper
        private readonly DBHelper db = new DBHelper();

        //=========================================================
        // Get All University Types
        //=========================================================

        public List<UniversityTypeModel> GetAllUniversityTypes()
        {
            List<UniversityTypeModel> list = new List<UniversityTypeModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UniversityType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "GETALL");

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new UniversityTypeModel
                    {
                        UniversityTypeId = Convert.ToInt32(dr["UniversityTypeId"]),
                        UniversityTypeCode = dr["UniversityTypeCode"] != DBNull.Value ? dr["UniversityTypeCode"].ToString() : "",
                        UniversityTypeName = dr["UniversityTypeName"] != DBNull.Value ? dr["UniversityTypeName"].ToString() : "",
                        IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null,
                        Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : "",
                        DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null
                    });
                }
            }

            return list;
        }

        //=========================================================
        // Get University Type By Id
        //=========================================================

        public UniversityTypeModel GetUniversityTypeById(int id)
        {
            UniversityTypeModel model = new UniversityTypeModel();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UniversityType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@UniversityTypeId", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.UniversityTypeId = Convert.ToInt32(dr["UniversityTypeId"]);
                    model.UniversityTypeCode = dr["UniversityTypeCode"] != DBNull.Value ? dr["UniversityTypeCode"].ToString() : "";
                    model.UniversityTypeName = dr["UniversityTypeName"] != DBNull.Value ? dr["UniversityTypeName"].ToString() : "";
                    model.Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : "";
                    model.DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null;
                    model.IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]);
                    model.CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null;
                }
            }

            return model;
        }

        //=========================================================
        // Insert University Type
        //=========================================================

        public bool InsertUniversityType(UniversityTypeModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UniversityType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@UniversityTypeCode", model.UniversityTypeCode);
                cmd.Parameters.AddWithValue("@UniversityTypeName", model.UniversityTypeName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //=========================================================
        // Update University Type
        //=========================================================

        public bool UpdateUniversityType(UniversityTypeModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UniversityType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@UniversityTypeId", model.UniversityTypeId);
                cmd.Parameters.AddWithValue("@UniversityTypeCode", model.UniversityTypeCode);
                cmd.Parameters.AddWithValue("@UniversityTypeName", model.UniversityTypeName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);   // ✅ yeh line missing thi — ab add ki

                con.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //=========================================================
        // Delete University Type
        //=========================================================

        public bool DeleteUniversityType(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UniversityType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@UniversityTypeId", id);

                con.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //=========================================================
        // Get Active University Types (dropdown ke liye)
        //=========================================================

        public List<UniversityTypeModel> GetActiveUniversityTypes()
        {
            List<UniversityTypeModel> list = new List<UniversityTypeModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UniversityType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new UniversityTypeModel
                    {
                        UniversityTypeId = Convert.ToInt32(dr["UniversityTypeId"]),
                        UniversityTypeCode = dr["UniversityTypeCode"].ToString(),
                        UniversityTypeName = dr["UniversityTypeName"].ToString()
                    });
                }
            }

            return list;
        }
    }
}