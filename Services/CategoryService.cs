using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    public class CategoryService
    {
        // Database Helper
        private readonly DBHelper db = new DBHelper();

        //=========================================================
        // Get All Categories
        //=========================================================

        public List<CategoryModel> GetAllCategories()
        {
            List<CategoryModel> list = new List<CategoryModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "GETALL");

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new CategoryModel
                    {
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryCode = dr["CategoryCode"].ToString(),
                        CategoryName = dr["CategoryName"].ToString(),
                        Description = dr["Description"].ToString(),
                        FeeConcession = dr["FeeConcession"] != DBNull.Value ? Convert.ToDecimal(dr["FeeConcession"]) : (decimal?)null,
                        DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null,
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }

            return list;
        }

        //=========================================================
        // Get Active Categories (for dropdowns everywhere)
        //=========================================================

        public List<CategoryModel> GetActiveCategories()
        {
            List<CategoryModel> list = new List<CategoryModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new CategoryModel
                    {
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        CategoryCode = dr["CategoryCode"].ToString(),
                        CategoryName = dr["CategoryName"].ToString()
                    });
                }
            }

            return list;
        }

        //=========================================================
        // Get Category By Id
        //=========================================================

        public CategoryModel GetCategoryById(int id)
        {
            CategoryModel model = new CategoryModel();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@CategoryId", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                    model.CategoryCode = dr["CategoryCode"].ToString();
                    model.CategoryName = dr["CategoryName"].ToString();
                    model.Description = dr["Description"].ToString();
                    model.FeeConcession = dr["FeeConcession"] != DBNull.Value ? Convert.ToDecimal(dr["FeeConcession"]) : (decimal?)null;
                    model.DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null;
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
            }

            return model;
        }

        //=========================================================
        // Insert Category
        //=========================================================

        public bool InsertCategory(CategoryModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@CategoryCode", model.CategoryCode);
                cmd.Parameters.AddWithValue("@CategoryName", model.CategoryName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FeeConcession", (object)model.FeeConcession ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //=========================================================
        // Update Category
        //=========================================================

        public bool UpdateCategory(CategoryModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@CategoryCode", model.CategoryCode);
                cmd.Parameters.AddWithValue("@CategoryName", model.CategoryName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FeeConcession", (object)model.FeeConcession ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //=========================================================
        // Delete Category
        //=========================================================

        public bool DeleteCategory(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@CategoryId", id);

                con.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}