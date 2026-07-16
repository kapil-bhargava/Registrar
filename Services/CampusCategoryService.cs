using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    public class CampusCategoryService
    {
        private readonly DBHelper db = new DBHelper();

        public List<CampusCategoryModel> GetAllCampusCategories()
        {
            List<CampusCategoryModel> list = new List<CampusCategoryModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CampusCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new CampusCategoryModel
                    {
                        CampusCategoryId = Convert.ToInt32(dr["CampusCategoryId"]),
                        CampusCategoryCode = dr["CampusCategoryCode"].ToString(),
                        CampusCategoryName = dr["CampusCategoryName"].ToString(),
                        Description = dr["Description"].ToString(),
                        DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null,
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }

            return list;
        }

        // Used everywhere a Campus Type dropdown is needed — any controller can call this
        public List<CampusCategoryModel> GetActiveCampusCategories()
        {
            List<CampusCategoryModel> list = new List<CampusCategoryModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CampusCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new CampusCategoryModel
                    {
                        CampusCategoryId = Convert.ToInt32(dr["CampusCategoryId"]),
                        CampusCategoryCode = dr["CampusCategoryCode"].ToString(),
                        CampusCategoryName = dr["CampusCategoryName"].ToString()
                    });
                }
            }

            return list;
        }

        public CampusCategoryModel GetCampusCategoryById(int id)
        {
            CampusCategoryModel model = new CampusCategoryModel();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CampusCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@CampusCategoryId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.CampusCategoryId = Convert.ToInt32(dr["CampusCategoryId"]);
                    model.CampusCategoryCode = dr["CampusCategoryCode"].ToString();
                    model.CampusCategoryName = dr["CampusCategoryName"].ToString();
                    model.Description = dr["Description"].ToString();
                    model.DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null;
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
            }

            return model;
        }

        public bool InsertCampusCategory(CampusCategoryModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CampusCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@CampusCategoryCode", model.CampusCategoryCode);
                cmd.Parameters.AddWithValue("@CampusCategoryName", model.CampusCategoryName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateCampusCategory(CampusCategoryModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CampusCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@CampusCategoryId", model.CampusCategoryId);
                cmd.Parameters.AddWithValue("@CampusCategoryCode", model.CampusCategoryCode);
                cmd.Parameters.AddWithValue("@CampusCategoryName", model.CampusCategoryName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteCampusCategory(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CampusCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@CampusCategoryId", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        //=================================================
        //campus management
        //=================================================

        public List<CampusModel> GetAllCampuses()
        {
            List<CampusModel> list = new List<CampusModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Campus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new CampusModel
                    {
                        CampusId = Convert.ToInt32(dr["CampusId"]),
                        CampusName = dr["CampusName"].ToString(),
                        CampusCode = dr["CampusCode"].ToString(),
                        CampusTypeId = dr["CampusTypeId"] != DBNull.Value ? Convert.ToInt32(dr["CampusTypeId"]) : (int?)null,
                        CampusTypeName = dr["CampusTypeName"] != DBNull.Value ? dr["CampusTypeName"].ToString() : "",
                        Capacity = dr["Capacity"] != DBNull.Value ? Convert.ToInt32(dr["Capacity"]) : (int?)null,
                        Address = dr["Address"].ToString(),
                        ContactNumber = dr["ContactNumber"].ToString(),
                        Email = dr["Email"].ToString(),
                        Dean = dr["Dean"].ToString(),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }

            return list;
        }

        public CampusModel GetCampusById(int id)
        {
            CampusModel model = new CampusModel();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Campus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@CampusId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.CampusId = Convert.ToInt32(dr["CampusId"]);
                    model.CampusName = dr["CampusName"].ToString();
                    model.CampusCode = dr["CampusCode"].ToString();
                    model.CampusTypeId = dr["CampusTypeId"] != DBNull.Value ? Convert.ToInt32(dr["CampusTypeId"]) : (int?)null;
                    model.Capacity = dr["Capacity"] != DBNull.Value ? Convert.ToInt32(dr["Capacity"]) : (int?)null;
                    model.Address = dr["Address"].ToString();
                    model.ContactNumber = dr["ContactNumber"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.Dean = dr["Dean"].ToString();
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
            }

            return model;
        }

        public bool InsertCampus(CampusModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Campus", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@CampusName", model.CampusName);
                cmd.Parameters.AddWithValue("@CampusCode", model.CampusCode);
                cmd.Parameters.AddWithValue("@CampusTypeId", (object)model.CampusTypeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Capacity", (object)model.Capacity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)model.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactNumber", (object)model.ContactNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dean", (object)model.Dean ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateCampus(CampusModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Campus", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@CampusId", model.CampusId);
                cmd.Parameters.AddWithValue("@CampusName", model.CampusName);
                cmd.Parameters.AddWithValue("@CampusCode", model.CampusCode);
                cmd.Parameters.AddWithValue("@CampusTypeId", (object)model.CampusTypeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Capacity", (object)model.Capacity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)model.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactNumber", (object)model.ContactNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dean", (object)model.Dean ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteCampus(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Campus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@CampusId", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}