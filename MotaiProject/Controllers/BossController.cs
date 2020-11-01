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
            return RedirectToAction("員工登入","Customer");
        }

        public ActionResult Boss首頁()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Customer");
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
                //tEmployee changeemp = new tEmployee();
                //changeemp =emp;
                //changeemp.EmployeeId= EmployeeId;
                //dbContext.tEmployees.Add(changeemp);
                //dbContext.SaveChanges()
                ;
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

        public ActionResult 工作日誌()
        {//要再改
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                var dlist = dbContext.tDiaries.Where(d => d.dEmployeeId.Equals(emp.EmployeeId));
                List<DiaryViewModel> diarylist = new List<DiaryViewModel>();
                foreach (tDiary item in dlist)
                {
                    DiaryViewModel diary = new DiaryViewModel();
                    //diary.Diary = item;
                    diarylist.Add(diary);
                }
                return View(diarylist);
            }
            return RedirectToAction("員工登入", "Customer");
        }
        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult Boss產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Customer");
            }
            List<ProductViewModel> productlist = new List<ProductViewModel>();
            productlist = productRespotiory.GetProductAll();
            return View(productlist);
        }
        public ActionResult 銷售數據()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Customer");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            var favorOrderby = from i in dbContext.tFavorites
                                group i by i.fProductId into j
                                select new
                                {
                                    Pid = j.Key,
                                    Pcount = j.Count(),
                                };

            return View(favorOrderby);

        }
    }
}