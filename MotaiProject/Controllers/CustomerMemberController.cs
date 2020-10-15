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

        public ActionResult 客戶看產品頁面(int CustomerId)
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

        public ActionResult 購物車清單(int CustomerId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tStatu> StateList = dbContext.tStatus.Where(c => c.sCustomerId == CustomerId).ToList();
            List<StatusCartViewModel> cartList = new List<StatusCartViewModel>();           
            foreach(var items in StateList)
            {
                tProduct cartProd = dbContext.tProducts.Where(p => p.ProductId == items.sProductId).FirstOrDefault();
                StatusCartViewModel cart = new StatusCartViewModel();
                cart.Product = cartProd;
                cartList.Add(cart);
            }            
            return View(cartList);
        }

        [HttpPost]
        public ActionResult AddToCart(int Customerid,AddToCartViewModel n購物車新增)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            var product = (new MotaiDataEntities()).tProducts.FirstOrDefault(p => p.ProductId == n購物車新增.sProductId);
            if (product == null)
            {
                return RedirectToAction("購物車清單");
            }
            tStatu cart = new tStatu();

            cart.sCustomerId = n購物車新增.sCustomerId;
            cart.sProductId = n購物車新增.sProductId;
            cart.sProductQty = n購物車新增.sProductQty;
            db.tStatus.Add(cart);
            db.SaveChanges();

            return RedirectToAction("購物車清單");


        }
    }
}