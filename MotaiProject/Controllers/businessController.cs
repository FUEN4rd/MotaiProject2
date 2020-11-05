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
    public class BusinessController : Controller
    {
        // GET: business
        public ActionResult Business首頁()
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
        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult 業務看產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
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
                    show.DiaryId = item.DiaryId;
                    show.dWarehouseNameId = item.dWarehouseNameId;
                    show.dWarehouseName = warename.WarehouseName;
                    DSaw.Add(show);
                }
                return View(DSaw);
            }
            return RedirectToAction("員工登入", "Employee");
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
                return RedirectToAction("工作日誌");
            }
            return RedirectToAction("員工登入","Employee");
        }

        public ActionResult 修改日誌(int id)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                MotaiDataEntities db = new MotaiDataEntities();
                tDiary diary = db.tDiaries.FirstOrDefault(m=>m.DiaryId==id);
                DiaryViewModel Diary = new DiaryViewModel();
                Diary.DiaryId = id;
                Diary.dEmployeeId = diary.dEmployeeId;
                Diary.eName = diary.tEmployee.eName;
                Diary.dWeather = diary.dWeather;
                Diary.dDate = diary.dDate;
                Diary.dDiaryNote = diary.dDiaryNote;
                Diary.dWarehouseNameId = diary.dWarehouseNameId;

                var warehouses = commodityRespoitory.GetWarehouseAll();
                List<SelectListItem> WareList = commodityRespoitory.GetSelectList(warehouses);
                Diary.warehouses = WareList;
                return View(Diary);
            }
            return RedirectToAction("員工登入", "Employee");

        }
        [HttpPost]
        public ActionResult 修改日誌(DiaryViewModel m)
        {

            MotaiDataEntities dbcontext = new MotaiDataEntities();
            tDiary diary = dbcontext.tDiaries.Find(m.DiaryId);
            if (diary != null)
            {
                diary.DiaryId = m.DiaryId;
                diary.dEmployeeId = m.dEmployeeId;
                diary.dDate = m.dDate;
                diary.dDiaryNote = m.dDiaryNote;
                diary.dWeather = m.dWeather;
                diary.dWarehouseNameId = m.dWarehouseNameId;
                dbcontext.SaveChanges();
                return RedirectToAction("工作日誌");
            }

            return View("業務看產品頁面");
        }



        public ActionResult 業務會計查詢()
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
            n消息.pDiscountCode = create消息.pDiscountCode;
            n消息.pDiscount = create消息.pDiscount;
            n消息.pCondition = create消息.pCondition;
            var date = DateTime.Now;
            n消息.pPromotionPostDate = date;

            int PromotionId = dbContext.tPromotions.OrderByDescending(o => o.PromotionId).First().PromotionId;
            PromotionId = PromotionId + 1;
            var uploagFile = create消息.upLoadimage;
            if (uploagFile.ContentLength > 0)
            {
                FileInfo file = new FileInfo(uploagFile.FileName);
                string photoName = Guid.NewGuid().ToString() + file.Extension;
                uploagFile.SaveAs(Server.MapPath("~/images/" + photoName));
                n消息.pADimage = "../../images/" + Url.Content(photoName);
                dbContext.tPromotions.Add(n消息);
            }
            dbContext.SaveChanges();
            return RedirectToAction("員工看消息");
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

                var uploagFile = promotion.upLoadimage;
                if (uploagFile.ContentLength > 0)
                {
                    FileInfo file = new FileInfo(uploagFile.FileName);
                    string photoName = Guid.NewGuid().ToString() + file.Extension;
                    uploagFile.SaveAs(Server.MapPath("~/images/" + photoName));
                    Promo.pADimage = "../../images/" + Url.Content(photoName);
                    dbContext.tPromotions.Add(Promo);
                }

                dbContext.SaveChanges();
            }
            return RedirectToAction("員工看消息");
        }

        public JsonResult 修改消息讀圖(int PromotionId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tPromotion Promo = dbContext.tPromotions.FirstOrDefault(p => p.PromotionId == PromotionId);
            if (Promo.pADimage != null)
            {
                string image = Url.Content(Promo.pADimage);
                return Json(new { images = image });
            }
            else
            {
                return Json(new { images = "" });
            }
        }

    }
}