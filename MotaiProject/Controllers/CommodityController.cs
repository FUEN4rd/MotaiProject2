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

    public class CommodityController : Controller
    {
        public ActionResult Commodity首頁()
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
        public ActionResult 員工看產品頁面()
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



        // GET: Commodity
        public ActionResult Index()
        {
            return View();
        }
        private CommodityRespoitory commodityRespoitory = new CommodityRespoitory();
        private ProductRespoitory productRespoitory = new ProductRespoitory();
        //進貨單
        public ActionResult 進貨單建立()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                StockCreateViewModel model = new StockCreateViewModel();
                StockDetailViewModel detail = new StockDetailViewModel();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                model.sEmployeeId = emp.EmployeeId;
                var productNames = productRespoitory.GetNameAll();
                List<SelectListItem> productlist = productRespoitory.GetSelectList(productNames);
                var warehouseNames = commodityRespoitory.GetWarehouseAll();
                List<SelectListItem> warehouselist = commodityRespoitory.GetSelectList(warehouseNames);
                detail.WareHouseNames = warehouselist;
                detail.ProductNames = productlist;
                model.StockDetail = detail;
                return View(model);
            }
            return RedirectToAction("員工登入", "Employee");
            
        }
        [HttpPost]
        public JsonResult 進貨單建立(StockCreateViewModel stockList)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                if (Session[CSession關鍵字.SK_STOCKDETAIL] == null)
                {
                    return Json(new {result=false,msg="進貨單尚未完成!",url=""});
                }
                else
                {
                    List<StockDetailViewModel> stocks = Session[CSession關鍵字.SK_STOCKDETAIL] as List<StockDetailViewModel>;
                    MotaiDataEntities dbContext = new MotaiDataEntities();
                    tStockList list = new tStockList();
                    list.sEmployeeId = emp.EmployeeId;
                    list.sStockSerialValue = stockList.sStockSerialValue;
                    list.sVendor = stockList.sVendor;
                    list.sVendorTel = stockList.sVendorTel;
                    list.sStockDate = stockList.sStockDate;
                    list.sStockNote = stockList.sStockNote;
                    dbContext.tStockLists.Add(list);
                    dbContext.SaveChanges();
                    foreach (var items in stocks)
                    {
                        tStockDetail detail = new tStockDetail();
                        detail.sStockId = dbContext.tStockLists.OrderByDescending(i => i.StockId).First().StockId;
                        detail.sProductId = items.sProductId;
                        detail.sCost = items.sCost;
                        detail.sQuantity = items.sQuantity;
                        detail.sWarehouseNameId = items.sWarehouseNameId;
                        detail.sNote = items.sNote;
                        dbContext.tStockDetails.Add(detail);
                    }
                    dbContext.SaveChanges();
                    Session[CSession關鍵字.SK_STOCKDETAIL] = null;
                    return Json(new { result=true,msg="新增成功",url=Url.Action("進貨單建立","Commodity")});
                }
            }
            else
            {
                return Json(new { result = false, msg = "尚未登入!",url=Url.Action("員工登入","Employee")});
            }
        }
        [HttpPost]
        public string createStockDetail(StockDetailViewModel stockDetail)
        {
            if(Session[CSession關鍵字.SK_STOCKDETAIL] == null)
            {
                List<StockDetailViewModel> stocks = new List<StockDetailViewModel>();
                stocks.Add(stockDetail);
                Session[CSession關鍵字.SK_STOCKDETAIL] = stocks;
            }
            else
            {
                List<StockDetailViewModel> stocks = Session[CSession關鍵字.SK_STOCKDETAIL] as List<StockDetailViewModel>;
                stocks.Add(stockDetail);
                Session[CSession關鍵字.SK_STOCKDETAIL] = stocks;
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            stockDetail.ProductName = dbContext.tProducts.Where(s => s.ProductId.Equals(stockDetail.sProductId)).FirstOrDefault().pName;
            stockDetail.WareHouseName = dbContext.tWarehouseNames.Where(w => w.WarehouseNameId.Equals(stockDetail.sWarehouseNameId)).FirstOrDefault().WarehouseName;

            string data = "<tr><td scope='row'>";
            if (stockDetail.sNote != null)
            {                
                data += stockDetail.ProductName.ToString() + "</td><td>";
                data += stockDetail.sCost.ToString() + "</td><td>";
                data += stockDetail.sQuantity.ToString() + "</td><td>";
                data += stockDetail.WareHouseName.ToString() + "</td><td>";
                if (stockDetail.sNote.Length > 10)
                {
                    for(int i=0;i< stockDetail.sNote.Length / 10; i++)
                    {
                        data += stockDetail.sNote.Substring(i * 10, 10)+"<br>";
                    }
                    data += "</td>";
                }
                else
                {
                    data += stockDetail.sNote.ToString() + "</td>";
                }                            
            }
            else
            {                
                data += stockDetail.ProductName.ToString() + "</td><td>";
                data += stockDetail.sCost.ToString() + "</td><td>";
                data += stockDetail.sQuantity.ToString() + "</td><td>";
                data += stockDetail.WareHouseName.ToString() + "</td><td>";
                data += "</td>";                
            }
            return data;
        }
        //刪除json deleteDetail
        [HttpPost]
        public JsonResult deleteStockDetail(int index)
        {
            List<StockDetailViewModel> stocks = Session[CSession關鍵字.SK_STOCKDETAIL] as List<StockDetailViewModel>;
            stocks.RemoveAt(index);
            Session[CSession關鍵字.SK_STOCKDETAIL] = stocks;
            return Json(new {msg="已刪除" });
        }
        public ActionResult 進貨單查詢()
        {
            return View();
        }

        //出貨單
        public ActionResult 出貨單建立()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {

            }
            return View();
        }
        public JsonResult 出貨單建立(int i)
        {
            return Json(new { });
        }
        //調貨單
        //倉儲
        public ActionResult 倉儲查詢()
        {
            return View();
        }
    }
}