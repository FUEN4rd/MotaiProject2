using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class EmployeeLoginController : Controller
    {
        // GET: EmployeeLogin               

        public ActionResult 員工登入()
        {
            return View();
        }
        [HttpPost]
        public ActionResult 員工登入(EmployeeLoginViewModel e登入資料)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tEmployee d資料確認 = dbContext.tEmployees.FirstOrDefault
                (e => e.EmployeeId == e登入資料.EmployeeId && e.ePassword.Equals(e登入資料.ePassword));
            if (d資料確認 != null)
            {
                Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = d資料確認;
                return RedirectToAction("員工首頁","EmployeeEnter"/*, new { d資料確認.EmployeeId }*/);
            }
            return View();           
        }        
    }
}