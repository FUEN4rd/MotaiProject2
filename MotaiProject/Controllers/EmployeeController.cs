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
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult 員工登入()
        {
            return View();
        }
        [HttpPost]
        public ActionResult 員工登入(EmployeeLoginViewModel e登入資料)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tEmployee d資料確認 = dbContext.tEmployees.Where(e => e.eAccount == e登入資料.eAccount && e.ePassword.Equals(e登入資料.ePassword)).FirstOrDefault();
            if (d資料確認 != null)
            {
                Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = d資料確認;
                return RedirectToAction("員工首頁");
            }
            else
            {
                Response.Write("帳號密碼錯誤!");
                return View();
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
                employee.Employee = emp;
                return View(employee);
            }
        }
        public JsonResult ChangePassword(int EmployeeId,string ePassword,string oldpass)
        {//要用到其他地方
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                //tEmployee changeemp = new tEmployee();
                //changeemp =emp;
                //changeemp.EmployeeId= EmployeeId;
                //dbContext.tEmployees.Add(changeemp);
                //dbContext.SaveChanges();    
                if (emp.ePassword==oldpass)
                {                  
                    emp.ePassword=ePassword;
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
        public ActionResult 新增員工(EmployeeViewModels n新增員工)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (dbContext.tEmployees.Count().Equals(0))
            {
                n新增員工.EmployeeId = 1;
            }
            tEmployee n新員工 = new tEmployee();
            n新員工 = n新增員工.Employee;
            dbContext.tEmployees.Add(n新員工);
            dbContext.SaveChanges();
            return RedirectToAction("員工首頁");
        }

        //產品
        public ActionResult 員工看產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            var q = from p in (new MotaiDataEntities()).tProducts
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
        public ActionResult 新增產品()
        {
            ProductViewModel newprod = new ProductViewModel();
            var categories = new ClassMethod().GetCategoryAll();
            List<SelectListItem> Cateitems = new List<SelectListItem>();
            foreach (var c in categories)
            {
                Cateitems.Add(new SelectListItem()
                {
                    Text = c.Value,
                    Value = c.Key.ToString()
                });
            }
            newprod.Categories = Cateitems;

            var materials = new ClassMethod().GetMaterialAll();
            List<SelectListItem> Mateitems = new List<SelectListItem>();
            foreach (var m in materials)
            {
                Mateitems.Add(new SelectListItem()
                {
                    Text = m.Value,
                    Value = m.Key.ToString()
                });
            }
            newprod.Materials = Mateitems;

            var sizes = new ClassMethod().GetSizeAll();
            List<SelectListItem> Sizeitems = new List<SelectListItem>();
            foreach (var s in sizes)
            {
                Sizeitems.Add(new SelectListItem()
                {
                    Text = s.Value,
                    Value = s.Key.ToString()
                });
            }
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
            
            int ProductId = db.tProducts.OrderByDescending(o=>o.ProductId).First().ProductId;
            ProductId = ProductId + 1;
            if(n新增產品.pImage.Count()> 0)
            {
                foreach(var uploagFile in n新增產品.pImage)
                {
                    if(uploagFile.ContentLength > 0)
                    {
                        tProductImage image = new tProductImage();
                        FileInfo file = new FileInfo(uploagFile.FileName);
                        string photoName = Guid.NewGuid().ToString() + file.Extension;
                        uploagFile.SaveAs(Server.MapPath("../Content/" + photoName));
                        image.ProductId = ProductId;
                        image.pImage = "../Content/" + photoName;
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
            tProduct prod = db.tProducts.FirstOrDefault(p => p.ProductId == id);
            if (prod == null)
            {
                return RedirectToAction("員工看產品頁面");
            }
            ProductViewModel product = new ProductViewModel();
            product.Product = prod;
            return View(product);

        }
        [HttpPost]
        public ActionResult 修改產品(ProductViewModel p)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct prod = db.tProducts.Find(p.ProductId);
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
                prod.pQty = p.pQty;
                db.SaveChanges();
            }
            return RedirectToAction("員工看產品頁面");
        }

        public ActionResult 工作日誌()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                var dlist = dbContext.tDiaries.Where(d => d.dEmployeeId.Equals(emp.EmployeeId));
                List<DiaryViewModel> diarylist = new List<DiaryViewModel>();
                foreach (tDiary item in dlist)
                {
                    DiaryViewModel diary = new DiaryViewModel();
                    diary.Diary = item;
                    diarylist.Add(diary);
                }
                return View(diarylist);
            }
            return RedirectToAction("員工登入");
        }

        public ActionResult 新增日誌()
        {
            tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
            ViewBag.name = emp.eName;
            ViewBag.empId = emp.EmployeeId;
            DiaryViewModel newDiary = new DiaryViewModel();
            var warehouses = new ClassMethod().GetCategoryAll();
            List<SelectListItem> WareList = new List<SelectListItem>();
            foreach (var item in warehouses)
            {
                WareList.Add(new SelectListItem()
                {
                    Text = item.Value,
                    Value = item.Key.ToString()
                });
            }
            newDiary.WarehouseName = WareList;
           
            return View(newDiary);
        }
        [HttpPost]
        public ActionResult 新增日誌(DiaryViewModel data)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                MotaiDataEntities db = new MotaiDataEntities();
                tDiary diary = new tDiary();
                diary = data.Diary;
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
                Diary.Diary = diary;
                return View(Diary);
            }
            return View("員工登入");
            
        }

        public ActionResult 會計審核()
        {
            OrderViewModel CheckOrder = new OrderViewModel();
            return View(CheckOrder);
        }

    }
}