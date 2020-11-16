using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

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
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                EmployeeViewModels employee = new EmployeeViewModels();
                employee.EmployeeId = emp.EmployeeId;
                employee.eName = emp.eName;
                employee.eAccount = emp.eAccount;
                employee.sPosition = emp.tPosition.pPosition;
                return View(employee);
            }
        }

        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult 倉儲看產品頁面()
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
        public ActionResult 新增產品()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
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
            prod.pWeight = n新增產品.pWeight;
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
            return RedirectToAction("倉儲看產品頁面");
        }
        public ActionResult 修改產品(int id)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities db = new MotaiDataEntities();
            tProduct product = db.tProducts.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return RedirectToAction("倉儲看產品頁面");
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
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
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
                            if (p.pImage[index] == null)
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
            return RedirectToAction("倉儲看產品頁面");
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

        private CommodityRespoitory commodityRespoitory = new CommodityRespoitory();
        private ProductRespoitory productRespoitory = new ProductRespoitory();
        //進貨單
        public ActionResult 進貨單建立()
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                StockCreateViewModel model = new StockCreateViewModel();
                StockDetailViewModel detail = new StockDetailViewModel();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                model.sEmployeeId = emp.EmployeeId;
                var productNames = productRespoitory.GetNameAll();
                List<SelectListItem> productlist = productRespoitory.GetPositionName(productNames);
                var warehouseNames = commodityRespoitory.GetWarehouseAll();
                List<SelectListItem> warehouselist = commodityRespoitory.GetSelectList(warehouseNames);
                detail.WareHouseNames = warehouselist;
                detail.ProductNames = productlist;
                DateTime today = DateTime.Now.Date;
                var count = dbContext.tStockLists.Where(s => s.sStockDate == today).ToList().Count;
                count++;
                model.sStockSerialValue = Convert.ToInt32(DateTime.Now.ToString("yyMMdd")+count.ToString("0000"));
                model.StockDetail = detail;
                model.sStockDate = DateTime.Now;
                return View(model);
            }
            else
            {
                return RedirectToAction("員工登入", "Employee");
            }
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
                        //倉儲變動
                        tWarehouse Warehouse = dbContext.tWarehouses.Where(w => w.WarehouseNameId.Equals(items.sWarehouseNameId) && w.wProductId.Equals(items.sProductId)).FirstOrDefault();
                        if(Warehouse != null)
                        {
                            Warehouse.wPQty += items.sQuantity;
                        }
                        else
                        {
                            tWarehouse warehouse = new tWarehouse();
                            warehouse.WarehouseNameId = items.sWarehouseNameId;
                            warehouse.wProductId = items.sProductId;
                            warehouse.wPQty = items.sQuantity;
                            dbContext.tWarehouses.Add(warehouse);
                        }
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
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tStockList> tStockLists = dbContext.tStockLists.ToList();
            List<StockSelectViewModel> stockSelects = new List<StockSelectViewModel>();
            foreach(var item in tStockLists)
            {
                List<tStockDetail> tStockDetails = dbContext.tStockDetails.Where(sd => sd.sStockId.Equals(item.StockId)).ToList();
                StockSelectViewModel select = new StockSelectViewModel();
                List<StockSelectDetailModel> details = new List<StockSelectDetailModel>();
                foreach(var itemdetail in tStockDetails)
                {
                    tProduct product = dbContext.tProducts.Where(p => p.ProductId.Equals(itemdetail.sProductId)).FirstOrDefault();
                    tWarehouseName warehouseName = dbContext.tWarehouseNames.Where(w => w.WarehouseNameId.Equals(itemdetail.sWarehouseNameId)).FirstOrDefault();
                    StockSelectDetailModel selectDetail = new StockSelectDetailModel();
                    selectDetail.ProductNum = product.pNumber;
                    selectDetail.ProductName = product.pName;
                    selectDetail.sCost = itemdetail.sCost;
                    selectDetail.sQuantity = itemdetail.sQuantity;
                    selectDetail.WareHouseName = warehouseName.WarehouseName;
                    selectDetail.sNote = itemdetail.sNote;
                    details.Add(selectDetail);
                }
                tEmployee employee = dbContext.tEmployees.Where(e => e.EmployeeId.Equals(item.sEmployeeId)).FirstOrDefault();
                select.EmployeeName = employee.eName;
                select.sStockSerialValue = item.sStockSerialValue;
                select.sVendor = item.sVendor;
                select.sVendorTel = item.sVendorTel;
                select.sStockDate = item.sStockDate;
                select.sStockNote = item.sStockNote;
                select.StockDetails = details;
                stockSelects.Add(select);
            }
            return View(stockSelects);
        }

        //出貨單
        public ActionResult 出貨單建立()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                ShipCreateShowViewModel model = new ShipCreateShowViewModel();
                DateTime today = DateTime.Now.Date;
                var count = dbContext.tShipLists.Where(s => s.sShipDate == today).ToList().Count;
                count++;
                model.sShipSerialValue = Convert.ToInt32(DateTime.Now.ToString("yyMMdd") + count.ToString("0000"));
                model.sShipDate = DateTime.Now;
                List<tOrder> orderSearch = dbContext.tOrders.Where(o => o.oCheck != null && o.cNote != "已出貨").ToList();
                List<OrderShipShowViewModel> orderShips = new List<OrderShipShowViewModel>();
                foreach (var item in orderSearch)
                {
                    OrderShipShowViewModel orderShip = new OrderShipShowViewModel();
                    orderShip.OrderId = item.OrderId;
                    orderShip.oAddress = item.oAddress;
                    orderShip.oCheck = item.oCheck;
                    orderShip.oCheckDate = item.oCheckDate;
                    orderShip.cNote = item.cNote;
                    orderShip.oDate = item.oDate;
                    orderShips.Add(orderShip);
                }
                model.ShipShows = orderShips;
                return View(model);
            }
            return RedirectToAction("員工登入", "Employee");
        }
        public JsonResult showOrderDetail(int OrderId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tOrderDetail> orderDetailSearchs = dbContext.tOrderDetails.Where(od => od.oOrderId.Equals(OrderId)).ToList();
            List<OrderDetailShipShowViewModel> orderDetails = new List<OrderDetailShipShowViewModel>();
            foreach(var item in orderDetailSearchs)
            {
                tProduct product = dbContext.tProducts.Where(p => p.ProductId.Equals(item.oProductId)).FirstOrDefault();
                OrderDetailShipShowViewModel orderdetail = new OrderDetailShipShowViewModel();
                orderdetail.ProductNum = product.pNumber;
                orderdetail.ProductName = product.pName;
                orderdetail.oProductQty = item.oProductQty;
                orderdetail.oNote = item.oNote;
                orderDetails.Add(orderdetail);
            }
            return Json(orderDetails);
        }
        public JsonResult chooseShipWare(int OrderId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tOrderDetail> orderDetailSearchs = dbContext.tOrderDetails.Where(od => od.oOrderId.Equals(OrderId)).ToList();
            List<WareShipChooseViewModel> ChooseList = new List<WareShipChooseViewModel>();
            foreach(var item in orderDetailSearchs)
            {
                List<tWarehouse> wareList = dbContext.tWarehouses.Where(w => w.wProductId.Equals(item.oProductId)).ToList();
                foreach(var itemware in wareList)
                {
                    tProduct product = dbContext.tProducts.Where(p => p.ProductId.Equals(item.oProductId)).FirstOrDefault();
                    WareShipChooseViewModel wareChoose = new WareShipChooseViewModel();
                    wareChoose.WareHouseName = dbContext.tWarehouseNames.Where(wn => wn.WarehouseNameId.Equals(itemware.WarehouseNameId)).FirstOrDefault().WarehouseName;
                    wareChoose.WareHouseId = itemware.WarehouseNameId;
                    wareChoose.OrderDetailId = item.oOrderDetailId;
                    wareChoose.ProductId = product.ProductId;
                    wareChoose.ProductNum = product.pNumber;
                    wareChoose.ProductName = product.pName;
                    wareChoose.ProductQty = itemware.wPQty;
                    ChooseList.Add(wareChoose);
                }
            }
            return Json(ChooseList);
        }
        [HttpPost]
        public ActionResult 出貨單建立(ShipCreateModel ShipList)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee employee = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                tShipList Shiplist = new tShipList();
                Shiplist.sEmployeeId = employee.EmployeeId;
                Shiplist.sShipSerialValue = ShipList.sShipSerialValue;
                Shiplist.sOrderId = ShipList.SelectOrder;
                Shiplist.sShipDate = ShipList.sShipDate;
                Shiplist.sShipNote = ShipList.sShipNote;
                dbContext.tShipLists.Add(Shiplist);
                dbContext.SaveChanges();
                for(int i = 0; i < ShipList.WareHouseId.Count; i++)
                {
                    if(ShipList.ShipProductQty[i] != 0)
                    {
                        tShipDetail tShipdetail = new tShipDetail();
                        tShipdetail.ShipId = dbContext.tShipLists.OrderByDescending(s => s.ShipId).First().ShipId;
                        tShipdetail.sOrderDetailId = ShipList.OrderDetailId[i];
                        tShipdetail.sProductId = ShipList.ProductId[i];
                        tShipdetail.sQuantity = ShipList.ShipProductQty[i];
                        tShipdetail.sWarehouseNameId = ShipList.WareHouseId[i];
                        dbContext.tShipDetails.Add(tShipdetail);
                        //倉儲變動
                        tWarehouse Warehouse = dbContext.tWarehouses.Where(w => w.WarehouseNameId == tShipdetail.sWarehouseNameId && w.wProductId == tShipdetail.sProductId).FirstOrDefault();
                        if (Warehouse != null)
                        {
                            Warehouse.wPQty -= ShipList.ShipProductQty[i];
                        }
                    }
                }
                tOrder order = dbContext.tOrders.Where(o => o.OrderId == Shiplist.sOrderId).FirstOrDefault();
                order.cNote = "已出貨";
                dbContext.SaveChanges();
            }
            return RedirectToAction("出貨單查詢");
        }
        public ActionResult 出貨單查詢()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tShipList> tShipLists = dbContext.tShipLists.ToList();
            List<ShipSelectViewModel> shipSelects = new List<ShipSelectViewModel>();
            foreach (var item in tShipLists)
            {
                List<tShipDetail> tShipDetails = dbContext.tShipDetails.Where(sd => sd.ShipId.Equals(item.ShipId)).ToList();
                ShipSelectViewModel select = new ShipSelectViewModel();
                List<ShipSelectDetailModel> details = new List<ShipSelectDetailModel>();
                foreach (var itemdetail in tShipDetails)
                {
                    tProduct product = dbContext.tProducts.Where(p => p.ProductId.Equals(itemdetail.sProductId)).FirstOrDefault();
                    tWarehouseName warehouseName = dbContext.tWarehouseNames.Where(w => w.WarehouseNameId.Equals(itemdetail.sWarehouseNameId)).FirstOrDefault();
                    tOrderDetail orderd = dbContext.tOrderDetails.Where(o => o.oOrderDetailId.Equals(itemdetail.sOrderDetailId)).FirstOrDefault();
                    ShipSelectDetailModel selectDetail = new ShipSelectDetailModel();
                    selectDetail.ProductNum = product.pNumber;
                    selectDetail.ProductName = product.pName;
                    selectDetail.OrderId = orderd.oOrderId;
                    selectDetail.sQuantity = itemdetail.sQuantity;
                    selectDetail.WareHouseName = warehouseName.WarehouseName;
                    details.Add(selectDetail);
                }
                tEmployee employee = dbContext.tEmployees.Where(e => e.EmployeeId.Equals(item.sEmployeeId)).FirstOrDefault();
                select.EmployeeName = employee.eName;
                select.ShipSerialValue = item.sShipSerialValue;
                select.ShipDate = item.sShipDate;
                select.ShipNote = item.sShipNote;
                select.ShipDetails = details;
                shipSelects.Add(select);
            }
            return View(shipSelects);
        }
        //調貨單
        public ActionResult 調貨單建立()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }

            //todo:調貨單介面
            TransferViewModel model = new TransferViewModel();
            var dicProduct = productRespotiory.GetProductNameAll();
            model.ProductNames = productRespoitory.GetSelectList(dicProduct);
            var dicWarehouse = commodityRespoitory.GetWarehouseAll();
            model.WareHouseInNames = commodityRespoitory.GetSelectList(dicWarehouse);
            model.Date = DateTime.Now;
            model.ProductQty = 1;
            return View(model);
        }
        public JsonResult WareOutSearch(int ProductId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tWarehouse> warehouseOut = dbContext.tWarehouses.Where(w => w.wProductId==ProductId && w.wPQty != 0).ToList();
            List<WarehouseOutList> outLists = new List<WarehouseOutList>();
            foreach (var item in warehouseOut)
            {
                tWarehouseName name = dbContext.tWarehouseNames.Where(wn => wn.WarehouseNameId.Equals(item.WarehouseNameId)).FirstOrDefault();
                WarehouseOutList outList = new WarehouseOutList();
                outList.WarehouseIdOut = item.WarehouseNameId;
                outList.WarehouseNameOut = name.WarehouseName;
                outLists.Add(outList);
            }
            return Json(outLists);
        }
        public JsonResult WareOutInventory(int WarehouseIdOut,int ProductId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tWarehouse warehouse = dbContext.tWarehouses.Where(w => w.wProductId.Equals(ProductId) && w.WarehouseNameId.Equals(WarehouseIdOut)).FirstOrDefault();
            int MaxQty = warehouse.wPQty;
            return Json(MaxQty);
        }
        [HttpPost]
        public ActionResult 調貨單建立(TransferSaveModel save)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee employee = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                tTransfer transfer = new tTransfer();
                transfer.tProductId = save.ProductId;
                transfer.tProductQty = save.ProductQty;
                transfer.tEmployeeId = employee.EmployeeId;
                transfer.tWNIdOut = save.WarehouseIdOut;
                transfer.tWNIdIn = save.WarehouseIdIn;
                transfer.tDate = save.Date;
                transfer.tNote = save.Note;
                tWarehouse warehouseOut = dbContext.tWarehouses.Where(wo => wo.WarehouseNameId.Equals(save.WarehouseIdOut) && wo.wProductId.Equals(save.ProductId)).FirstOrDefault();
                warehouseOut.wPQty = warehouseOut.wPQty - save.ProductQty;
                tWarehouse warehouseIn = dbContext.tWarehouses.Where(wo => wo.WarehouseNameId.Equals(save.WarehouseIdIn) && wo.wProductId.Equals(save.ProductId)).FirstOrDefault();
                if(warehouseIn == null)
                {
                    tWarehouse newwarehouseIn = new tWarehouse();
                    newwarehouseIn.WarehouseNameId = save.WarehouseIdIn;
                    newwarehouseIn.wProductId = save.ProductId;
                    newwarehouseIn.wPQty = save.ProductQty;
                    dbContext.tWarehouses.Add(newwarehouseIn);
                }
                else
                {
                    warehouseIn.wPQty = warehouseIn.wPQty + save.ProductQty;
                }
                dbContext.tTransfers.Add(transfer);
                dbContext.SaveChanges();
                return RedirectToAction("調貨單查詢","Commodity");
            }
            return RedirectToAction("員工登入","Employee");
        }
        public ActionResult 調貨單查詢()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<TransferSearchViewModel> model = new List<TransferSearchViewModel>();
            List<tTransfer> transfers = dbContext.tTransfers.ToList();
            foreach (var item in transfers)
            {
                TransferSearchViewModel tItem = new TransferSearchViewModel();
                tProduct product = dbContext.tProducts.Where(p => p.ProductId.Equals(item.tProductId)).FirstOrDefault();
                tItem.ProductName = product.pName;
                tItem.ProductQty = item.tProductQty;
                tItem.WareHouseOutName = dbContext.tWarehouseNames.Where(wo => wo.WarehouseNameId.Equals(item.tWNIdOut)).FirstOrDefault().WarehouseName;
                tItem.WareHouseInName = dbContext.tWarehouseNames.Where(wi => wi.WarehouseNameId.Equals(item.tWNIdIn)).FirstOrDefault().WarehouseName;
                tItem.Date = item.tDate;
                tItem.tNote = item.tNote;
                model.Add(tItem);
            }
            return View(model);
        }
        //倉儲
        public ActionResult 倉儲查詢()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            else
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                CommodityViewModel model = new CommodityViewModel();
                List<tWarehouse> Warehouses = dbContext.tWarehouses.OrderBy(w => w.WarehouseNameId).ToList();
                List<WareInventorySelectViewModel> InventoryList = new List<WareInventorySelectViewModel>();
                List<WarningQuantityViewModel> WarningList = new List<WarningQuantityViewModel>();
                foreach (var item in Warehouses)
                {
                    WareInventorySelectViewModel wareInventory = new WareInventorySelectViewModel();
                    wareInventory.WarehouseName = dbContext.tWarehouseNames.Where(wn => wn.WarehouseNameId.Equals(item.WarehouseNameId)).FirstOrDefault().WarehouseName;
                    wareInventory.ProductName = dbContext.tProducts.Where(pn => pn.ProductId.Equals(item.wProductId)).FirstOrDefault().pName;
                    wareInventory.ProductQty = item.wPQty;
                    InventoryList.Add(wareInventory);
                }
                var productInventories = dbContext.tWarehouses.GroupBy(p => p.wProductId,p=>p.wPQty);
                foreach(var warnitem in productInventories)
                {
                    WarningQuantityViewModel warningQuantity = new WarningQuantityViewModel();
                    tProduct product = dbContext.tProducts.Where(p => p.ProductId == warnitem.Key).FirstOrDefault();
                    int ProductQty = 0;
                    foreach(var qty in warnitem)
                    {
                        ProductQty += qty;
                    }
                    if (ProductQty < 3)
                    {
                        warningQuantity.ProductName = product.pName;
                        warningQuantity.underStock = ProductQty;
                        WarningList.Add(warningQuantity);
                    }
                }
                model.InventorySelect = InventoryList;
                model.InventoryWaring = WarningList;
                return View(model);
            }
        }
        
        
      
    }
}