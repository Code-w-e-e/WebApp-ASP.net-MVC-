using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace MyProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;
        
        public AccountController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;

            _config = config;

            
        }
        
   
        public IDbConnection Connection
        {
            get
            {
               
               return new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
                
            }
        }

        //List<User> UsersList = GetUserAll();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        private List<User> GetUserAll()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<User>("SELECT * FROM \"Users\"").ToList();

                return result;
            }
        }
        
        static List<User> UserData = new List<User>();

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var passwordHash = sha256_base64(password);
            Console.WriteLine(passwordHash);

            foreach (var user in GetUserAll())
            {
                if(user.Password == passwordHash && user.Login == login)
                {

                    using (IDbConnection db = Connection)
                    {
                        string Date = DateTime.Now.ToString();
                        string commandBD = "INSERT INTO \"History\" (fio, department, part, Date) VALUES ('" + user.FIO + "', '" + user.Department + "', '" + user.Part + "','" + Date + "');";
                        db.Query<User>(commandBD);

                        
                    }
                    UserData.Clear();
                    UserData.Add(user);
                    
                    return View("Main", UserData);
                }
            }
            ViewData["Message"] = "Неверные логин и(или) пароль";
            return View();
            

        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Login");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Back()
        {

            return View("Main", UserData);
        }

        static string sha256_base64(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            return Convert.ToBase64String(crypto);
        }

    }
}
