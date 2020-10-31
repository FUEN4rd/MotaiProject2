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
        private CommodityRespoitory commodityRespoitory = new CommodityRespoitory();        
        private ProductRespoitory productRespoitory = new ProductRespoitory();
        public ActionResult 實體店新增訂單()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                EmployeeOrderViewModel model = new EmployeeOrderViewModel();
                model.oEmployeeId = emp.EmployeeId;
                var warehouseNames = commodityRespoitory.GetWarehouseAll();
                List<SelectListItem> warehouselist = commodityRespoitory.GetSelectList(warehouseNames);
                model.WareHouseNames = warehouselist;
                return View(model);
            }
            else
            {
                return RedirectToAction("員工登入", "Employee");
            }                
        }
        public JsonResult 實體店新增訂單(EmployeeOrderViewModel empOrder)
        {
            return Json(new { });
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
        public ActionResult 客戶看訂單()
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;
                MotaiDataEntities db = new MotaiDataEntities();
                List<tOrder> orders = db.tOrders.Where(o => o.oCustomerId.Equals(cust.CustomerId)).ToList();
                List<CustomerOrderViewModel> OrderList = new List<CustomerOrderViewModel>();
                foreach (var items in orders)
                {
                    CustomerOrderViewModel order = new CustomerOrderViewModel();
                    order.oDate = items.oDate;
                    order.WarehouseName = db.tWarehouseNames.Where(w => w.WarehouseNameId.Equals(items.oWarehouseName)).FirstOrDefault().WarehouseName;
                    order.EmployeeName = db.tEmployees.Where(e => e.EmployeeId.Equals(items.oEmployeeId)).FirstOrDefault().eName;
                    order.cNote = items.cNote;
                    List<tOrderDetail> orderdetails = db.tOrderDetails.Where(od => od.oOrderId.Equals(items.OrderId)).ToList();
                    List<CustomerOrderDetailViewModel> OrderDetailList = new List<CustomerOrderDetailViewModel>();
                    foreach(var itemDetail in orderdetails)
                    {
                        CustomerOrderDetailViewModel orderdetail = new CustomerOrderDetailViewModel();
                        orderdetail.ProductNum = db.tProducts.Where(p => p.ProductId.Equals(itemDetail.oProductId)).FirstOrDefault().pNumber;
                        orderdetail.ProductName = db.tProducts.Where(p => p.ProductId.Equals(itemDetail.oProductId)).FirstOrDefault().pName;
                        orderdetail.oProductQty = itemDetail.oProductQty;
                        orderdetail.oNote = itemDetail.oNote;
                        OrderDetailList.Add(orderdetail);
                    }
                    order.CustomerOrderDetails = OrderDetailList;
                    OrderList.Add(order);                    
                }
                return View(OrderList);
            }
            return RedirectToAction("首頁");
        }
    }
}