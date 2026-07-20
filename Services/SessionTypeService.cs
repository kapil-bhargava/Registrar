using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    public class SessionTypeService
    {
        private readonly DBHelper db = new DBHelper();

        public List<SessionTypeModel> GetAllSessionTypes()
        {
            List<SessionTypeModel> list = new List<SessionTypeModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SessionType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new SessionTypeModel
                    {
                        SessionTypeId = Convert.ToInt32(dr["SessionTypeId"]),
                        SessionTypeCode = dr["SessionTypeCode"].ToString(),
                        SessionTypeName = dr["SessionTypeName"].ToString(),
                        Description = dr["Description"].ToString(),
                        DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null,
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                    });
                }
            }

            return list;
        }

        // Used everywhere a Session Type dropdown is needed — e.g. Academic Session's Create form
        public List<SessionTypeModel> GetActiveSessionTypes()
        {
            List<SessionTypeModel> list = new List<SessionTypeModel>();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SessionType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETACTIVE");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new SessionTypeModel
                    {
                        SessionTypeId = Convert.ToInt32(dr["SessionTypeId"]),
                        SessionTypeCode = dr["SessionTypeCode"].ToString(),
                        SessionTypeName = dr["SessionTypeName"].ToString()
                    });
                }
            }

            return list;
        }

        public SessionTypeModel GetSessionTypeById(int id)
        {
            SessionTypeModel model = new SessionTypeModel();

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SessionType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@SessionTypeId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.SessionTypeId = Convert.ToInt32(dr["SessionTypeId"]);
                    model.SessionTypeCode = dr["SessionTypeCode"].ToString();
                    model.SessionTypeName = dr["SessionTypeName"].ToString();
                    model.Description = dr["Description"].ToString();
                    model.DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : (int?)null;
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
            }

            return model;
        }

        public bool InsertSessionType(SessionTypeModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SessionType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@SessionTypeCode", model.SessionTypeCode);
                cmd.Parameters.AddWithValue("@SessionTypeName", model.SessionTypeName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateSessionType(SessionTypeModel model)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SessionType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@SessionTypeId", model.SessionTypeId);
                cmd.Parameters.AddWithValue("@SessionTypeCode", model.SessionTypeCode);
                cmd.Parameters.AddWithValue("@SessionTypeName", model.SessionTypeName);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", (object)model.DisplayOrder ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteSessionType(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SessionType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@SessionTypeId", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}