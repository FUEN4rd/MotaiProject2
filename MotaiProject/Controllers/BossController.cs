using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class BossController : Controller
    {
        // GET: Boss
        public ActionResult Boss登出()
        {
            Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = null;
            return RedirectToAction("員工登入", "Employee");
        }

        public ActionResult Boss首頁()
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
        public JsonResult ChangePassword(int EmployeeId, string ePassword, string oldpass)
        {//要用到其他地方
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                MotaiDataEntities dbContext = new MotaiDataEntities();
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

        public ActionResult Boss看工作日誌()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {

                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                var dlist = dbContext.tDiaries.ToList();
                List<DiaryViewModel> DSaw = new List<DiaryViewModel>();
                foreach (var item in dlist)
                {
                    DiaryViewModel show = new DiaryViewModel();
                    show.eName = item.tEmployee.eName;
                    show.dDate = item.dDate;
                    show.dWeather = item.dWeather;
                    show.dDiaryNote = item.dDiaryNote;
                    show.dWarehouseNameId = item.dWarehouseNameId;

                    DSaw.Add(show);
                }
                return View(DSaw);
            }
            return RedirectToAction("員工登入", "Employee");
        }
        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult Boss產品頁面()
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
        public ActionResult 銷售數據()
        {//改viewmodel
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            var favorOrder = (from i in dbContext.tFavorites
                                group i by i.fProductId into j
                                select new
                                {
                                    Pid = j.Key,
                                    Pcount = j.Count(),
                                }).ToList();
            var buyOrder = (from i in dbContext.tOrderDetails
                            group i by i.oProductId into j
                            select new
                            {
                                Pid = j.Key,
                                Pcount = j.Sum(p => p.oProductQty)
                            }).ToList();
            List<BossViewModel> BossV = new List<BossViewModel>();
            BossViewModel trans = new BossViewModel();
            List<favorViewModel> favorV = new List<favorViewModel>();
            List<buyViewModel> buyV = new List<buyViewModel>();

            foreach(var item in favorOrder)
            {  
                favorViewModel favor = new favorViewModel();               
                favor.favorID = item.Pid;
                favor.faverCount = item.Pcount;
                favorV.Add(favor);
            }
            foreach (var item in buyOrder)
            {
                buyViewModel buy = new buyViewModel();
                buy.buyID = item.Pid;
                buy.buyCount = item.Pcount;
                buyV.Add(buy);
            }
            foreach(var item in favorV)
            {
                BossViewModel bv = new BossViewModel();
                foreach (var item2 in buyV)
                {
                    bv.favorV = item;
                    bv.buyV = item2;
                }
                BossV.Add(bv);
            }
            return View(BossV);


        }
    }
}