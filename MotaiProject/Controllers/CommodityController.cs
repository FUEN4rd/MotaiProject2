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
        private CommodityRespoitory CommodityRespoitory = new CommodityRespoitory();
        private ProductRespoitory ProductRespoitory = new ProductRespoitory();
        public ActionResult 進貨單建立()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                StockCreateViewModel model = new StockCreateViewModel();
                StockDetailViewModel detail = new StockDetailViewModel();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                model.sEmployeeId = emp.EmployeeId;
                var productNames = ProductRespoitory.GetNameAll();
                List<SelectListItem> productlist = ProductRespoitory.GetSelectList(productNames);
                var warehouseNames = CommodityRespoitory.GetWarehouseAll();
                List<SelectListItem> warehouselist = CommodityRespoitory.GetSelectList(warehouseNames);
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
            return Json(new { });
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
    }
}