using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class CommodityController : Controller
    {
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