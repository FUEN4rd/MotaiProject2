using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult 首頁()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult 登出()
        {
            Session[CSession關鍵字.SK_LOGINED_CUSTOMER] = null;
            return RedirectToAction("首頁");
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
                return RedirectToAction("首頁");
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

        public ActionResult 忘記密碼()
        {
            return View();
        }

        //public ActionResult 已登入首頁()
        //{
        //    if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] == null)
        //        return RedirectToAction("首頁");
        //    return View();
        //}

        public ActionResult 產品頁面()
        {
            var q = from p in (new MotaiDataEntities()).tProducts//先撈資料,產品的工廠
                    select p;
            List<ProductViewModel> productlist = new List<ProductViewModel>();
            foreach (tProduct item in q)
            {
                ProductViewModel prod = new ProductViewModel();
                prod.Product = item;
                productlist.Add(prod);
            }
            return View(productlist);
        }
        public ActionResult 產品細節(int fid)
        {
            tProduct product = (new MotaiDataEntities()).tProducts.FirstOrDefault(p => p.ProductId == fid);
            if (product == null)
                return RedirectToAction("List");
            return View(product);
        }
        public ActionResult 最新消息()
        {
            return View();
        }

        public ActionResult 購物車清單()
        {            
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                List<tStatu> StateList = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList();
                List<StatusCartViewModel> cartList = new List<StatusCartViewModel>();
                foreach (var items in StateList)
                {
                    tProduct cartProd = dbContext.tProducts.Where(p => p.ProductId == items.sProductId).FirstOrDefault();
                    StatusCartViewModel cart = new StatusCartViewModel();
                    cart.Product = cartProd;
                    cartList.Add(cart);
                }
                return View(cartList);
            }
            else
            {
                return RedirectToAction("首頁");
            }
        }
        //待修
        public ActionResult AddToCart(int ProductId)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                var product = (new MotaiDataEntities()).tProducts.FirstOrDefault(p => p.ProductId == ProductId);
                StatusCartViewModel cart = new StatusCartViewModel();
                cart.Product = product;
                cart.sProductQty = 1;
                return View(cart);
            }
            return RedirectToAction("首頁");
        }

        [HttpPost]
        public ActionResult AddToCart(StatusCartViewModel n購物車新增)
        {
            tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
            MotaiDataEntities db = new MotaiDataEntities();
            var product = (new MotaiDataEntities()).tProducts.FirstOrDefault(p => p.ProductId == n購物車新增.Product.ProductId);
            if (product == null)
            {
                return RedirectToAction("購物車清單");
            }
            tStatu cart = new tStatu();

            cart.sCustomerId = cust.CustomerId;
            cart.sProductId = n購物車新增.Product.ProductId;
            cart.sProductQty = n購物車新增.sProductQty;
            db.tStatus.Add(cart);
            db.SaveChanges();
            return RedirectToAction("購物車清單");
        }

        public ActionResult 收藏清單()
        {            
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                List<tFavorite> FavorList = dbContext.tFavorites.Where(c => c.fCustomerId == cust.CustomerId).ToList();
                List<FavoriteViewModel> favorList = new List<FavoriteViewModel>();
                foreach (var items in FavorList)
                {
                    tProduct favorProd = dbContext.tProducts.Where(p => p.ProductId == items.fProductId).FirstOrDefault();
                    FavoriteViewModel favor = new FavoriteViewModel();
                    favor.Product = favorProd;
                    favorList.Add(favor);
                }
                return View(favorList);
            }
            else
            {
                return RedirectToAction("首頁");
            }
        }

        public ActionResult 新增收藏(int ProductId)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                tFavorite x = db.tFavorites.Where(c => c.fCustomerId.Equals(cust.CustomerId)
                && c.fProductId == ProductId).FirstOrDefault();
                if (x != null)
                {
                    Response.Write("這筆紀錄已新增過");
                }
                else
                {
                    x.fCustomerId = cust.CustomerId;
                    x.fProductId = ProductId;
                    db.tFavorites.Add(x);
                    db.SaveChanges();
                }
            }
            return View();
        }

        public ActionResult 刪除收藏(int ProductId)
        {
            tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
            MotaiDataEntities db = new MotaiDataEntities();
            tFavorite x = db.tFavorites.Where(c => c.fCustomerId.Equals(cust.CustomerId)
            && c.fProductId == ProductId).FirstOrDefault();
            if (x != null)
            {
                db.tFavorites.Remove(x);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}