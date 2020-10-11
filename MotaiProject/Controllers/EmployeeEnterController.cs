using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class EmployeeEnterController : Controller
    {
        // GET: EmployeeEnter
        public ActionResult 員工首頁()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入","EmployeeLogin");
            }
            return View();
        }
        //員工
        public ActionResult 新增員工()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "EmployeeLogin");
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
                return RedirectToAction("員工登入", "EmployeeLogin");
            }
            var q = from p in (new MotaiDataEntities()).tProducts//先撈資料,產品的工廠
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
            foreach(var c in categories)
            {
                Cateitems.Add(new SelectListItem()
                {
                    Text = c.Value,
                    Value = c.Key.ToString()
                });
            }
            newprod.pCategory = Cateitems;

            List<tProductMaterial> materials = new ClassMethod().GetMaterialAll();
            List<SelectListItem> Mateitems = new List<SelectListItem>();
            foreach(var m in materials)
            {
                Mateitems.Add(new SelectListItem()
                {
                    Text = m.Material,
                    Value = m.pMaterialId.ToString()
                });
            }
            newprod.pMaterial = Mateitems;

            List<tProductSize> sizes = new ClassMethod().GetSizeAll();
            List<SelectListItem> Sizeitems = new List<SelectListItem>();
            foreach(var s in sizes)
            {
                Sizeitems.Add(new SelectListItem() {
                    Text = s.Size,
                    Value = s.pSizeId.ToString()
                });
            }
            newprod.pSize = Sizeitems;
            return View(newprod);
        }
        [HttpPost]
        public ActionResult 新增產品(ProductViewModel n新增產品)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct prod = new tProduct();
            prod.pNumber = n新增產品.pNumber;
            prod.pName = n新增產品.pName;
            //prod.pCategory = db.tProductCategories.Where(c => c.Category == n新增產品.pCategory);



            db.tProducts.Add(prod);
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
                
                //prod.pNumber = p.pNumber;
                //prod.pName = p.pName;
                //prod.pCategory = p.pCategory;
                //prod.pMaterial = p.pMaterial;
                //prod.pSize = p.pSize;
                //prod.pLxWxH = p.pLxWxH;
                //prod.pWeight = p.pWeight;
                //prod.pImage = p.pImage;
                //prod.pPrice = p.pPrice;
                //prod.pQty = p.pQty;
                db.SaveChanges();
            }
            return RedirectToAction("員工看產品頁面");
        }
        public ActionResult Delete(int id)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct prod = db.tProducts.FirstOrDefault(p => p.ProductId == id);
            if (prod != null)
            {
                db.tProducts.Remove(prod);
                db.SaveChanges();

            }
            return RedirectToAction("員工看產品頁面");
        }
    }
}