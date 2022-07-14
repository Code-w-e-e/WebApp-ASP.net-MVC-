using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using System.Diagnostics;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Npgsql;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
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

        
        public IActionResult UsersList()
        {
            var model = GetUserAll();


            return View(model);
        }

        public IActionResult AbonentList()
        {
            var model = GetAbonentAll();

            return View(model);
        }

        public IActionResult CertificateList()
        {
            var model = GetCertificateLate();

            return View(model);
        }
        public IActionResult History()
        {
            var model = GetHistory();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddUser(string FIO, string Role, string LogIn, string Password, string Department, string Part)
        {
            var passwordHash = sha256_base64(Password);
            using (IDbConnection db = Connection)
             {
                 
                   string commandBD = "INSERT INTO \"Users\" (fio, login, password, role, department, part) VALUES" +
                    " ('" + FIO + "', '" + LogIn + "', '" + passwordHash + "','" + Role + "', " +
                    "'" + Department + "','" + Part + "');";
                   db.Query<User>(commandBD);
                   
             }
            var model = GetUserAll();

            return View("UsersList", model);
              
        }

        
        public IActionResult DeleteUser(int Id)
        {
            using (IDbConnection db = Connection)
            {
                //User user = db.QueryFirstOrDefault(p => p.ID == id);

               string commandBD = "DELETE FROM \"Users\" WHERE id='"+Id+"'";
               db.Query<User>(commandBD);

            }
            var model = GetUserAll();

            return View("UsersList", model);

        }

        public IActionResult UpdateUser(int Id)
        {
            using (IDbConnection db = Connection)
            {           
                string commandBD = "SELECT * FROM \"Users\" WHERE id='" + Id + "'";
                var user = db.Query<User>(commandBD).ToList();
                return View("Edit", user);
            }

        }

        [HttpPost]
        public IActionResult SaveChangeUser(int Id, string FIO, string Role, string LogIn, string Password, string Department, string Part)
        {
            var passwordHash = sha256_base64(Password);
            using (IDbConnection db = Connection)
            {
                string commandBD = "UPDATE \"Users\" " + "SET fio='"+FIO+ "', login='" + LogIn + "', " +
                    "password='" + passwordHash + "', role='" + Role + "', department='" + Department + "', part='" + Part + "' " + 
                    "WHERE id='" + Id + "'";
                var user = db.Query<User>(commandBD).ToList();
                
                var model = GetUserAll();

                return View("UsersList", model);
            }

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<User> GetUserAll()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<User>("SELECT * FROM \"Users\" ORDER BY id").ToList();

                return result;
            }
        }

        private List<Abonent> GetAbonentAll()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Abonent>("SELECT * FROM \"Abonents\" ORDER BY id").ToList();

                return result;
            }
        }

        private List<Certificate> GetCertificateLate()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Certificate>("SELECT * FROM \"Certificates\" WHERE dateend IS NOT NULL").ToList();

                return result;
            }
        }
        private List<History> GetHistory()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<History>("SELECT * FROM \"History\"").ToList();
                result.Reverse();
                return result;
            }
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