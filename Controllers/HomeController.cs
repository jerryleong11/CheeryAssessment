using CheeryAssessment.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace CheeryAssessment.Controllers
{
    public class HomeController : Controller
    {
        private DB_Entities _db = new DB_Entities();

        public ActionResult Index()
        {
            if (Session["userid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = _db.userModels.Where(s => s.email.Equals(email) && s.password.Equals(f_password)).ToList();

                if (data.Count > 0)
                {
                    Session["userid"] = data.FirstOrDefault().id;
                    Session["email"] = data.FirstOrDefault().email;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    //return RedirectToAction("Login");
                    return View();
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserModel _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.userModels.FirstOrDefault(s => s.email == _user.email);
                if (check == null)
                {
                    DateTime dtNow = DateTime.Now;

                    _user.password = GetMD5(_user.password);
                    _user.creation_date = dtNow;
                    _user.last_login_date = dtNow;

                    _user.UserBalanceModel = new UserBalanceModel();
                    _user.UserBalanceModel.bal_usd = "100.00";
                    _user.UserBalanceModel.bal_myr = "0.00";
                    _user.UserBalanceModel.bal_sgd = "0.00";
                    _user.UserBalanceModel.bal_aud = "0.00";
                    _user.UserBalanceModel.last_update_date = dtNow;
                    _user.UserBalanceModel.creation_date = dtNow;

                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.userModels.Add(_user);
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i=0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String;
        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}