// Services/StudentLoginService.cs
using Regis.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Regis.Services
{
    public class StudentLoginService
    {
        private readonly DBHelper db = new DBHelper();

        public (bool success, int applicationId, string fullName, string studentId) ValidateLogin(string username, string passwordHash)
        {
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentLogin", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "VALIDATE");
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        return (true, Convert.ToInt32(dr["ApplicationId"]), dr["FullName"].ToString(), dr["StudentId"] as string);
                }
            }
            return (false, 0, null, null);
        }

        public bool CreateLogin(int applicationId, string username, string plainPassword)
        {
            string hash = HashPassword(plainPassword);
            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_StudentLogin", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "INSERT");
                cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", hash);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        // Registrar ne jitne students already admit kiye hain, unke liye
        // default login bana do — Username = StudentId, Password = StudentId
        public int GenerateMissingLoginsForConfirmedStudents()
        {
            int created = 0;
            var pending = new List<(int applicationId, string studentId)>();

            using (SqlConnection con = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT a.ApplicationId, a.StudentId
                  FROM Application a
                  WHERE a.StudentId IS NOT NULL
                    AND a.ApplicationId NOT IN (SELECT ApplicationId FROM StudentLogin)", con))
            {
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        pending.Add((Convert.ToInt32(dr["ApplicationId"]), dr["StudentId"].ToString()));
            }

            foreach (var p in pending)
            {
                CreateLogin(p.applicationId, p.studentId, p.studentId);
                created++;
            }
            return created;
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}