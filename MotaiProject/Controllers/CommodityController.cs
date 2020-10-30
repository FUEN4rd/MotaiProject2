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
        public JsonResult 進貨單建立(StockListViewModel stockList)
        {
            return Json(new { });
        }
        [HttpPost]
        public string createStockDetail(StockDetailViewModel stockDetail)
        {
            string data="";
            return data;
        }
    }
}