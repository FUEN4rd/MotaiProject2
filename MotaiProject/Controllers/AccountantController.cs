using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class AccountantController : Controller
    {
        // GET: Accountant
        public ActionResult Accountant首頁()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            else
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                EmployeeViewModels employee = new EmployeeViewModels();
                employee.EmployeeId = emp.EmployeeId;
                employee.eName = emp.eName;
                employee.eAccount = emp.eAccount;
                return View(employee);
            }
        }
    }
}