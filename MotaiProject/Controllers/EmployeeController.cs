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
                dbContext.SaveChanges()
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
            List<SelectListItem> Cateitems = new ProductRespoitory().GetSelectList(categories);
            newprod.Categories = Cateitems;

            var materials = new ProductRespoitory().GetMaterialAll();
            List<SelectListItem> Mateitems = new ProductRespoitory().GetSelectList(materials);
            newprod.Materials = Mateitems;

            var sizes = new ProductRespoitory().GetSizeAll();
            List<SelectListItem> Sizeitems = new ProductRespoitory().GetSelectList(sizes);
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

            int ProductId = db.tProducts.OrderByDescending(o => o.ProductId).First().ProductId;
            ProductId = ProductId + 1;
            if (n新增產品.pImage.Count() > 0)
            {
                foreach (var uploagFile in n新增產品.pImage)
                {
                    if (uploagFile.ContentLength > 0)
                    {
                        tProductImage image = new tProductImage();
                        FileInfo file = new FileInfo(uploagFile.FileName);
                        string photoName = Guid.NewGuid().ToString() + file.Extension;
                        uploagFile.SaveAs(Server.MapPath("../images/" + photoName));
                        image.ProductId = ProductId;
                        image.pImage = "~/images/" + photoName;
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
            ProductViewModel Prod = new ProductViewModel();
            Prod.pNumber = product.pNumber;
            Prod.pName = product.pName;
            Prod.psCategory = product.tProductCategory.Category;
            Prod.psMaterial = product.tProductMaterial.Material;
            Prod.psSize = product.tProductSize.Size;
            Prod.pLxWxH = product.pLxWxH;
            Prod.pWeight = product.pWeight;
            Prod.pIntroduction = product.pIntroduction;
            Prod.pPrice = product.pPrice;
            Prod.pQty = (int)product.pQty;
            return View(Prod);

        }
        [HttpPost]
        public ActionResult 修改產品(EmpProductViewModel p)
        {
            if (CSession關鍵字.SK_LOGINED_EMPLOYEE == null)
            {
                return RedirectToAction("員工登入");
            }
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
                var dlist = dbContext.tDiaries.OrderBy(c => c.dEmployeeId).ToList();
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
            return RedirectToAction("員工登入");
        }

        public ActionResult 新增日誌()
        {
            tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;

            ViewBag.name = emp.eName;
            ViewBag.empId = emp.EmployeeId;
            DiaryViewModel newDiary = new DiaryViewModel();
            var warehouses = new ProductRespoitory().GetCategoryAll();
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
            return View("員工登入");

        }

        public ActionResult 會計審核()
        {
            OrderViewModel CheckOrder = new OrderViewModel();
            return View(CheckOrder);
        }

    }
}