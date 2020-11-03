using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace MotaiProject.Controllers
{
    public class PeopleController : Controller
    {
        // GET: People
        public ActionResult People首頁()
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
        public ActionResult 新增員工()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            return View();
        }
        [HttpPost]
        public ActionResult 新增員工(EmployeeViewModels create員工)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (dbContext.tEmployees.Count().Equals(0))
            {
                create員工.EmployeeId = 1;
            }
            tEmployee n新員工 = new tEmployee();
            n新員工.eAccount = create員工.eAccount;
            n新員工.EmployeeId = create員工.EmployeeId;
            n新員工.ePassword = create員工.ePassword;
            n新員工.eName = create員工.eName;
            n新員工.ePosition = create員工.ePosition;
            n新員工.eBranch = create員工.eBranch;
            dbContext.tEmployees.Add(n新員工);
            dbContext.SaveChanges();
            return RedirectToAction("員工首頁");
        }

        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult 人事看產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            List<ProductViewModel> productlist = new List<ProductViewModel>();
            productlist = productRespotiory.GetProductAll();
            foreach (var items in productlist)
            {
                if (items.psImage.Count > 0)
                {
                    items.epsImage = Url.Content(items.psImage[0]);
                }
                else
                {
                    items.epsImage = "";
                }
            }
            return View(productlist);
        }
    }
}