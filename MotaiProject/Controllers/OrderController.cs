using AllPay.Payment.Integration;
using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class OrderController : Controller
    {
        // GET: Order
        private CommodityRespoitory commodityRespoitory = new CommodityRespoitory();
        private ProductRespoitory productRespoitory = new ProductRespoitory();
        private OrderRespoitory orderRespoitory = new OrderRespoitory();
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
                    list.oCustomerId = dbContext.tCustomers.Where(c => c.cCellPhone.Equals(empOrder.cCellphone)).FirstOrDefault().CustomerId;
                    list.oAddress = empOrder.oAddress;
                    list.oDate = empOrder.oDate;
                    list.oPromotionId = empOrder.oPromotionId;
                    list.cNote = empOrder.cNote;
                    list.oWarehouseName = empOrder.oWarehouseName;
                    dbContext.tOrders.Add(list);
                    dbContext.SaveChanges();
                    foreach (var items in orders)
                    {
                        tOrderDetail detail = new tOrderDetail();
                        detail.oOrderId = dbContext.tOrders.OrderByDescending(i => i.OrderId).First().OrderId;
                        detail.oProductId = items.oProductId;
                        detail.oProductQty = items.oProductQty;
                        detail.oNote = items.oNote;
                        dbContext.tOrderDetails.Add(detail);
                    }
                    dbContext.SaveChanges();
                    Session[CSession關鍵字.SK_STOCKDETAIL] = null;
                    int OrderId = dbContext.tOrders.OrderByDescending(i => i.OrderId).First().OrderId;

                    //JsonResult json = new JsonResult();
                    //json.ContentType = "text/html";
                    //json.Data =  new { result = true, msg = "新增成功", url = Url.Action("realcheckview", "Order", new { OrderId }) };
                    //return json;


                    return Json(new { result = true, msg = "新增成功", url = Url.Action("realcheckview", "Order",new { OrderId }) });

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
        //實體結帳畫面
        public ActionResult realcheckview()
        {
            //抓訂單資料再選取未結帳完的訂單去結帳
            return View();
        }


        [HttpPost]
        public ActionResult realcheckview(int OrderId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            EmployeeCheckoutViewModel model = new EmployeeCheckoutViewModel();
            EmployeeOrderViewModel Order = new EmployeeOrderViewModel();
            tOrder order = dbContext.tOrders.Where(o => o.OrderId.Equals(OrderId)).FirstOrDefault();
            Order.oCustomerId = (int)order.oCustomerId;
            Order.oAddress = order.oAddress;
            Order.oDate = order.oDate;
            Order.cNote = order.cNote;
            List<tOrderDetail> orderDetails = dbContext.tOrderDetails.Where(od => od.oOrderId.Equals(OrderId)).ToList();
            List<EmployeeOrderDetailViewModel> Orderdetails = new List<EmployeeOrderDetailViewModel>();
            foreach (var itemdetails in orderDetails)
            {
                tProduct product = dbContext.tProducts.Where(p => p.ProductId.Equals(itemdetails.oProductId)).FirstOrDefault();
                EmployeeOrderDetailViewModel Orderdetail = new EmployeeOrderDetailViewModel();
                Orderdetail.oOrderId = OrderId;
                Orderdetail.ProductName = product.pName;
                Orderdetail.ProductNum = product.pNumber;
                Orderdetail.oProductQty = itemdetails.oProductQty;
                model.TotalAmount += itemdetails.oProductQty * Convert.ToInt32(product.pPrice);
                Orderdetails.Add(Orderdetail);
            }
            int promotionId = orderRespoitory.SelectPromotionId(model.TotalAmount, order.oDate);
            if (promotionId != 0)
            {
                Order.oPromotionId = promotionId;
            }
            model.Order = Order;
            model.orderDetails = Orderdetails;

            return View(model);
        }
        [HttpPost]
        public JsonResult OrderPay(int payType, int OrderId, int payMoney)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tOrderPay pay = dbContext.tOrderPays.OrderByDescending(op => op.oOrderInstallment).Where(op => op.oOrderId.Equals(OrderId)).FirstOrDefault();
            if (pay == null)
            {
                tOrderPay orderPay = new tOrderPay();
                orderPay.oOrderId = OrderId;
                orderPay.oOrderInstallment = 1;
                orderPay.oPayType = payType;
                orderPay.oPayment = payMoney;
                orderPay.oPayDate = DateTime.Now;
                dbContext.tOrderPays.Add(orderPay);
                dbContext.SaveChanges();
            }
            else
            {
                tOrderPay orderPay = new tOrderPay();
                orderPay.oOrderId = OrderId;
                orderPay.oOrderInstallment = pay.oOrderInstallment++;
                orderPay.oPayType = payType;
                orderPay.oPayment = payMoney;
                orderPay.oPayDate = DateTime.Now;
                dbContext.tOrderPays.Add(orderPay);
                dbContext.SaveChanges();
            }
            return Json(new { msg = "結帳完成", url = Url.Action("實體店新增訂單", "Order") });
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
                    foreach (var itemDetail in orderdetails)
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
        //網購寫入訂單
        public ActionResult ordernotice()
        {
            return View();
        }
        public ActionResult webOrder(WebPay payData)
        {
            //HttpClient remote = new HttpClient();
            
            string szHtml = String.Empty;
            List<string> enErrors = new List<string>();
            try
            {
                string OrderId = "0001";
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = AllPay.Payment.Integration.HttpMethod.ServerPOST;
                    oPayment.ServiceURL = "https://payment-stage.opay.tw/Cashier/AioCheckOut/V5";
                    //oPayment.ServiceURL = "http://localhost" + Url.Action("購物車清單", "Customer"); ;
                    oPayment.HashKey = "5294y06JbISpM5x9";
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";
                    oPayment.MerchantID = "2000132";
                    //oPayment.MerchantID = System.Configuration.ConfigurationManager.AppSettings["MerchantID"];
                    /* 基本參數 */
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority;
                    oPayment.Send.ReturnURL = baseURI + Url.Action("ordernotice", "Order");
                    oPayment.Send.ClientBackURL = baseURI;
                    oPayment.Send.OrderResultURL = "<<您要收到付款完成通知的瀏覽器端網址>>";
                    string[] trade = Guid.NewGuid().ToString().Split('-');
                    string tradeno="";
                    foreach(var items in trade)
                    {
                        tradeno += items;
                    }
                    //oPayment.Send.MerchantTradeNo = OrderId + tradeno.Substring(0,16);
                    oPayment.Send.MerchantTradeNo = "dfafdasdfafdd";
                    //oPayment.Send.MerchantTradeDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    oPayment.Send.MerchantTradeDate = DateTime.Now;
                    //oPayment.Send.TotalAmount = Decimal.Parse(payData.totalPay);
                    oPayment.Send.TotalAmount = Decimal.Parse("10000");
                    oPayment.Send.TradeDesc = "感謝購買墨台商品";
                    //if(payData.payType == 1)
                    //{
                    //    oPayment.Send.ChoosePayment = PaymentMethod.ATM;
                    //    oPayment.SendExtend.ExpireDate = Int32.Parse("3");
                    //}
                    //else
                    //{
                    //    oPayment.Send.ChoosePayment = PaymentMethod.Credit;
                    //}
                    oPayment.Send.ChoosePayment = PaymentMethod.ALL;
                    //oPayment.Send.PaymentType;
                    oPayment.Send.Remark = "饒了我吧";
                    oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;
                    oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.No;
                    oPayment.Send.HoldTrade = HoldTradeType.No;
                    oPayment.Send.DeviceSource = DeviceType.PC;
                    oPayment.Send.UseRedeem = UseRedeemFlag.No; //購物金/紅包折抵
                    oPayment.Send.IgnorePayment = ""; // 例如財付通:Tenpay
                    // 加入選購商品資料。
                    oPayment.Send.Items.Add(new Item()
                    {
                        Name = "哈",
                        Price = Decimal.Parse("100"),
                        Currency = "NTD",
                        Quantity = int.Parse("9"),
                        URL = "<< 產品說明位址 >>"
                    });
                    //foreach (var item in payData.Items)
                    //{
                    //    oPayment.Send.Items.Add(new Item()
                    //    {
                    //        Name = item.Name,
                    //        Price = Decimal.Parse(item.Price),
                    //        Currency = "NTD",
                    //        Quantity = int.Parse(item.Quantity),
                    //        URL = "<< 產品說明位址 >>"
                    //    });
                    //}
                    // 當付款方式為 ALL 時，建議增加的參數。
                    oPayment.SendExtend.PaymentInfoURL = "<<您要接收回傳自動櫃員機/超商/條碼付款相關資訊的網址。>> ";
                    /* 產生訂單 */
                    enErrors.AddRange(oPayment.CheckOut());
                    /* 產生產生訂單 Html Code 的方法 */
                    //string szHtml = String.Empty;
                    enErrors.AddRange(oPayment.CheckOutString(ref szHtml));
                    //string response = remote.UploadString("https://payment-stage.opay.tw/Cashier/AioCheckOut/V5", Json(oPayment).ToString());
                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    string szErrorMessage = String.Join("\\r\\n", enErrors);
                }
            }
            ViewBag.AllPayRedirect = szHtml;
            return View();
            //var request = new HttpRequestMessage();
            //request.Content = new StringContent(szHtml);
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            //return request;
        }

        public HttpResponseMessage PostComplex()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            return response;
        }
    }
}