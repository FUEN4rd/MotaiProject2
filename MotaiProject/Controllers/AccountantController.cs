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
    public class AccountantController : Controller
    {
        // GET: Accountant
        public ActionResult Accountant首頁()
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
        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult 會計看產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
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

    }
}