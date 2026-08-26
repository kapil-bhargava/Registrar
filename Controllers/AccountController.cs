// Controllers/AccountController.cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Regis.Filters;
using Regis.Models;
using Regis.Services;

namespace Regis.Controllers
{
    [IgnoreAuth]   // 👈 poore controller pe — login/logout pe global auth filter nahi chalega
    public class AccountController : Controller
    {
        private string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["erpdb"].ConnectionString;
        private readonly StudentLoginService studentLoginService = new StudentLoginService();

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["RegistrarUser"] != null)
                return RedirectToAction("Dashboard", "Dashboard");

            if (Session["StudentUser"] != null)
                return RedirectToAction("StudentDashboard", "StudentLogin");

            // TODO: Teacher module banne ke baad yahan uska check bhi aayega

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the highlighted fields.";
                return View(model);
            }

            string username = model.Username?.Trim();
            string password = model.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Fill the Username and Password.";
                return View(model);
            }

            // Brute-force throttle (session-based)
            int attempts = Session["LoginAttempts"] != null ? Convert.ToInt32(Session["LoginAttempts"]) : 0;
            if (attempts >= 5)
            {
                TempData["Error"] = "Too many failed attempts. Please try again later.";
                return View(model);
            }

            string hashedPassword = HashPassword(password);

            // -------- 1) Registrar table me try karo --------
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarLogin_Validate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string registrarName = reader["Username"].ToString();
                        string registrarLoginId = reader["LoginId"].ToString();

                        Session.Clear();   // 👈 purana koi bhi session (StudentUser waghera) hata do
                        Session["RegistrarUser"] = registrarName;
                        Session["RegistrarLoginId"] = registrarLoginId;
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
            }

            // -------- 2) Student table me try karo --------
            var studentResult = studentLoginService.ValidateLogin(username, hashedPassword);
            if (studentResult.success)
            {
                Session.Clear();   // 👈 purana koi bhi session (RegistrarUser waghera) hata do
                Session["StudentUser"] = studentResult.fullName;
                Session["StudentApplicationId"] = studentResult.applicationId;
                Session["StudentIdNo"] = studentResult.studentId;
                return RedirectToAction("StudentDashboard", "StudentLogin");
            }

            // -------- 3) Teacher table (jab module banega, yahan block aayega) --------
            // var teacherResult = teacherLoginService.ValidateLogin(username, hashedPassword);
            // if (teacherResult.success) { ... return RedirectToAction("Dashboard", "Teacher"); }

            // -------- Kahin match nahi hua --------
            Session["LoginAttempts"] = attempts + 1;
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