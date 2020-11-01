using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        //韋宏訂單
        public ActionResult 詳細訂單(int id)
        {
            MotaiDataEntities db = new MotaiDataEntities();
            var orderDetails = db.tOrderDetails.Where(o => o.oOrderId == id);
            List<OrderDetailViewModel> orderdetail = new List<OrderDetailViewModel>();
            List<OrderViewModel> OrderList = new List<OrderViewModel>();
            if (orderdetail.FirstOrDefault() != null)
            {
                return View(OrderList);

            }
            return View("首頁");
        }



        //客戶訂單
        //public ActionResult 客戶訂單()
        //{
        //    if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
        //    {
        //        tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
        //        MotaiDataEntities db = new MotaiDataEntities();
        //        List<tOrder> orderlist = db.tOrders.Where(o => o.oCustomerId.Equals(cust.CustomerId)).ToList();
        //        List<OrderViewModel> OrderList = new List<OrderViewModel>();
        //        foreach (var items in orderlist)
        //        {
        //            OrderViewModel order = new OrderViewModel();
        //            order.Order = items;
        //            OrderList.Add(order);
        //            db.SaveChanges();
        //        }
        //        return View(OrderList);
        //    }
        //    return RedirectToAction("首頁");

        //}
    }
}