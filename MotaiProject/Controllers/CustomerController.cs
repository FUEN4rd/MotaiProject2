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
using System.Net.Mail;
using System.Net;

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
                ViewBag.Count = count + "項";
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
                ViewBag.Count = count + "項";
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
                npv.PromotionName = items.PromotionName;
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
                ViewBag.Count = count + "項";

            }
            DetailPromotionViewModel Promo = promotionRespotiory.GetPromotionById(PromotionId);
            return View(Promo);
        }

        //GET: 會員登入
        [HttpPost]
        public JsonResult 會員登入(CustomerLoginViewModel c登入資料)
        {
            if (c登入資料.cValidateCode != null)
            {
                string code = c登入資料.cValidateCode;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tCustomer d資料確認 = dbContext.tCustomers.FirstOrDefault
                    (c => c.cAccount == c登入資料.cAccount && c.cPassword.Equals(c登入資料.cPassword));
                if (d資料確認 != null)
                {
                    if (code == TempData["codecode"].ToString())
                    {
                        Session[CSession關鍵字.SK_LOGINED_CUSTOMER] = d資料確認;
                        return Json(new { result = true, msg = "登入成功",url = Url.Action("首頁", "Customer") });
                    }
                    return Json(new { result = false,msg = "驗證碼錯誤" });
                }
                return Json(new { result = false, msg = "帳號或密碼有誤" });
            }
            return Json(new { result = false, msg = "請輸入驗證碼" });
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
            TempData["codecode"] = code;
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

        //[HttpPost]
        //public JsonResult beforeSendEmail(ForgotPasswordViewModel c電子郵件)
        //{
        //    MotaiDataEntities dbContext = new MotaiDataEntities();
        //    tCustomer d信箱確認 = dbContext.tCustomers.Where(c => c.cEmail == c電子郵件.Email).FirstOrDefault();
        //    if (d信箱確認 != null)
        //    {
        //        string passwordG = Guid.NewGuid().ToString();
        //        d信箱確認.cPassword = passwordG;
        //        dbContext.SaveChanges();
        //        return Json(new { result = true, msg = "已寄出修改密碼的信件!", url = Url.Action("首頁", "Customer"), password = passwordG, name = d信箱確認.cName,});                
        //    }
        //    return Json(new { result = false, msg = "此電子郵件尚未被註冊", url = Url.Action("會員註冊", "Customer") });
        //}

        public ActionResult SendEmail(ForgotPasswordViewModel c電子郵件)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tCustomer d信箱確認 = dbContext.tCustomers.Where(c => c.cEmail == c電子郵件.Email).FirstOrDefault();
            if (d信箱確認 != null)
            {
                string passwordG = Guid.NewGuid().ToString();
                d信箱確認.cPassword = passwordG;
                dbContext.SaveChanges();

                SmtpClient Client = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = "fuen12302@gmail.com",
                        Password = "fuen12302"
                    }
                };
                MailMessage Message = new MailMessage();
                Message.To.Add(new MailAddress(c電子郵件.Email, d信箱確認.cName));
                Message.From = new MailAddress("fuen12302@gmail.com", "墨台");
                Message.Subject = "墨台 忘記密碼認證信";
                Message.IsBodyHtml = true;
                Message.Body = d信箱確認.cName + "您好非常感謝您到墨台進行選購，我們已收到您重設密碼的申請。\r\n請您回到首頁，以此密碼登入：" + d信箱確認.cPassword + "\r\n登入後，點選右上角第一個Icon進入會員中心，重新設定一組新的密碼。\r\n首頁連結： https://motai.azurewebsites.net/ ";
                Message.Priority = MailPriority.Low;
                ////MailAddress ToEmail = new MailAddress("hongyeelin5@gmail.com", "HY LIN2");
                //MailAddress FromEmail = new MailAddress("fuen12302@gmail.com", "墨台");
                //MailAddress ToEmail = new MailAddress(c電子郵件.Email, d信箱確認.cName);
                //MailMessage Message = new MailMessage()
                //{
                //    From = FromEmail,
                //    Subject = "墨台 忘記密碼認證信",
                //    Body = d信箱確認.cName + "您好非常感謝您到墨台進行選購，我們已收到您重設密碼的申請。\r\n請您回到首頁，以此密碼登入：" + d信箱確認.cPassword + "\r\n登入後，點選右上角第一個Icon進入會員中心，重新設定一組新的密碼。\r\n首頁連結： https://motai.azurewebsites.net/ "
                //    //  IsBodyHtml = true
                //};
                //Message.To.Add(ToEmail);
                Client.Send(Message);
                Client.Dispose();
                Message.Dispose();
                return Json(new { result = true, msg = "已寄出修改密碼的信件!", url = Url.Action("首頁", "Customer") });
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
                ViewBag.Count = count + "項";


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
                { return Json(new { result = false, msg = "密碼及帳號長度需介於6~12字元" }); }

                if (newMember.cPassword != newMember.cConfirmPassword)
                {return Json(new { result = false, msg = "密碼及確認密碼必須相同" });}

                List<tCustomer> custo = dbContext.tCustomers.ToList();
                foreach (var item in custo)
                {
                    if (item.cAccount == newMember.cAccount)
                    {
                        return Json(new { result = false, msg = "此帳號已被註冊" });
                    }
                    else if (item.cEmail == newMember.cEmail)
                    {
                        return Json(new { result = false, msg = "此信箱已被使用" });
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
                return Json(new { result = true, msg = "註冊成功", url = Url.Action("首頁", "Customer") });
            }     
            return Json(new { result = false, msg = "帳號、密碼、姓名、手機、地址及信箱必須填寫" });
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
        public JsonResult 修改會員資料(MemberViewModel oldMember)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer customer = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tCustomer cust = dbContext.tCustomers.Find(customer.CustomerId);
                List<tCustomer> custList = dbContext.tCustomers.ToList();
                foreach (var item in custList)
                {
                    if (item.cCellPhone == oldMember.cCellPhone)
                    {
                        if (cust.cCellPhone != oldMember.cCellPhone)
                        {
                            return Json(new { result = false, msg = "此手機號碼已被註冊" });
                        }
                    }
                    else if (item.cEmail == oldMember.cEmail)
                    {
                        if (cust.cEmail != oldMember.cEmail)
                        {
                            return Json(new { result = false, msg = "此信箱已被註冊" });
                        }
                    }
                }
                cust.cName = oldMember.cName;
                cust.cTelePhone = oldMember.cTelePhone;
                cust.cGUI = oldMember.cGUI;
                cust.cEmail = oldMember.cEmail;
                cust.cAddress = oldMember.cAddress;
                dbContext.SaveChanges();
                return Json(new { result = true, msg = "修改成功", url = Url.Action("會員中心", "Customer") });
            }
            return Json(new { result = true, msg = "請先登入", url = Url.Action("首頁", "Customer") });
        }
        //public ActionResult 修改會員資料(MemberViewModel member)
        //{
        //    if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
        //    {
        //        tCustomer customer = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
        //        MotaiDataEntities db = new MotaiDataEntities();
        //        tCustomer cust = db.tCustomers.Find(customer.CustomerId);
        //        if (cust != null)
        //        {
        //            cust.cName = member.cName;
        //            cust.cTelePhone = member.cTelePhone;
        //            cust.cGUI = member.cGUI;
        //            cust.cEmail = member.cEmail;
        //            cust.cAddress = member.cAddress;
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("會員中心");
        //    }
        //    return RedirectToAction("首頁");
        //}

        private ProductRespoitory productRespotiory = new ProductRespoitory();
        private CommodityRespoitory commodityRespoitory = new CommodityRespoitory();
        public ActionResult 產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count + "項";
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
                ViewBag.Count = count + "項";
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
                ViewBag.Count = count + "項";

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
                ViewBag.Count = count + "項";
                tStatu productStatus = db.tStatus.Where(s => s.sCustomerId == cust.CustomerId&&s.sProductId==ProductId).FirstOrDefault();
                if (productStatus != null)
                {
                    int statusQty = 0;
                    statusQty = productStatus.sProductQty;
                    var product = (new MotaiDataEntities()).tProducts.FirstOrDefault(p => p.ProductId == ProductId);
                    int productQty = productRespotiory.GetProductQtyById(ProductId);
                    if (product != null && productQty - statusQty >= buyQty)
                    {
                        productStatus.sProductQty += buyQty;
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
                    var product = (new MotaiDataEntities()).tProducts.FirstOrDefault(p => p.ProductId == ProductId);
                    int productQty = productRespotiory.GetProductQtyById(ProductId);
                    if (product != null && productQty >= buyQty)
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
                ViewBag.Count = count + "項";
            }
            return RedirectToAction("購物車清單");
        }
        //客戶看訂單
        public ActionResult 過往訂單()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                List<tOrder> orders = db.tOrders.Where(o => o.oCustomerId == cust.CustomerId).ToList();
                List<CustomerOrderViewModel> OrderList = new List<CustomerOrderViewModel>();
                foreach (var items in orders)
                {
                    CustomerOrderViewModel order = new CustomerOrderViewModel();
                    order.oDate = items.oDate;
                    order.WarehouseName = db.tWarehouseNames.Where(w => w.WarehouseNameId.Equals(items.oWarehouseName)).FirstOrDefault().WarehouseName;
                    tEmployee employee = db.tEmployees.Where(e => e.EmployeeId == items.oEmployeeId).FirstOrDefault();
                    if (employee != null)
                    {
                        order.EmployeeName = employee.eName;
                    }
                    order.cNote = items.cNote;
                    List<tOrderDetail> orderdetails = db.tOrderDetails.Where(od => od.oOrderId == items.OrderId).ToList();
                    List<CustomerOrderDetailViewModel> OrderDetailList = new List<CustomerOrderDetailViewModel>();
                    int originPrice = 0;
                    foreach (var itemDetail in orderdetails)
                    {
                        CustomerOrderDetailViewModel orderdetail = new CustomerOrderDetailViewModel();
                        tProduct product = db.tProducts.Where(p => p.ProductId == itemDetail.oProductId).FirstOrDefault();
                        orderdetail.ProductNum = product.pNumber;
                        orderdetail.ProductName = product.pName;
                        orderdetail.ProductPrice = product.pPrice;
                        orderdetail.oProductQty = itemDetail.oProductQty;
                        orderdetail.oNote = itemDetail.oNote;
                        OrderDetailList.Add(orderdetail);
                        originPrice += Convert.ToInt32(product.pPrice) * itemDetail.oProductQty;
                    }
                    if(items.oPromotionId != null)
                    {
                        tPromotion promotion = db.tPromotions.Where(p => p.PromotionId == items.oPromotionId).FirstOrDefault();
                        order.TotalAmount = originPrice - Convert.ToInt32(promotion.pDiscount);
                    }
                    else
                    {
                        order.TotalAmount = originPrice;
                    }
                    List<tOrderPay> paylists = db.tOrderPays.Where(op => op.oOrderId == items.OrderId).ToList();
                    foreach(var itemPay in paylists)
                    {
                        order.AlreadyPay += Convert.ToInt32(itemPay.oPayment);
                    }
                    order.Unpaid = order.TotalAmount - order.AlreadyPay;
                    order.CustomerOrderDetails = OrderDetailList;
                    OrderList.Add(order);
                }
                return View(OrderList);
            }
            return RedirectToAction("首頁");
        }
        //收藏
        public ActionResult 收藏清單()
        {            
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                List<tFavorite> FavorList = dbContext.tFavorites.Where(c => c.fCustomerId == cust.CustomerId).ToList();
                List<FavoriteViewModel> favorList = new List<FavoriteViewModel>();

                int count = dbContext.tStatus.Where(c => c.sCustomerId == cust.CustomerId).ToList().Count;
                ViewBag.Count = count + "項";

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