using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class CustomerLoginController : Controller
    {
        public ActionResult 首頁()
        {           
            return View();
        }
               

        //GET: 會員登入
        [HttpPost]
        public ActionResult 會員登入(CustomerLoginViewModel c登入資料)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tCustomer d資料確認 = dbContext.tCustomers.FirstOrDefault
                (c => c.cAccount == c登入資料.cAccount && c.cPassword.Equals(c登入資料.cPassword));
            if (d資料確認 != null)
            {
                Session[CSession關鍵字.SK_LOGINED_CUSTOMER] = d資料確認;
                CustomerViewModel cust = new CustomerViewModel();
                cust.Customer = d資料確認;
                return RedirectToAction("已登入首頁","CustomerMember", new {cust.CustomerId});
            }
            return RedirectToAction("首頁");
        }

        public ActionResult 會員註冊()
        {
            return View();
        }
        [HttpPost]
        public ActionResult 會員註冊(CustomerViewModel n新會員模組)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (dbContext.tCustomers.Count() == 0)
            {
                n新會員模組.CustomerId = 1;
            }

            tCustomer n新會員 = new tCustomer();
            n新會員 = n新會員模組.Customer;
            dbContext.tCustomers.Add(n新會員);
            dbContext.SaveChanges();
            return RedirectToAction("首頁");
        }

    }
}