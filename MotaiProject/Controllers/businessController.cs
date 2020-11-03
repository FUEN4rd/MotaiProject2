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
                        uploagFile.SaveAs(Server.MapPath("~/images/" + photoName));
                        image.ProductId = ProductId;
                        image.pImage = "~" + Url.Content("~/images/" + photoName);
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
            List<SelectListItem> Cateitems = new ProductRespoitory().GetSelectList(categories);
            Prod.Categories = Cateitems;
            var materials = new ProductRespoitory().GetMaterialAll();
            List<SelectListItem> Mateitems = new ProductRespoitory().GetSelectList(materials);
            Prod.Materials = Mateitems;
            var sizes = new ProductRespoitory().GetSizeAll();
            List<SelectListItem> Sizeitems = new ProductRespoitory().GetSelectList(sizes);
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
                    foreach (var oldItem in oldImages)
                    {
                        if (index < p.pImage.Count)
                        {
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
        public ActionResult 業務會計查詢()
        {
            List<OrderViewModel> CheckOrder = new List<OrderViewModel>();
            return View(CheckOrder);
        }
    }
}