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
        //Logout
        [HttpPost]
        public ActionResult 登出()
        {
            Session[CSession關鍵字.SK_LOGINED_CUSTOMER] = null;
            return RedirectToAction("首頁");
        }
        //Promotion
        int pageSize = 10;
        public ActionResult 消息(int page = 1)
        {
            MotaiDataEntities db = new MotaiDataEntities();

            int cpage = page < 1 ? 1 : page;
            //資料庫讀取 tPromotions 為資料庫名稱
            var promotion = db.tPromotions.OrderByDescending(c => c.PromotionId).ToList();
            //開新List 取值
            List<news> reslsit = new List<news>();
            foreach (var items in promotion)
            {
                //實體化 class
                news res = new news();
                NewPromotionViewModel npv = new NewPromotionViewModel();
                //Prom 讀取入get set
                npv.sPromotinoCategory = items.tPromotionCategory.PromtionCategory;
                npv.PromotionDescription = items.PromotionDescription;
                npv.pADimage = items.pADimage;
                npv.pPromotionWeb = items.pPromotionWeb;
                npv.pPromotionPostDate = items.pPromotionPostDate;
                res.newPrmotion = npv;
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
                return RedirectToAction("首頁");
            }
            return RedirectToAction("首頁");
        }

        [HttpPost]
        public JsonResult beforeSendEmail(ForgotPasswordViewModel c電子郵件)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tCustomer d信箱確認 = dbContext.tCustomers.Where(c => c.cEmail == c電子郵件.Email).FirstOrDefault();
            if (d信箱確認 != null)
            {
                return Json(new { result = true, msg = "已寄出修改密碼的信件!", url = Url.Action("首頁", "Customer") });
            }
            return Json(new { result = false, msg = "此電子郵件尚未被註冊", url = Url.Action("會員註冊", "Customer") });
        }
        [HttpPost]
        public JsonResult afterSendEmail(int CustomerId, string cPassword)
        {
            tCustomer customer = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
            MotaiDataEntities dbContext = new MotaiDataEntities();

            //if (a)
            //{
            //    customer.cPassword = cPassword;
            //    Session[CSession關鍵字.SK_LOGINED_CUSTOMER] = customer;
            //    tCustomer changePwd = dbContext.tCustomers.Where(c => c.CustomerId.Equals(customer.CustomerId)).FirstOrDefault();
            //    changePwd.cPassword = cPassword;
            //    dbContext.SaveChanges();
            //    return Json(new { result = true, msg = "更新成功" });
            //}
            //else
            //    {
            //    return Json(new { result = false, msg = "舊密碼錯誤" });
            //}
            return Json(new { result = false, msg = "舊密碼錯誤" });
        }
        public ActionResult 會員中心()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MemberViewModel member = new MemberViewModel();
                member.cName = cust.cName;
                member.cTelePhone = cust.cTelePhone;
                member.cCellPhone = cust.cCellPhone;
                member.cAddress = cust.cAddress;
                member.cGUI = cust.cGUI;
                member.cEmail = cust.cEmail;
                member.cAccount = cust.cAccount;
                member.cPassword = cust.cPassword;
                return View(member);
            }
            return RedirectToAction("首頁");
        }
        public JsonResult CustomerChangePassword(int CustomerId, string cPassword, string oldpass)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer customer = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                
                if (customer.cPassword == oldpass)
                {
                    customer.cPassword = cPassword;
                    Session[CSession關鍵字.SK_LOGINED_CUSTOMER] = customer;
                    tCustomer changePwd = dbContext.tCustomers.Where(c => c.CustomerId.Equals(customer.CustomerId)).FirstOrDefault();
                    changePwd.cPassword = cPassword;
                    dbContext.SaveChanges();
                    return Json(new { result = true, msg = "更新成功" });
                }
                else
                {
                    return Json(new { result = false, msg = "舊密碼錯誤" });
                }

            }
            else
            {
                return Json(new { result = false, msg = "請先登入" });
            }
        }

        public ActionResult 會員註冊()
        {
            return View();
        }        
        [HttpPost]
        public JsonResult 會員註冊(RegisterViewModel newMember)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tCustomer> custo = dbContext.tCustomers.ToList();
            foreach (var item in custo)
            {
                if (item.cAccount == newMember.cAccount)
                {
                    return Json(new {msg = "帳號已被註冊" });
                }else if(item.cEmail == newMember.cEmail)
                {
                    return Json(new { msg = "信箱已被使用" });
                }
            }
            if (newMember.cPassword == newMember.cConfirmPassword)
            {
                tCustomer newmember = new tCustomer();
                newmember.cAccount = newMember.cAccount;
                newmember.cPassword = newMember.cPassword;
                newmember.cName = newMember.cName;
                newmember.cTelePhone = newMember.cTelePhone;
                newmember.cCellPhone = newMember.cCellPhone;
                newmember.cAddress = newMember.cAddress;
                newmember.cGUI = newMember.cGUI;
                newmember.cEmail = newMember.cEmail;
                dbContext.tCustomers.Add(newmember);
                dbContext.SaveChanges();
                return Json(new { msg = "註冊成功" });
            }
            return Json(new {msg = "註冊失敗" });
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
            MemberViewModel customer = new MemberViewModel();
            customer.cAccount = cust.cAccount;
            customer.cPassword = cust.cPassword;
            customer.cName = cust.cName;
            customer.cTelePhone = cust.cTelePhone;
            customer.cCellPhone = cust.cCellPhone;
            customer.cAddress = cust.cAddress;
            customer.cGUI = cust.cGUI;
            customer.cEmail = cust.cEmail;
            return View(customer);
        }
        [HttpPost]
        public ActionResult 修改會員資料(MemberViewModel member)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer customer = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                tCustomer cust = db.tCustomers.Find(customer.CustomerId);
                if (cust != null)
                {
                    cust.cName = member.cName;
                    cust.cTelePhone = member.cTelePhone;
                    cust.cGUI = member.cGUI;
                    cust.cEmail = member.cEmail;
                    cust.cAddress = member.cAddress;
                    db.SaveChanges();
                }
                return RedirectToAction("會員中心");
            }
            return RedirectToAction("首頁");
        }

        //Product
        private ProductRespoitory productRespotiory = new ProductRespoitory();

        public ActionResult 產品頁面()
        {            
            List<ProductViewModel> productlist = productRespotiory.GetProductAll();
            return View(productlist);
        }
        public ActionResult 產品細節(int ProductId)
        {
            ProductViewModel Prod = productRespotiory.GetProductById(ProductId);
            ViewBag.Qty = Prod.pQty;
            return View(Prod);
        }

        public ActionResult 購物車清單()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {

                MotaiDataEntities dbContext = new MotaiDataEntities();
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;                
                List<tStatu> StateList = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList();
                List<StatusCartViewModel> cartList = new List<StatusCartViewModel>();
                
                foreach (var items in StateList)
                {                 
                    tProduct cartProd = dbContext.tProducts.Where(p => p.ProductId == items.sProductId).FirstOrDefault();
                    StatusCartViewModel cartC = new StatusCartViewModel();
                    cartC.StatusId = items.StatusId;
                    cartC.pName = cartProd.pName;
                    cartC.pPrice = cartProd.pPrice;
                    cartC.sProductQty = items.sProductQty;
                    cartList.Add(cartC);
                }                
                return View(cartList);
            }
            else
            {
                return RedirectToAction("首頁");
            }
        }        
        //加入購物車
        public JsonResult AddToCart(int ProductId, int buyQty)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                var product = (new MotaiDataEntities()).tProducts.FirstOrDefault(p => p.ProductId == ProductId);
                if (product != null && product.pQty > buyQty)
                {
                    tStatu cart = new tStatu();
                    cart.sCustomerId = cust.CustomerId;
                    cart.sProductId = ProductId;
                    cart.sProductQty = buyQty;
                    db.tStatus.Add(cart);
                    db.SaveChanges();
                    return Json(new { result = true, msg = "加入成功" });
                }
                else
                {
                    return Json(new { result = false, msg = "庫存不足" });
                }
            }
            else
            {
                return Json(new { result = false, msg = "請先登入" });
            }
        }

        public ActionResult 購物車內刪除(int fid)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tStatu statu = db.tStatus.Where(s => s.StatusId.Equals(fid)).FirstOrDefault();
            if ( statu != null)
            {
                db.tStatus.Remove(statu);
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
                    favor.fProductId = items.fProductId;
                    favor.fCustomerId = cust.CustomerId;
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
        //加入收藏
        public JsonResult AddFavorite(int ProductId)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities dbContext = new MotaiDataEntities();                
                tFavorite favor = new tFavorite();
                //if (dbContext.tFavorites.Count().Equals(0))
                //{
                //    favor.FavoriteId = 1;
                //}
                favor.fCustomerId = cust.CustomerId;
                favor.fProductId = ProductId;
                dbContext.tFavorites.Add(favor);
                dbContext.SaveChanges();
                return Json(new { result = true, msg = "加入成功" });
            }
            else
            {
                return Json(new { result = false, msg = "請先登入" });
            }
        }

        public JsonResult CancelFavorite(int ProductId)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null) {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                tFavorite favor = db.tFavorites.Where(f => f.fProductId.Equals(ProductId)&&f.fCustomerId.Equals(cust.CustomerId)).FirstOrDefault();
                db.tFavorites.Remove(favor);
                db.SaveChanges();
                return Json(new { result = true, msg = "刪除成功" });
            }
            else
            {
                return Json(new { result = false, msg = "請先登入" });
            }
        }

        public JsonResult CheckFavor(int ProductId)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                if(db.tFavorites.Where(f => f.fProductId.Equals(ProductId) && f.fCustomerId.Equals(cust.CustomerId)).FirstOrDefault() == null)
                {
                    return Json(new { check = false });
                }
                else
                {
                    return Json(new { check = true });
                }
            }
            else
            {
                return Json(new { check = false });
            }            
        }        

        public JsonResult 收藏排名()
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            var favorOrderby = (from i in dbContext.tFavorites
                             group i by i.fProductId into j
                             select new
                             {
                                 Pid = j.Key,
                                 Pcount = j.Count(),
                             }).OrderByDescending(j => j.Pcount).Take(10).ToArray();           
                                                                 
            return Json(new { favorOrderby });

        }

        public JsonResult 購買排名()
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            var buyOrderby = (from i in dbContext.tOrderDetails
                                group i by i.oProductId into j
                                select new
                                {
                                    Pid = j.Key,
                                    Pcount = j.Sum(p=>p.oProductQty),
                                }).OrderByDescending(j => j.Pcount).Take(10).ToArray();

            return Json(new { buyOrderby });
        }
    }
}