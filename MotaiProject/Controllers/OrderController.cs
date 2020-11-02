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
                EmployeeOrderDetailViewModel detail = new EmployeeOrderDetailViewModel();
                model.oEmployeeId = emp.EmployeeId;
                var warehouseNames = commodityRespoitory.GetWarehouseAll();
                List<SelectListItem> warehouselist = commodityRespoitory.GetSelectList(warehouseNames);
                var productNames = productRespoitory.GetNameAll();
                List<SelectListItem> productlist = commodityRespoitory.GetSelectList(productNames);
                model.WareHouseNames = warehouselist;
                detail.ProductNames = productlist;
                model.empOrderDetail = detail;
                return View(model);
            }
            else
            {
                return RedirectToAction("員工登入", "Employee");
            }                
        }
        [HttpPost]
        public JsonResult 實體店新增訂單(EmployeeOrderViewModel empOrder)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                if (Session[CSession關鍵字.SK_ORDERDETAIL] == null)
                {
                    return Json(new { result = false, msg = "訂單尚未完成!", url = "" });
                }
                else
                {
                    List<EmployeeOrderDetailViewModel> orders = Session[CSession關鍵字.SK_ORDERDETAIL] as List<EmployeeOrderDetailViewModel>;
                    MotaiDataEntities dbContext = new MotaiDataEntities();
                    tOrder list = new tOrder();
                    list.oEmployeeId = emp.EmployeeId;
                    list.oCustomerId = dbContext.tCustomers.Where(c=>c.cCellPhone.Equals(empOrder.cCellphone)).FirstOrDefault().CustomerId;
                    list.oAddress = empOrder.oAddress;
                    list.oDate = empOrder.oDate;
                    list.oPromotionId = empOrder.oPromotionId;
                    list.cNote = empOrder.cNote;
                    dbContext.tOrders.Add(list);
                    dbContext.SaveChanges();
                    foreach (var items in orders)
                    {
                        tOrderDetail detail = new tOrderDetail();
                        detail.oOrderId = dbContext.tOrderDetails.OrderByDescending(i => i.oOrderId).First().oOrderId;
                        detail.oProductId = items.oProductId;
                        detail.oProductQty = items.oProductQty;
                        detail.oNote = items.oNote;
                        dbContext.tOrderDetails.Add(detail);
                    }
                    dbContext.SaveChanges();
                    Session[CSession關鍵字.SK_STOCKDETAIL] = null;
                    return Json(new { result = true, msg = "新增成功", url = Url.Action("實體店新增訂單", "Order") });
                }
            }
            else
            {
                return Json(new { result = false, msg = "尚未登入!", url = Url.Action("員工登入", "Employee") });
            }
        }
        [HttpPost]
        public string createOrderDetail(EmployeeOrderDetailViewModel orderDetail)
        {
            if (Session[CSession關鍵字.SK_ORDERDETAIL] == null)
            {
                List<EmployeeOrderDetailViewModel> orders = new List<EmployeeOrderDetailViewModel>();
                orders.Add(orderDetail);
                Session[CSession關鍵字.SK_ORDERDETAIL] = orders;
            }
            else
            {
                List<EmployeeOrderDetailViewModel> orders = Session[CSession關鍵字.SK_ORDERDETAIL] as List<EmployeeOrderDetailViewModel>;
                orders.Add(orderDetail);
                Session[CSession關鍵字.SK_ORDERDETAIL] = orders;
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            orderDetail.ProductName = dbContext.tProducts.Where(s => s.ProductId.Equals(orderDetail.oProductId)).FirstOrDefault().pName;            

            string data = "<tr><td scope='row'>";
            if (orderDetail.oNote != null)
            {
                data += orderDetail.ProductName.ToString() + "</td><td>";
                data += orderDetail.ProductNum.ToString() + "</td><td>";
                data += orderDetail.oProductQty.ToString() + "</td><td>";
                if (orderDetail.oNote.Length > 10)
                {
                    for (int i = 0; i < orderDetail.oNote.Length / 10; i++)
                    {
                        data += orderDetail.oNote.Substring(i * 10, 10) + "<br>";
                    }
                    data += "</td>";
                }
                else
                {
                    data += orderDetail.oNote.ToString() + "</td>";
                }
            }
            else
            {
                data += orderDetail.ProductName.ToString() + "</td><td>";
                data += orderDetail.ProductNum.ToString() + "</td><td>";
                data += orderDetail.oProductQty.ToString() + "</td><td>";
                data += "</td>";
            }
            return data;
        }
        //刪除Json deleteDetail
        [HttpPost]
        public JsonResult deleteOrderDetail(int index)
        {
            List<EmployeeOrderDetailViewModel> orders = Session[CSession關鍵字.SK_ORDERDETAIL] as List<EmployeeOrderDetailViewModel>;
            orders.RemoveAt(index);
            Session[CSession關鍵字.SK_ORDERDETAIL] = orders;
            return Json(new { msg = "已刪除" });
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