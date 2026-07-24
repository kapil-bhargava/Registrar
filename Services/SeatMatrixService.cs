using Regis.Helpers;
using Regis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Regis.Services
{
    public class SeatMatrixService
    {
        private readonly DBHelper db = new DBHelper();

        public List<SeatMatrixModel> GetAllSeatMatrix()
        {
            var list = new List<SeatMatrixModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatMatrix", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new SeatMatrixModel
                        {
                            SeatMatrixId = Convert.ToInt32(dr["SeatMatrixId"]),
                            CourseId = Convert.ToInt32(dr["CourseId"]),
                            CourseName = dr["CourseName"].ToString(),
                            CourseCode = dr["CourseCode"].ToString(),
                            AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                            SessionName = dr["SessionName"].ToString(),
                            TotalSeats = Convert.ToInt32(dr["TotalSeats"]),
                            IsActive = Convert.ToBoolean(dr["IsActive"]),
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null
                        });
                    }
                }
            }
            return list;
        }

        public SeatMatrixModel GetSeatMatrixById(int id)
        {
            SeatMatrixModel model = null;
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatMatrix", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYID");
                cmd.Parameters.AddWithValue("@SeatMatrixId", id);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new SeatMatrixModel
                        {
                            SeatMatrixId = Convert.ToInt32(dr["SeatMatrixId"]),
                            CourseId = Convert.ToInt32(dr["CourseId"]),
                            AcademicSessionId = Convert.ToInt32(dr["AcademicSessionId"]),
                            TotalSeats = Convert.ToInt32(dr["TotalSeats"]),
                            IsActive = Convert.ToBoolean(dr["IsActive"])
                        };
                    }
                }
            }
            return model;
        }

        public bool InsertSeatMatrix(SeatMatrixModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatMatrix", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@AcademicSessionId", model.AcademicSessionId);
                cmd.Parameters.AddWithValue("@TotalSeats", model.TotalSeats);
                cmd.Parameters.AddWithValue("@IsActive", true);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool UpdateSeatMatrix(SeatMatrixModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatMatrix", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "UPDATE");
                cmd.Parameters.AddWithValue("@SeatMatrixId", model.SeatMatrixId);
                cmd.Parameters.AddWithValue("@CourseId", model.CourseId);
                cmd.Parameters.AddWithValue("@AcademicSessionId", model.AcademicSessionId);
                cmd.Parameters.AddWithValue("@TotalSeats", model.TotalSeats);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        public bool DeleteSeatMatrix(int id)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatMatrix", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "DELETE");
                cmd.Parameters.AddWithValue("@SeatMatrixId", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }

        //===================================================================
        //                          SeatCategoryMapping
        //===================================================================
        public List<SeatCategoryMappingModel> GetAllSeatCategoryMappings()
        {
            var dict = new Dictionary<int, SeatCategoryMappingModel>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatCategoryMapping", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETALL");
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int seatMatrixId = Convert.ToInt32(dr["SeatMatrixId"]);
                        if (!dict.ContainsKey(seatMatrixId))
                        {
                            dict[seatMatrixId] = new SeatCategoryMappingModel
                            {
                                SeatMatrixId = seatMatrixId,
                                CourseName = dr["CourseName"].ToString(),
                                CourseCode = dr["CourseCode"].ToString(),
                                SessionName = dr["SessionName"].ToString(),
                                TotalSeats = Convert.ToInt32(dr["TotalSeats"])
                            };
                        }

                        if (dr["CategoryId"] != DBNull.Value)
                        {
                            dict[seatMatrixId].CategoryAllocations.Add(new CategorySeatEntry
                            {
                                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                CategoryName = dr["CategoryName"].ToString(),
                                AllocatedSeats = Convert.ToInt32(dr["AllocatedSeats"])
                            });
                        }
                    }
                }
            }
            return new List<SeatCategoryMappingModel>(dict.Values);
        }

        public List<CategorySeatEntry> GetSeatCategoryMappingBySeatMatrix(int seatMatrixId)
        {
            var list = new List<CategorySeatEntry>();
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatCategoryMapping", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "GETBYSEATMATRIX");
                cmd.Parameters.AddWithValue("@SeatMatrixId", seatMatrixId);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new CategorySeatEntry
                        {
                            CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            AllocatedSeats = Convert.ToInt32(dr["AllocatedSeats"])
                        });
                    }
                }
            }
            return list;
        }

        public bool SaveSeatCategoryMapping(SeatCategoryMappingModel model)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_SeatCategoryMapping", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "SAVE");
                cmd.Parameters.AddWithValue("@SeatMatrixId", model.SeatMatrixId);
                cmd.Parameters.AddWithValue("@CategoryIds", model.CategoryIdsCsv);
                cmd.Parameters.AddWithValue("@Seats", model.SeatsCsv);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows != 0;
            }
        }
    }
}