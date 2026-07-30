// Controllers/AccountController.cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Regis.Models;

namespace Regis.Controllers
{
    
    public class AccountController : Controller
    {
        private string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["erpdb"].ConnectionString;

        [HttpGet]
        public ActionResult Login()
        {
            // agar already logged in hai, seedha dashboard bhej do
            if (Session["RegistrarUser"] != null)
                return RedirectToAction("Dashboard", "Dashboard");

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                TempData["Error"] = "Fill the Username and Password.";
                return View(model);
            }

            string hashedPassword = HashPassword(model.Password);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_RegistrarLogin_Validate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", model.Username.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Session["RegistrarUser"] = reader["Username"].ToString();
                            Session["RegistrarLoginId"] = reader["LoginId"].ToString();

                            return RedirectToAction("Dashboard", "Dashboard");
                        }
                    }
                }
            }

            TempData["Error"] = "Wrong Username and Password.";
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}