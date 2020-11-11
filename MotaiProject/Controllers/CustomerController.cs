using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Http.Cors;
using System.Drawing;
using System.Text;
using System.IO;

namespace MotaiProject.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult 首頁()
        {
            if(Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;   
                ViewBag.Count = count;
            }
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
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = db.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;
            }
                

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
                //npv.PromotionDescription = items.PromotionDescription;
                if (items.PromotionDescription.Length > 10)
                {
                    npv.PromotionDescription = items.PromotionDescription.Substring(0, 10) + "...";                    
                }
                else
                {
                    npv.PromotionDescription = items.PromotionDescription;
                }
                npv.pADimage = items.pADimage;
                npv.pPromotionPostDate = items.pPromotionPostDate;               
                npv.PromotionId = items.PromotionId;
                res.newPromotion = npv;
                reslsit.Add(res);
            }
            var result = reslsit.ToPagedList(cpage, pageSize);
            return View(result);
        }
        private PromotionRespoitory promotionRespotiory = new PromotionRespoitory();
        public ActionResult 消息細節(int PromotionId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;

            }
            DetailPromotionViewModel Promo = promotionRespotiory.GetPromotionById(PromotionId);
            return View(Promo);
        }

        //GET: 會員登入
        [HttpPost]
        public ActionResult 會員登入(CustomerLoginViewModel c登入資料)
        {
            string code = Request.Form["code"].ToString();
            if (code == TempData["code"].ToString())
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
            return RedirectToAction("首頁");
        }

        [HttpPost]
        public ActionResult Login()
        {
            string code = Request.Form["code"].ToString();
            if (code == TempData["code"].ToString())
            {
                ViewBag.code = code;
                ViewBag.Ans = TempData["code"];
                ViewBag.Result = "驗證正確";
                return View();
            }
            else
            {
                ViewBag.code = code;
                ViewBag.Ans = TempData["code"];
                ViewBag.Result = "驗證錯誤";
                return View();
            }
        }
        private string RandomCode(int length)
        {
            string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            return sb.ToString();
        }
        private void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }



        public ActionResult GetValidateCode()
        {
            byte[] data = null;
            string code = RandomCode(5);
            TempData["code"] = code;
            //定義一個畫板
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 40))
            {
                //畫筆,在指定畫板畫板上畫圖
                //g.Dispose();
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font("黑體", 18.0F), Brushes.Blue, new Point(10, 8));
                    //繪製干擾線(數字代表幾條)
                    PaintInterLine(g, 10, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
        }
        ////[HttpPost]
        //public JsonResult GetValidateCode()
        //{
        //    byte[] data = null;
        //    string code = RandomCode(5);
        //    TempData["code"] = code;
        //    定義一個畫板
        //    MemoryStream ms = new MemoryStream();
        //    using (Bitmap map = new Bitmap(100, 40))
        //    {
        //        畫筆,在指定畫板畫板上畫圖
        //        g.Dispose();
        //        using (Graphics g = Graphics.FromImage(map))
        //        {
        //            g.Clear(Color.White);
        //            g.DrawString(code, new Font("黑體", 18.0F), Brushes.Blue, new Point(10, 8));
        //            繪製干擾線(數字代表幾條)
        //            PaintInterLine(g, 10, map.Width, map.Height);
        //        }
        //        map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    }
        //    data = ms.GetBuffer();
        //    return Json(new { picture = File(data, "image/jpeg") });
        //}






        [HttpPost]
        public JsonResult beforeSendEmail(ForgotPasswordViewModel c電子郵件)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tCustomer d信箱確認 = dbContext.tCustomers.Where(c => c.cEmail == c電子郵件.Email).FirstOrDefault();
            if (d信箱確認 != null)
            {
                string passwordG = Guid.NewGuid().ToString();
                d信箱確認.cPassword = passwordG;
                dbContext.SaveChanges();
                return Json(new { result = true, msg = "已寄出修改密碼的信件!", url = Url.Action("首頁", "Customer"), password = passwordG, name = d信箱確認.cName,});                
            }
            return Json(new { result = false, msg = "此電子郵件尚未被註冊", url = Url.Action("會員註冊", "Customer") });
        }

        public ActionResult 會員中心()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                tCustomer custor = db.tCustomers.Find(cust.CustomerId);
                MemberViewModel member = new MemberViewModel();


                MotaiDataEntities dbContext = new MotaiDataEntities();
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;


                member.cName = custor.cName;
                member.cTelePhone = custor.cTelePhone;
                member.cCellPhone = custor.cCellPhone;
                member.cAddress = custor.cAddress;
                member.cGUI = custor.cGUI;
                member.cEmail = custor.cEmail;
                member.cAccount = custor.cAccount;
                member.cPassword = custor.cPassword;
                
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

            if (newMember.cAccount != null &&
                newMember.cPassword != null &&
                newMember.cName != null &&
                newMember.cCellPhone != null &&
                newMember.cAddress != null &&
                newMember.cEmail != null
                )
            {
                if (newMember.cPassword.Length < 6 && newMember.cAccount.Length < 6)
                { return Json(new { msg = "密碼及帳號長度需介於6~12字元" }); }

                if (newMember.cPassword != newMember.cConfirmPassword)
                {return Json(new { msg = "密碼及確認密碼必須相同" });}

                List<tCustomer> custo = dbContext.tCustomers.ToList();
                foreach (var item in custo)
                {
                    if (item.cAccount == newMember.cAccount)
                    {
                        return Json(new { msg = "帳號已被註冊" });
                    }
                    else if (item.cEmail == newMember.cEmail)
                    {
                        return Json(new { msg = "信箱已被使用" });
                    }
                }
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
            return Json(new {msg = "帳號、密碼、姓名、手機、地址及信箱必須填寫" });
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
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tCustomer custer = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
            int count = dbContext.tStatus.Where(c => c.sCustomerId == custer.CustomerId).ToList().Count;
            ViewBag.Count = count;
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
        private CommodityRespoitory commodityRespoitory = new CommodityRespoitory();
        public ActionResult 產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;
            }
               
            List<ProductViewModel> productlist = productRespotiory.GetProductAll();
            MotaiDataEntities db = new MotaiDataEntities();
            List<tProduct> CAdb = db.tProducts.ToList();
            
            List<string> L = new List<string>();
            List<string> L2 = new List<string>();
            List<string> L3 = new List<string>();
            foreach (var x in CAdb)
            {
                if (L.Find(R=>R==x.tProductCategory.Category)==null) { L.Add(x.tProductCategory.Category); };
                if (L2.Find(R => R == x.tProductMaterial.Material) == null) { L2.Add(x.tProductMaterial.Material); };
                if (L3.Find(R => R == x.tProductSize.Size) == null) { L3.Add(x.tProductSize.Size); };
            }
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

                if (items.pPrice < 5000) { items.pPriceGroup = "NTD.5,000以下"; }
                else if (items.pPrice < 50000) { items.pPriceGroup = "NTD.5,000~50,000"; }
                else if (items.pPrice < 200000) { items.pPriceGroup = "NTD.50,000~200,000"; }
                else if (items.pPrice < 1000000) { items.pPriceGroup = "NTD.200,000~1,000,000"; }
                else { items.pPriceGroup = "NTD.1,000,000以上"; }
                items.AllCategory = L;
                items.AllMaterial = L2;
                items.AllSize = L3;//只有有出現的商品
            }
            

            return View(productlist);
        }
        public ActionResult 產品細節(int ProductId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;
            }
            ProductViewModel Prod = productRespotiory.GetProductById(ProductId);
            ViewBag.Qty = Prod.pQty;
            return View(Prod);
        }

        [HttpPost]
        public JsonResult 產品細節2()
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                return Json( new {  mse = count });
            }
            return Json(new { mse = "ERROR" });
        }

        public ActionResult 購物車清單()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;
              
                List<tStatu> StateList = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList();
                List<StatusCartViewModel> cartList = new List<StatusCartViewModel>();
                StatusCartGoToPayViewModel Cart = new StatusCartGoToPayViewModel();
                foreach (var items in StateList)
                {                 
                    tProduct cartProd = dbContext.tProducts.Where(p => p.ProductId == items.sProductId).FirstOrDefault();
                    StatusCartViewModel cartC = new StatusCartViewModel();
                    cartC.StatusId = items.StatusId;
                    cartC.ProductId = items.sProductId;
                    cartC.pName = cartProd.pName;
                    cartC.pPrice = cartProd.pPrice;
                    cartC.sProductQty = items.sProductQty;
                    cartList.Add(cartC);
                }
                Cart.Carts = cartList;
                var warehouseNames = commodityRespoitory.GetWarehouseAll();
                List<SelectListItem> warehouselist = commodityRespoitory.GetSelectList(warehouseNames);
                Cart.warehouses = warehouselist;
                return View(Cart);
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
                MotaiDataEntities db = new MotaiDataEntities();
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = db.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;
        
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
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = db.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;
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

                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count;

                foreach (var items in FavorList)
                {
                    tProduct favorProd = dbContext.tProducts.Where(pp => pp.ProductId == items.fProductId).FirstOrDefault();
                    FavoriteViewModel favor = new FavoriteViewModel();
                    favor.fProductId = items.fProductId;
                    favor.fCustomerId = cust.CustomerId;
                    favor.Product = favorProd;
                    favorList.Add(favor);
                    tProduct p = new tProduct();
                    List<string> pimage = new List<string>();
                    p.ProductId = favor.fProductId;
                    pimage = productRespotiory.GetProductShowImages(p);
                    if (pimage.Count > 0)
                    {
                        favor.epsImage = Url.Content(pimage[0]);
                    }
                    else
                    {
                        favor.epsImage = "";
                    }
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
            List<tFavorite> favordb = dbContext.tFavorites.ToList();
            string func(int i){
                tProduct p = new tProduct();
                List<string> pimage = new List<string>();
                p.ProductId = i;
                pimage = productRespotiory.GetProductShowImages(p);
                if (pimage.Count > 0)
                {
                    return Url.Content(pimage[0]);
                }
                else
                {
                    return "";
                }
            }
            var favorOrderby = (from i in favordb
                                group i by  i.fProductId into j
                                select new
                                {
                                    Pid = j.Key,
                                    Pimage = func(j.Key),
                                    Pcount = j.Count(),
                             }).OrderByDescending(j => j.Pcount).Take(10).ToArray();           
                                                                 
            return Json(new { favorOrderby });

        }

        public JsonResult 購買排名()
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tOrderDetail> orderdb = dbContext.tOrderDetails.ToList();
            //這樣才能在linq內使用函式
            string func(int i)
            {
                tProduct p = new tProduct();
                List<string> pimage = new List<string>();
                p.ProductId = i;
                pimage = productRespotiory.GetProductShowImages(p);
                if (pimage.Count > 0)
                {
                    return Url.Content(pimage[0]);
                }
                else
                {
                    return "";
                }
            }
            var buyOrderby = (from i in orderdb
                              group i by i.oProductId into j
                              select new
                              {
                                  Pid = j.Key,
                                  Pimage = func(j.Key),
                                  Pcount = j.Sum(p=>p.oProductQty),
                                }).OrderByDescending(j => j.Pcount).Take(10).ToArray();

            return Json(new { buyOrderby });
        }
    }
}