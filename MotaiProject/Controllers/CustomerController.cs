using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


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
        int pageSize = 10;
        public ActionResult 消息(int page = 1)
        {
            MotaiDataEntities db = new MotaiDataEntities();

            int cpage = page < 1 ? 1 : page;
            //資料庫讀取 tPromotions 為資料庫名稱
            var promotion = db.tPromotions.OrderBy(c => c.PromotionId).ToList();
            //開新List 取值
            List<PromotionViewModel> reslsit = new List<PromotionViewModel>();
            foreach(var items in promotion)
            {
                //實體化 class
                PromotionViewModel res = new PromotionViewModel();
                //Prom 讀取入get set
                res.Prom = items;
                //
                reslsit.Add(res);
            }
            var result = reslsit.ToPagedList(cpage, pageSize);


            return View(result);
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
        public ActionResult 會員中心()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                CustomerViewModel customer = new CustomerViewModel();
                customer.Customer = cust;
                return View(customer);
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
        public ActionResult 修改會員資料(int CustomerId)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tCustomer cust = db.tCustomers.FirstOrDefault(c => c.CustomerId == CustomerId);
            if (cust == null)
            {
                return RedirectToAction("會員中心");
            }
            CustomerViewModel customer = new CustomerViewModel();
            customer.Customer = cust;
            return View(customer);
        }
        [HttpPost]
        public ActionResult 修改會員資料(CustomerViewModel c)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tCustomer cust = db.tCustomers.Find(c.CustomerId);
            if (cust != null)
            {
                cust.cName = c.cName;
                cust.cPassword = c.cPassword;
                cust.cTelePhone = c.cTelePhone;
                cust.cGUI = c.cGUI;
                cust.cEmail = c.cEmail;
                cust.cAddress = c.cAddress;
                cust.cAccount = c.cAccount;
                db.SaveChanges();
            }
            return RedirectToAction("會員中心");
        }
        //public ActionResult 已登入首頁()
        //{
        //    if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] == null)
        //        return RedirectToAction("首頁");
        //    return View();
        //}

        public ActionResult 產品頁面()
        {
            MotaiDataEntities db = new MotaiDataEntities();
            List<tProduct> prod = db.tProducts.ToList();
            List<ProductViewModel> productlist = new List<ProductViewModel>();            
            foreach (tProduct item in prod)
            {
                List<tProductImage> images = db.tProductImages.Where(i => i.ProductId.Equals(item.ProductId)).ToList();
                ProductViewModel Prod = new ProductViewModel();
                Prod.Product = item;
                foreach(var imageitem in images)
                {
                    Prod.psImage.Add(imageitem.pImage);
                }
                productlist.Add(Prod);
            }
            return View(productlist);
        }
        public ActionResult 產品細節(int id)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct product = db.tProducts.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return RedirectToAction("List");
            }
            ViewBag.Qty = product.pQty;
            ProductViewModel prod = new ProductViewModel();
            prod.Product = product;
            prod.pQty = product.pQty;        
            return View(prod);
        }

        public ActionResult 購物車清單()
        {            
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                List<tStatu> StateList = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList();
                List<StatusCustomerViewModel> cartList = new List<StatusCustomerViewModel>();
                foreach (var items in StateList)
                {
                    tProduct cartProd = dbContext.tProducts.Where(p => p.ProductId == items.sProductId).FirstOrDefault();
                    StatusCustomerViewModel cartC = new StatusCustomerViewModel();                    
                    cartC.Status.Product = cartProd;
                    cartList.Add(cartC);
                }
                return View(cartList);
            }
            else
            {
                return RedirectToAction("首頁");
            }
        }

       




        //待修
        public ActionResult 購物車新增(int ProductId)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                var product = db.tProducts.FirstOrDefault(p => p.ProductId == ProductId);
                StatusCartViewModel cart = new StatusCartViewModel();
                cart.Product = product;                
                return Json(cart);
            }
            return RedirectToAction("首頁");
        }

        [HttpPost]
        public ActionResult 購物車新增(StatusCartViewModel n購物車新增)
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
        public ActionResult 購物車內刪除(int fid)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct product = db.tProducts.FirstOrDefault(p => p.ProductId == fid);
            if (product != null)
            {
                db.tProducts.Remove(product);
                db.SaveChanges();
            }
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
            //return View("收藏清單");
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