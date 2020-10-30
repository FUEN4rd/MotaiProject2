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

        public ActionResult 進貨單建立()
        {
            StockCreateViewModel model = new StockCreateViewModel();
            return View();
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