using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class CustomerMemberController : Controller
    {
        // GET: CustomerMember
        public ActionResult 已登入首頁(int CustomerId)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] == null)
                return RedirectToAction("首頁","CustomerLogin");
            return View();
        }

        public ActionResult 客戶看產品頁面()
        {
            var q = from p in (new MotaiDataEntities()).tProducts//先撈資料,產品的工廠
                    select p;
            List<CustProdViewModel> productlist = new List<CustProdViewModel>();
            foreach (tProduct item in q)
            {
                CustProdViewModel prod = new CustProdViewModel();
                prod.Product = item;
                productlist.Add(prod);
            }
            return View(productlist);
        }

    }
}