using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

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
                    var id = dr["UniversityTypeId"];
                    var code = dr["UniversityTypeCode"];
                    var name = dr["UniversityTypeName"];
                    var active = dr["IsActive"];
                    var created = dr["CreatedDate"];
                    var description = dr["Description"];
                    var order = dr["DisplayOrder"];

            
                        
                    list.Add(new UniversityTypeModel
                    {
                        UniversityTypeId = Convert.ToInt32(id),
                        UniversityTypeCode = code.ToString(),
                        UniversityTypeName = name.ToString(),
                        IsActive = Convert.ToBoolean(active),
                        CreatedDate = Convert.ToDateTime(created),
                        Description = description.ToString(),
                        DisplayOrder = Convert.ToInt32(order)
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
                    model.UniversityTypeCode = dr["UniversityTypeCode"].ToString();
                    model.UniversityTypeName = dr["UniversityTypeName"].ToString();
                    model.Description = dr["Description"].ToString();
                    model.DisplayOrder = Convert.ToInt32(dr["DisplayOrder"]);
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
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@DisplayOrder", model.DisplayOrder);
                
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                //cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                //object value = cmd.Parameters.GetDateParameter("@CreatedDate", DateTime.Now);

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
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@DisplayOrder", model.DisplayOrder);

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
    }
}