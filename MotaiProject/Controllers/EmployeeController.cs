﻿using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult 員工登入()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult 員工登入(EmployeeLoginViewModel e登入資料)
        //{
        //    MotaiDataEntities dbContext = new MotaiDataEntities();
        //    tEmployee d資料確認 = dbContext.tEmployees.Where(e => e.eAccount == e登入資料.eAccount && e.ePassword.Equals(e登入資料.ePassword)).FirstOrDefault();
        //    if (d資料確認 != null)
        //    {
        //        Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = d資料確認;

        //        switch (d資料確認.eBranch)
        //        {
        //            case 1:
        //                return RedirectToAction("Boss首頁", "Boss");
        //            case 2:
        //                return RedirectToAction("Business首頁", "Business");
        //            case 3:
        //                return RedirectToAction("Accountant首頁", "Accountant");
        //            case 4:
        //                return RedirectToAction("People首頁", "People");
        //            case 5:
        //                return RedirectToAction("Commodity首頁", "Commodity");
        //            default:
        //                return RedirectToAction("員工首頁", "Employee");
        //        }
        //    }
        //    else
        //    {
        //        Response.Write("帳號密碼錯誤!");
        //        return View();
        //    }
        //}

        [HttpPost]
        public JsonResult 員工登入(EmployeeLoginViewModel e登入資料)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tEmployee d資料確認 = dbContext.tEmployees.Where(e => e.eAccount == e登入資料.eAccount && e.ePassword.Equals(e登入資料.ePassword)).FirstOrDefault();
            if (d資料確認 != null)
            {
                Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = d資料確認;
                switch (d資料確認.eBranch)
                {
                    case 1:
                        return Json(new { result = true, msg = "登入成功", url = Url.Action("Boss首頁", "Boss") });
                    case 2:
                        return Json(new { result = true, msg = "登入成功", url = Url.Action("Business首頁", "Business") });
                    case 3:
                        return Json(new { result = true, msg = "登入成功", url = Url.Action("Accountant首頁", "Accountant") });
                    case 4:
                        return Json(new { result = true, msg = "登入成功", url = Url.Action("People首頁", "People") });
                    case 5:
                        return Json(new { result = true, msg = "登入成功", url = Url.Action("Commodity首頁", "Commodity") });
                    default:
                        return Json(new { result = true, msg = "帳號或密碼有誤", url = Url.Action("員工首頁", "Employee") });
                }
            }
            else
            {
                return Json(new { result = false, msg = "帳號或密碼有誤" });
            }
        }

        public ActionResult 員工登出()
        {
            Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = null;
            return RedirectToAction("員工登入");
        }

        public ActionResult 員工首頁()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
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
        public JsonResult ChangePassword(int EmployeeId, string ePassword, string oldpass)
        {//要用到其他地方
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee changeemp = new tEmployee();
                changeemp = emp;
                changeemp.EmployeeId = EmployeeId;
                dbContext.tEmployees.Add(changeemp);
                dbContext.SaveChanges();
                if (emp.ePassword == oldpass)
                {
                    emp.ePassword = ePassword;
                    Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = emp;
                    tEmployee changePwd = dbContext.tEmployees.Where(e => e.EmployeeId.Equals(emp.EmployeeId)).FirstOrDefault();
                    changePwd.ePassword = ePassword;
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
        //員工
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

        //產品
        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult 員工看產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            List<ProductViewModel> productlist = new List<ProductViewModel>();
            productlist = productRespotiory.GetProductAll();
            foreach(var items in productlist)
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
        public ActionResult 新增產品()
        {
            if (CSession關鍵字.SK_LOGINED_EMPLOYEE == null)
            {
                return RedirectToAction("員工登入");
            }
            ProductViewModel newprod = new ProductViewModel();
            var categories = new ProductRespoitory().GetCategoryAll();
            List<SelectListItem> Cateitems = new ProductRespoitory().GetPositionName(categories);
            newprod.Categories = Cateitems;

            var materials = new ProductRespoitory().GetMaterialAll();
            List<SelectListItem> Mateitems = new ProductRespoitory().GetPositionName(materials);
            newprod.Materials = Mateitems;

            var sizes = new ProductRespoitory().GetSizeAll();
            List<SelectListItem> Sizeitems = new ProductRespoitory().GetPositionName(sizes);
            newprod.Sizes = Sizeitems;
            return View(newprod);
        }
        [HttpPost]
        public ActionResult 新增產品(ProductViewModel n新增產品)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct prod = new tProduct();
            prod.pNumber = n新增產品.pNumber;
            prod.pName = n新增產品.pName;
            prod.pCategory = n新增產品.pCategory;
            prod.pMaterial = n新增產品.pMaterial;
            prod.pSize = n新增產品.pSize;
            prod.pLxWxH = n新增產品.pLxWxH;
            prod.pPrice = n新增產品.pPrice;
            prod.pQty = n新增產品.pQty;
            db.tProducts.Add(prod);

            tProduct Product = db.tProducts.OrderByDescending(o => o.ProductId).FirstOrDefault();
            int ProductId;
            if (Product == null)
            {
                ProductId = 1;
            }
            else
            {
                ProductId = Product.ProductId;
                ProductId++;
            }
            if (n新增產品.pImage.Count() > 0)
            {
                foreach (var uploagFile in n新增產品.pImage)
                {
                    if (uploagFile.ContentLength > 0)
                    {
                        tProductImage image = new tProductImage();
                        FileInfo file = new FileInfo(uploagFile.FileName);
                        string photoName = Guid.NewGuid().ToString() + file.Extension;
                        uploagFile.SaveAs(Server.MapPath("~/images/" + photoName));
                        image.ProductId = ProductId;
                        image.pImage = "~"+Url.Content("~/images/" + photoName);                        
                        db.tProductImages.Add(image);
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("員工看產品頁面");
        }
        public ActionResult 修改產品(int id)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct product = db.tProducts.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return RedirectToAction("員工看產品頁面");
            }
            EmpProductViewModel Prod = new EmpProductViewModel();
            Prod.ProductId = id;
            Prod.pNumber = product.pNumber;
            Prod.pName = product.pName;
            Prod.psCategory = product.tProductCategory.Category;
            Prod.pCategory = product.pCategory;
            Prod.psMaterial = product.tProductMaterial.Material;
            Prod.pMaterial = product.pMaterial;
            Prod.psSize = product.tProductSize.Size;
            Prod.pSize = product.pSize;
            Prod.pLxWxH = product.pLxWxH;
            Prod.pWeight = product.pWeight;
            Prod.pIntroduction = product.pIntroduction;
            Prod.pPrice = product.pPrice;            
            var categories = new ProductRespoitory().GetCategoryAll();
            List<SelectListItem> Cateitems = new ProductRespoitory().GetPositionName(categories);
            Prod.Categories = Cateitems;
            var materials = new ProductRespoitory().GetMaterialAll();
            List<SelectListItem> Mateitems = new ProductRespoitory().GetPositionName(materials);
            Prod.Materials = Mateitems;
            var sizes = new ProductRespoitory().GetSizeAll();
            List<SelectListItem> Sizeitems = new ProductRespoitory().GetPositionName(sizes);
            Prod.Sizes = Sizeitems;
            return View(Prod);

        }
        [HttpPost]
        public ActionResult 修改產品(EmpProductViewModel p)
        {
            if (CSession關鍵字.SK_LOGINED_EMPLOYEE == null)
            {
                return RedirectToAction("員工登入");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tProduct prod = dbContext.tProducts.Find(p.ProductId);
            if (prod != null)
            {
                prod.pNumber = p.pNumber;
                prod.pName = p.pName;
                prod.pCategory = p.pCategory;
                prod.pMaterial = p.pMaterial;
                prod.pSize = p.pSize;
                prod.pLxWxH = p.pLxWxH;
                prod.pWeight = p.pWeight;
                prod.pPrice = p.pPrice;
                List<tProductImage> oldImages = dbContext.tProductImages.Where(imgs => imgs.ProductId.Equals(p.ProductId)).ToList();
                if (oldImages.Count > p.pImage.Count)
                {
                    int index = 0;                    
                    foreach(var oldItem in oldImages)
                    {
                        if(index< p.pImage.Count)
                        {
                            if (p.pImage[index]==null)
                            {
                                break;
                            }
                                if (p.pImage[index].ContentLength > 0)
                            {
                                FileInfo file = new FileInfo(p.pImage[index].FileName);
                                string photoName = Guid.NewGuid().ToString() + file.Extension;
                                p.pImage[index].SaveAs(Server.MapPath("~/images/" + photoName));
                                oldItem.pImage = Url.Content("~/images/" + photoName);
                                //Directory.Delete(Url.Content(oldItem.pImage));
                            }
                        }
                        else
                        {
                            dbContext.tProductImages.Remove(oldItem);
                        }
                        index++;
                    }
                }
                else
                {
                    int index = 0;
                    foreach (var item in p.pImage)
                    {
                        if (index < oldImages.Count)
                        {
                            FileInfo file = new FileInfo(item.FileName);
                            string photoName = Guid.NewGuid().ToString() + file.Extension;
                            item.SaveAs(Server.MapPath("~/images/" + photoName));
                            oldImages[index].pImage = Url.Content("~/images/" + photoName);
                        }
                        else
                        {
                            tProductImage image = new tProductImage();
                            FileInfo file = new FileInfo(item.FileName);
                            string photoName = Guid.NewGuid().ToString() + file.Extension;
                            item.SaveAs(Server.MapPath("~/images/" + photoName));
                            image.ProductId = p.ProductId;
                            image.pImage = "~" + Url.Content("~/images/" + photoName);
                            dbContext.tProductImages.Add(image);
                        }
                        index++;
                    }
                }
                dbContext.SaveChanges();
            }
            return RedirectToAction("員工看產品頁面");
        }
        public JsonResult 修改產品讀圖(int ProductId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            var imageArray = dbContext.tProductImages.Where(i => i.ProductId.Equals(ProductId)).ToArray();
            if (imageArray.Length > 0)
            {
                List<string> imagelist = new List<string>();
                foreach (var items in imageArray)
                {
                    string image = Url.Content(items.pImage);
                    imagelist.Add(image);
                }
                string[] imagearray = imagelist.ToArray();
                return Json(new { images = imagearray });
            }
            else
            {
                return Json(new { images = "" });
            }
        }

        public ActionResult 工作日誌()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {

                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                var dlist = dbContext.tDiaries.OrderBy(c => c.dEmployeeId).ToList();                
                

                List<DiaryViewModel> DSaw = new List<DiaryViewModel>();
                foreach (var item in dlist)
                {
                    tWarehouseName warename = dbContext.tWarehouseNames.Where(w => w.WarehouseNameId.Equals(item.dWarehouseNameId)).FirstOrDefault();
                    DiaryViewModel show = new DiaryViewModel();
                    show.eName = item.tEmployee.eName;
                    show.dDate = item.dDate;
                    show.dWeather = item.dWeather; 
                    show.dDiaryNote = item.dDiaryNote;
                    show.dWarehouseNameId = item.dWarehouseNameId;
                    show.dWarehouseName = warename.WarehouseName;
                    DSaw.Add(show);
                }
                return View(DSaw);
            }
            return RedirectToAction("員工登入");
        }
        private CommodityRespoitory commodityRespoitory = new CommodityRespoitory();
        public ActionResult 新增日誌()
        {
            tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;

            ViewBag.name = emp.eName;
            ViewBag.empId = emp.EmployeeId;
            DiaryViewModel newDiary = new DiaryViewModel();
            var warehouses = commodityRespoitory.GetWarehouseAll();
            List<SelectListItem> WareList = commodityRespoitory.GetSelectList(warehouses);            
            newDiary.warehouses = WareList;

            return View(newDiary);
        }
        [HttpPost]
        public ActionResult 新增日誌(DiaryViewModel data)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {

                MotaiDataEntities db = new MotaiDataEntities();
                tDiary diary = new tDiary();
                List<DiaryViewModel> DShow = new List<DiaryViewModel>();
                diary.dEmployeeId = data.dEmployeeId;
                diary.DiaryId = data.DiaryId;
                diary.dDate = data.dDate;
                diary.dWeather = data.dWeather;
                diary.dWarehouseNameId = data.dWarehouseNameId;
                diary.dDiaryNote = data.dDiaryNote;
                db.tDiaries.Add(diary);
                db.SaveChanges();
                return RedirectToAction("員工首頁");
            }
            return RedirectToAction("員工首頁");
        }

        public ActionResult 修改日誌(int id)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                MotaiDataEntities db = new MotaiDataEntities();
                tDiary diary = db.tDiaries.Where(d => d.dEmployeeId.Equals(emp.EmployeeId)).FirstOrDefault();
                DiaryViewModel Diary = new DiaryViewModel();
                //Diary.Diary = diary;
                return View(Diary);
            }
            return RedirectToAction("員工登入");

        }

        public ActionResult 會計審核()
        {
            OrderViewModel CheckOrder = new OrderViewModel();
            return View(CheckOrder);
        }
        public ActionResult 會計查詢()
        {
            List<OrderViewModel> CheckOrder = new List<OrderViewModel>();
            return View(CheckOrder);
        }


        private PromotionRespoitory promotionRespoitory = new PromotionRespoitory();
        public ActionResult 員工看消息()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            List<DetailPromotionViewModel> promotionlist = new List<DetailPromotionViewModel>();
            promotionlist = promotionRespoitory.GetPromotionAll();
            return View(promotionlist);
        }

        public ActionResult 新增消息()
        {
            if (CSession關鍵字.SK_LOGINED_EMPLOYEE != null)
            {
                DetailPromotionViewModel NewPromo = new DetailPromotionViewModel();
                var categories = new PromotionRespoitory().GetPromoCategoryAll();
                List<SelectListItem> Cateitems = new CommodityRespoitory().GetSelectList(categories);
                NewPromo.Categories = Cateitems;
                return View(NewPromo);

            }
            return RedirectToAction("員工登入");
        }

        [HttpPost]
        public ActionResult 新增消息(DetailPromotionViewModel create消息)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tPromotion n消息 = new tPromotion();
            n消息.PromotionName = create消息.PromotionName;
            n消息.PromotinoCategory = create消息.pCategory;
            n消息.PromotionDescription = create消息.PromotionDescription;
            n消息.pPromotionStartDate = create消息.pPromotionStartDate;
            n消息.pPromotionDeadline = create消息.pPromotionDeadline;
            //n消息.pADimage = create消息.pADimage;
            n消息.pDiscountCode = create消息.pDiscountCode;
            n消息.pDiscount = create消息.pDiscount;
            n消息.pCondition = create消息.pCondition;
            var date = DateTime.Now;
            n消息.pPromotionPostDate = date;
          
            int PromotionId = dbContext.tPromotions.OrderByDescending(o => o.PromotionId).First().PromotionId;
            PromotionId = PromotionId + 1;

            //if (create消息.upLoadimage.Count() > 0)
            //{               
            //    foreach (var uploagFile in create消息.upLoadimage)
            //    {
            //        if (uploagFile.ContentLength > 0)
            //        {
            //            FileInfo file = new FileInfo(uploagFile.FileName);
            //            string photoName = Guid.NewGuid().ToString() + file.Extension;
            //            uploagFile.SaveAs(Server.MapPath("~/images/" + photoName));
            //            n消息.pADimage = "~" + Url.Content("~/images/" + photoName);
            //            dbContext.tPromotions.Add(n消息);
            //            dbContext.SaveChanges();
            //        }
            //    }
            //}
            dbContext.SaveChanges();
            return RedirectToAction("員工看消息");


            //if (n新增產品.pImage.Count() > 0)
            //{
            //    foreach (var uploagFile in n新增產品.pImage)
            //    {
            //        if (uploagFile.ContentLength > 0)
            //        {
            //            tProductImage image = new tProductImage();
            //            FileInfo file = new FileInfo(uploagFile.FileName);
            //            string photoName = Guid.NewGuid().ToString() + file.Extension;
            //            uploagFile.SaveAs(Server.MapPath("~/images/" + photoName));
            //            image.ProductId = ProductId;
            //            image.pImage = "~" + Url.Content("~/images/" + photoName);
            //            db.tProductImages.Add(image);
            //        }
            //    }
            //}
        }

        public ActionResult 修改消息(int id)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tPromotion promotion = dbContext.tPromotions.FirstOrDefault(p => p.PromotionId == id);
            if (promotion == null)
            {
                return RedirectToAction("員工看產品頁面");
            }
            DetailPromotionViewModel Promo = new DetailPromotionViewModel();
            Promo.pADimage = promotion.pADimage;
            Promo.pCondition = promotion.pCondition;
            Promo.pDiscount = promotion.pDiscount;
            Promo.pPromotionDeadline = promotion.pPromotionDeadline;
            Promo.pPromotionPostDate = promotion.pPromotionPostDate;
            Promo.pPromotionStartDate = promotion.pPromotionStartDate;
            Promo.pPromotionWeb = promotion.pPromotionWeb;
            Promo.PromotionDescription = promotion.PromotionDescription;
            Promo.sPromotinoCategory = promotion.tPromotionCategory.PromtionCategory;
            Promo.PromotionName = promotion.PromotionName;
            Promo.pDiscountCode = promotion.pDiscountCode;
            Promo.PromotionId = promotion.PromotionId;

            Promo.sPromotinoCategory = promotion.tPromotionCategory.PromtionCategory;
            Promo.pCategory = promotion.PromotinoCategory;
            var categories = new ProductRespoitory().GetCategoryAll();
            List<SelectListItem> Cateitems = new ProductRespoitory().GetPositionName(categories);
            Promo.Categories = Cateitems;
            return View(Promo);
        }
        [HttpPost]
        public ActionResult 修改消息(DetailPromotionViewModel promotion)
        {
            if (CSession關鍵字.SK_LOGINED_EMPLOYEE == null)
            {
                return RedirectToAction("員工登入");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tPromotion Promo = dbContext.tPromotions.FirstOrDefault(p => p.PromotionId == promotion.PromotionId);
            if (Promo != null)
            {
                Promo.pADimage = promotion.pADimage;
                Promo.pCondition = promotion.pCondition;
                Promo.pDiscount = promotion.pDiscount;
                Promo.pPromotionDeadline = promotion.pPromotionDeadline;
                Promo.pPromotionPostDate = promotion.pPromotionPostDate;
                Promo.pPromotionStartDate = promotion.pPromotionStartDate;
                Promo.pPromotionWeb = promotion.pPromotionWeb;
                Promo.PromotionDescription = promotion.PromotionDescription;
                Promo.PromotinoCategory = promotion.pCategory;
                Promo.PromotionName = promotion.PromotionName;
                Promo.pDiscountCode = promotion.pDiscountCode;
                Promo.PromotionId = promotion.PromotionId;
                dbContext.SaveChanges();
            }
            return RedirectToAction("員工看消息");
        }


        public JsonResult 修改消息讀圖(int id)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            var imageArray = dbContext.tProductImages.Where(i => i.ProductId.Equals(id)).ToArray();
            if (imageArray.Length > 0)
            {
                List<string> imagelist = new List<string>();
                foreach (var items in imageArray)
                {
                    string image = Url.Content(items.pImage);
                    imagelist.Add(image);
                }
                string[] imagearray = imagelist.ToArray();
                return Json(new { images = imagearray });
            }
            else
            {
                return Json(new { images = "" });
            }
        }
    }
}