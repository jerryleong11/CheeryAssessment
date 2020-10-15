using CheeryAssessment.Attributes;
using CheeryAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheeryAssessment.Controllers
{
    [SessionAuthorize]
    public class UserController : Controller
    {
        private DB_Entities _db = new DB_Entities();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewProfile()
        {
            long userid = long.Parse(Session["userid"].ToString());
            var data = _db.userModels.Where(u => u.id == userid).ToList().FirstOrDefault();

            return View(data);
        }
    }
}