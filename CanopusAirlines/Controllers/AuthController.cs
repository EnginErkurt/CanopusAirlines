using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CanopusAirlines.Controllers
{
    public class AuthController : Controller
    {
        // BAĞLANTI ADRESİ (Web.config'deki isme göre burayı değiştirebilirsin ama bu da çalışır)
        // Eğer çalışmazsa "Server=(localdb)\\MSSQLLocalDB" kısmını kontrol et.
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=CanopusAirports;Trusted_Connection=True;";

        // --- GİRİŞ YAP (LOGIN) ---
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read()) // Kullanıcı bulundu!
                    {
                        // ASP.NET SESSION (Oturum) Özelliği
                        // Kullanıcı bilgilerini sunucu hafızasına atıyoruz.
                        Session["UserID"] = dr["UserID"].ToString();
                        Session["UserEmail"] = dr["Email"].ToString();
                        Session["UserName"] = dr["FirstName"].ToString();

                        // Ana Sayfaya gönder
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Email veya şifre hatalı!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Bağlantı hatası: " + ex.Message;
                    return View();
                }
            }
        }

        // --- KAYIT OL (REGISTER) ---
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string email, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_RegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    con.Open();
                    // ExecuteScalar tek bir değer döndürür (Bizim prosedürde 1 veya 0)
                    int result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result == 1)
                    {
                        // Başarılıysa Giriş sayfasına at
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.Error = "Bu email adresi zaten kullanılıyor!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Hata: " + ex.Message;
                    return View();
                }
            }
        }

        // --- ÇIKIŞ YAP (LOGOUT) ---
        public ActionResult Logout()
        {
            Session.Clear(); // Tüm hafızayı temizle
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}