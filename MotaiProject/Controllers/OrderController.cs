using AllPay.Payment.Integration;
using MotaiProject.Models;
using MotaiProject.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
                model.oDate = DateTime.Now.Date;
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
                    return Json(new { result = false, msg = "訂單尚未完成!", url = Url.Action("實體店新增訂單","Order") });
                }
                else
                {
                    List<EmployeeOrderDetailViewModel> orders = Session[CSession關鍵字.SK_ORDERDETAIL] as List<EmployeeOrderDetailViewModel>;
                    MotaiDataEntities dbContext = new MotaiDataEntities();
                    tCustomer customer = dbContext.tCustomers.Where(c => c.cCellPhone == empOrder.cCellphone).FirstOrDefault();
                    tOrder list = new tOrder();
                    list.oEmployeeId = emp.EmployeeId;
                    list.oCustomerId = customer.CustomerId;
                    if(empOrder.oAddress == "")
                    {
                        list.oAddress = customer.cAddress;
                    }
                    else
                    {
                        list.oAddress = empOrder.oAddress;
                    }
                    list.oDate = empOrder.oDate;
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
                    int orderId = dbContext.tOrders.OrderByDescending(i => i.OrderId).First().OrderId;
                    return Json(new { result = true, msg = "新增成功", url = Url.Action("realCheckView", "Order"), OrderId = orderId });
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
        public ActionResult realCheckView(int OrderId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            EmployeeCheckoutViewModel model = new EmployeeCheckoutViewModel();
            EmployeeOrderViewModel Order = new EmployeeOrderViewModel();
            tOrder order = dbContext.tOrders.Where(o => o.OrderId.Equals(OrderId)).FirstOrDefault();
            Order.CustomerName = dbContext.tCustomers.Where(c=>c.CustomerId==order.oCustomerId).FirstOrDefault().cName;
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
                model.originalPrice += itemdetails.oProductQty * Convert.ToInt32(product.pPrice);
                Orderdetails.Add(Orderdetail);
            }
            int promotionId = orderRespoitory.SelectPromotionId(model.originalPrice, order.oDate);
            if (promotionId != 0)
            {
                tPromotion promotion = dbContext.tPromotions.Where(p => p.PromotionId.Equals(promotionId)).FirstOrDefault();
                Order.PromotionName = promotion.PromotionName;
                Order.PromotionDiscount = promotion.pDiscount;
                Order.PromotionCondition = promotion.pCondition;
                model.TotalAmount = model.originalPrice - Convert.ToInt32(promotion.pDiscount);
            }
            else
            {
                Order.PromotionName = "不適用任何優惠活動";
                Order.PromotionDiscount = "0";
                Order.PromotionCondition = "無";
                model.TotalAmount = model.originalPrice;
            }
            model.AlreadyPay = 0;
            model.Unpaid = model.TotalAmount;
            if(dbContext.tOrderPays.Where(op => op.oOrderId.Equals(OrderId)).ToList() != null)
            {
                List<tOrderPay> pays = dbContext.tOrderPays.Where(op => op.oOrderId.Equals(OrderId)).ToList();
                foreach(var item in pays)
                {
                    model.AlreadyPay += Convert.ToInt32(item.oPayment);
                }
                model.Unpaid = model.TotalAmount - model.AlreadyPay;
            }
            model.Order = Order;
            model.orderDetails = Orderdetails;

            return View(model);
        }
        //分期實體結帳畫面
        public ActionResult 分期結帳畫面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            return View();
        }
        //搜尋客戶訂單
        public JsonResult SearchCustomerOrder(string CustomerCell)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tCustomer cust = dbContext.tCustomers.Where(c => c.cCellPhone.Equals(CustomerCell)).FirstOrDefault();
            if(cust == null)
            {
                return Json(new {result= false , msg="查無此人" });
            }
            else
            {
                int custId = cust.CustomerId;
                List<tOrder> orderlist = dbContext.tOrders.Where(o => o.oCustomerId == custId).ToList();
                List<SearchCustomerOrderModel> searchlist = new List<SearchCustomerOrderModel>();
                foreach(var item in orderlist)
                {
                    if(item.oCheck == null)
                    {
                        SearchCustomerOrderModel search = new SearchCustomerOrderModel();
                        search.orderId = item.OrderId;
                        search.purchaseDate = item.oDate.ToString("yyyy/MM/dd");
                        search.WarehouseName = dbContext.tWarehouseNames.Where(wn => wn.WarehouseNameId==item.oWarehouseName).FirstOrDefault().WarehouseName;
                        searchlist.Add(search);
                    }
                }
                return Json(new { result = true, list = searchlist });
            }
        }
        //顯示訂單詳細內容
        public JsonResult showOrderDetail(int OrderId)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tOrderDetail> orderDetailSearchs = dbContext.tOrderDetails.Where(od => od.oOrderId.Equals(OrderId)).ToList();
            List<OrderDetailShipShowViewModel> orderDetails = new List<OrderDetailShipShowViewModel>();
            foreach (var item in orderDetailSearchs)
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
        //實體結帳動作
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
        //網購接受信用卡訂單
        public HttpResponseBase orderCredit([System.Web.Http.FromBody]string merchantTradeNo)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    oPayment.HashKey = "5294y06JbISpM5x9";
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";
                    /* 取回付款結果 */
                    enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {
                    /* 支付後的回傳的基本參數 */
                    string szMerchantID = String.Empty;
                    string szMerchantTradeNo = String.Empty;
                    string szRtnCode = String.Empty;
                    string szRtnMsg = String.Empty;
                    /* 使用定期定額交易時，回傳的額外參數 */
                    string szPeriodType = String.Empty;
                    string szFrequency = String.Empty;
                    string szExecTimes = String.Empty;
                    string szAmount = String.Empty;
                    string szGwsr = String.Empty;
                    string szProcessDate = String.Empty;
                    string szAuthCode = String.Empty;
                    string szFirstAuthAmount = String.Empty;
                    string szTotalSuccessTimes = String.Empty;
                    // 取得資料
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 使用定期定額交易時回傳的參數 */
                            case "MerchantID": szMerchantID = htFeedback[szKey].ToString(); break;
                            case "MerchantTradeNo":
                                szMerchantTradeNo = htFeedback[szKey].ToString();
                                break;
                            case "RtnCode": szRtnCode = htFeedback[szKey].ToString(); break;
                            case "RtnMsg": szRtnMsg = htFeedback[szKey].ToString(); break;
                            case "PeriodType": szPeriodType = htFeedback[szKey].ToString(); break;
                            case "Frequency": szFrequency = htFeedback[szKey].ToString(); break;
                            case "ExecTimes": szExecTimes = htFeedback[szKey].ToString(); break;
                            case "Amount": szAmount = htFeedback[szKey].ToString(); break;
                            case "Gwsr": szGwsr = htFeedback[szKey].ToString(); break;
                            case "ProcessDate": szProcessDate = htFeedback[szKey].ToString(); break;
                            case "AuthCode": szAuthCode = htFeedback[szKey].ToString(); break;
                            case "FirstAuthAmount":
                                szFirstAuthAmount = htFeedback[szKey].ToString();
                                break;
                            case "TotalSuccessTimes":
                                szTotalSuccessTimes = htFeedback[szKey].ToString();
                                break;
                            default: break;
                        }
                    }
                    
                    WebOrderModel Trade = Session[szMerchantTradeNo] as WebOrderModel;
                    //先建訂單
                    tOrder newOrder = new tOrder();
                    newOrder.oCustomerId = Trade.customer.CustomerId;
                    newOrder.oDate = Trade.payDate;
                    newOrder.oAddress = Trade.webpay.shipAddress;
                    newOrder.oWarehouseName = 1;
                    dbContext.tOrders.Add(newOrder);
                    dbContext.SaveChanges();
                    tOrder CreateOrder = dbContext.tOrders.OrderByDescending(o => o.OrderId).FirstOrDefault();
                    tOrderPay pay = new tOrderPay();
                    pay.oOrderId = CreateOrder.OrderId;
                    pay.oOrderInstallment = 1;
                    pay.oPayType = Trade.webpay.payType;
                    pay.oPayment = Convert.ToInt32(Trade.webpay.totalPay);
                    pay.oPayDate = Trade.payDate;
                    dbContext.tOrderPays.Add(pay);
                    foreach (var item in Trade.boughtList)
                    {
                        tOrderDetail orderDetail = new tOrderDetail();
                        orderDetail.oOrderId = CreateOrder.OrderId;
                        orderDetail.oProductId = item.sProductId;
                        orderDetail.oProductQty = item.sProductQty;
                        dbContext.tOrderDetails.Add(orderDetail);
                        dbContext.tStatus.Remove(item);
                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    // 其他資料處理。
                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                this.Response.Clear();
                // 回覆成功訊息。
                if (enErrors.Count() == 0)
                {
                    this.Response.Write("1|OK");
                }
                // 回覆錯誤訊息。
                else
                {
                    this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
                }
                this.Response.Flush();
                this.Response.End();
            }
            return this.Response;
        }
        //public void orderCredit(FormCollection callback)
        //{
        //    MotaiDataEntities dbContext = new MotaiDataEntities();
        //    List<string> enErrors = new List<string>();
        //    Hashtable htFeedback = null;
        //    try
        //    {
        //        using (AllInOne oPayment = new AllInOne())
        //        {
        //            oPayment.HashKey = "5294y06JbISpM5x9";
        //            oPayment.HashIV = "v77hoKGq4kWxNNIS";
        //            /* 取回付款結果 */
        //            enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
        //        }
        //        // 取回所有資料
        //        if (enErrors.Count() == 0)
        //        {
        //            /* 支付後的回傳的基本參數 */
        //            string szMerchantID = String.Empty;
        //            string szMerchantTradeNo = String.Empty;
        //            string szPaymentDate = String.Empty;
        //            string szPaymentType = String.Empty;
        //            string szPaymentTypeChargeFee = String.Empty;
        //            string szRtnCode = String.Empty;
        //            string szRtnMsg = String.Empty;
        //            string szSimulatePaid = String.Empty;
        //            string szTradeAmt = String.Empty;
        //            string szTradeDate = String.Empty;
        //            string szTradeNo = String.Empty;
        //            // 取得資料
        //            foreach (string szKey in htFeedback.Keys)
        //            {
        //                switch (szKey)
        //                {
        //                    /* 支付後的回傳的基本參數 */
        //                    case "MerchantID": szMerchantID = htFeedback[szKey].ToString();
        //                        break;
        //                    case "MerchantTradeNo":
        //                        szMerchantTradeNo = htFeedback[szKey].ToString();
        //                        break;
        //                    case "PaymentDate": szPaymentDate = htFeedback[szKey].ToString();
        //                        break;
        //                    case "PaymentType": szPaymentType = htFeedback[szKey].ToString();
        //                        break;
        //                    case "PaymentTypeChargeFee":szPaymentTypeChargeFee =htFeedback[szKey].ToString();
        //                        break;
        //                    case "RtnCode": szRtnCode = htFeedback[szKey].ToString();
        //                        break;
        //                    case "RtnMsg": szRtnMsg = htFeedback[szKey].ToString();
        //                        break;
        //                    case "SimulatePaid": szSimulatePaid = htFeedback[szKey].ToString();
        //                        break;
        //                    case "TradeAmt": szTradeAmt = htFeedback[szKey].ToString();
        //                        break;
        //                    case "TradeDate": szTradeDate = htFeedback[szKey].ToString();
        //                        break;
        //                    case "TradeNo": szTradeNo = htFeedback[szKey].ToString();
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //            WebOrderModel Trade = Session[szMerchantTradeNo] as WebOrderModel;
        //            //先建訂單
        //            tOrder newOrder = new tOrder();
        //            newOrder.oCustomerId = Trade.customer.CustomerId;
        //            newOrder.oDate = Trade.payDate;
        //            newOrder.oAddress = Trade.webpay.shipAddress;
        //            newOrder.oWarehouseName = 1;
        //            dbContext.tOrders.Add(newOrder);
        //            dbContext.SaveChanges();
        //            tOrder CreateOrder = dbContext.tOrders.OrderByDescending(o => o.OrderId).FirstOrDefault();
        //            tOrderPay pay = new tOrderPay();
        //            pay.oOrderId = CreateOrder.OrderId;
        //            pay.oOrderInstallment = 1;
        //            pay.oPayType = Trade.webpay.payType;
        //            pay.oPayment = Convert.ToInt32(Trade.webpay.totalPay);
        //            pay.oPayDate = Trade.payDate;
        //            dbContext.tOrderPays.Add(pay);
        //            foreach(var item in Trade.boughtList)
        //            {
        //                tOrderDetail orderDetail = new tOrderDetail();
        //                orderDetail.oOrderId = CreateOrder.OrderId;
        //                orderDetail.oProductId = item.sProductId;
        //                orderDetail.oProductQty = item.sProductQty;
        //                dbContext.tOrderDetails.Add(orderDetail);
        //                dbContext.tStatus.Remove(item);
        //            }
        //            dbContext.SaveChanges();
        //        }
        //        else
        //        {
        //            // 其他資料處理。
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // 例外錯誤處理。
        //        enErrors.Add(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Response.Clear();
        //        // 回覆成功訊息。
        //        if (enErrors.Count() == 0)
        //        {
        //            this.Response.Write("1|OK");
        //        }
        //        // 回覆錯誤訊息。
        //        else
        //        {
        //            this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
        //        }
        //        this.Response.Flush();
        //        this.Response.End();
        //    }
        //}
        //網購傳回ATM訂單
        public void orderATM(string callback)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    oPayment.HashKey = "5294y06JbISpM5x9";
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";
                    /* 取回付款結果 */
                    enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {
                    /* 支付後的回傳的基本參數 */
                    string szMerchantID = String.Empty;
                    string szMerchantTradeNo = String.Empty;
                    string szPaymentType = String.Empty;
                    string szRtnCode = String.Empty;
                    string szRtnMsg = String.Empty;
                    string szTradeAmt = String.Empty;
                    string szTradeDate = String.Empty;
                    string szTradeNo = String.Empty;
                    string szBankCode = String.Empty;
                    string szVirtualAccount = String.Empty;
                    string szExpireDate = String.Empty;
                    // 取得資料
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 支付後的回傳的基本參數 */
                            case "MerchantID": szMerchantID = htFeedback[szKey].ToString(); break;
                            case "MerchantTradeNo":szMerchantTradeNo = htFeedback[szKey].ToString();
                                break;
                            case "RtnCode": szRtnCode = htFeedback[szKey].ToString(); break;
                            case "RtnMsg": szRtnMsg = htFeedback[szKey].ToString(); break;
                            case "TradeNo": szTradeNo = htFeedback[szKey].ToString(); break;
                            case "TradeAmt": szTradeAmt = htFeedback[szKey].ToString(); break;
                            case "PaymentType": szPaymentType = htFeedback[szKey].ToString(); break;
                            case "TradeDate": szTradeDate = htFeedback[szKey].ToString(); break;
                            case "BankCode": szBankCode = htFeedback[szKey].ToString(); break;
                            case "vAccount": szVirtualAccount = htFeedback[szKey].ToString(); break;
                            case "ExpireDate": szExpireDate = htFeedback[szKey].ToString(); break;
                            default:
                                break;
                        }
                    }
                    WebOrderModel Trade = Session[szMerchantTradeNo] as WebOrderModel;
                    //先建訂單
                    tOrder newOrder = new tOrder();
                    newOrder.oCustomerId = Trade.customer.CustomerId;
                    newOrder.oDate = Trade.payDate;
                    newOrder.oAddress = Trade.webpay.shipAddress;
                    newOrder.oWarehouseName = 1;
                    dbContext.tOrders.Add(newOrder);
                    dbContext.SaveChanges();
                    tOrder CreateOrder = dbContext.tOrders.OrderByDescending(o => o.OrderId).FirstOrDefault();
                    foreach (var item in Trade.boughtList)
                    {
                        tOrderDetail orderDetail = new tOrderDetail();
                        orderDetail.oOrderId = CreateOrder.OrderId;
                        orderDetail.oProductId = item.sProductId;
                        orderDetail.oProductQty = item.sProductQty;
                        dbContext.tOrderDetails.Add(orderDetail);
                        dbContext.tStatus.Remove(item);
                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    // 其他資料處理。
                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                this.Response.Clear();
                // 回覆成功訊息。
                if (enErrors.Count() == 0)
                {
                    this.Response.Write("1|OK");
                }
                // 回覆錯誤訊息。
                else
                {
                    this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
                }
                this.Response.Flush();
                this.Response.End();
            }
        }
        //網購接收ATM繳費完成通知
        public void ATMpayOff(string callback)
        {

        }
        //網購寫入訂單
        public string webOrder(WebPay payData)
        {
            if (Session[CSession關鍵字.SK_LOGINED_CUSTOMER] != null)
            {
                tCustomer cust = Session[CSession關鍵字.SK_LOGINED_CUSTOMER] as tCustomer;

                MotaiDataEntities dbContext = new MotaiDataEntities();
                List<tStatu> StatuList = dbContext.tStatus.Where(s => s.sCustomerId.Equals(cust.CustomerId)).ToList();
                string szHtml = String.Empty;
                List<string> enErrors = new List<string>();
                try
                {
                    using (AllInOne oPayment = new AllInOne())
                    {
                        /* 服務參數 */
                        oPayment.ServiceMethod = AllPay.Payment.Integration.HttpMethod.HttpPOST;
                        oPayment.ServiceURL = "https://payment-stage.opay.tw/Cashier/AioCheckOut/V5";
                        oPayment.HashKey = "5294y06JbISpM5x9";
                        oPayment.HashIV = "v77hoKGq4kWxNNIS";
                        oPayment.MerchantID = "2000132";
                        /* 基本參數 */
                        string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority;
                        if(payData.payType == 2)
                        {
                            oPayment.Send.ReturnURL = baseURI + Url.Action("orderCredit", "Order");
                        }
                        else
                        {
                            oPayment.Send.ReturnURL = baseURI + Url.Action("ATMpayOff", "Order");
                        }
                        oPayment.Send.ClientBackURL = baseURI;
                        //oPayment.Send.OrderResultURL = baseURI;
                        int number = (DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) * 3;
                        oPayment.Send.MerchantTradeNo = "MD" + DateTime.Now.Date.ToString("yyyyMMdd") + number.ToString("000000");
                        oPayment.Send.MerchantTradeDate = DateTime.Now;
                        oPayment.Send.TotalAmount = Decimal.Parse(payData.totalPay);
                        oPayment.Send.TradeDesc = "感謝購買墨台商品";
                        if (payData.payType == 1)
                        {
                            oPayment.Send.ChoosePayment = PaymentMethod.ATM;
                            oPayment.SendExtend.ExpireDate = Int32.Parse("3");
                        }
                        else
                        {
                            oPayment.Send.ChoosePayment = PaymentMethod.Credit;
                        }
                        //oPayment.Send.ChoosePayment = PaymentMethod.ALL;
                        oPayment.Send.Remark = "饒了我吧";
                        oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;
                        oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.No;
                        oPayment.Send.HoldTrade = HoldTradeType.No;
                        oPayment.Send.DeviceSource = DeviceType.PC;
                        oPayment.Send.UseRedeem = UseRedeemFlag.No; //購物金/紅包折抵
                        oPayment.Send.IgnorePayment = ""; // 例如財付通:Tenpay
                        // 加入選購商品資料。
                        foreach (var item in StatuList)
                        {
                            tProduct product = dbContext.tProducts.Where(p => p.ProductId.Equals(item.sProductId)).FirstOrDefault();
                            //var chg = JObject.Parse(item.Value.ToString());
                            oPayment.Send.Items.Add(new Item()
                            {
                                Name = product.pName,
                                Price = product.pPrice,
                                Currency = "NTD",
                                Quantity = item.sProductQty,
                                URL = "<< 產品說明位址 >>"
                            });

                        }
                        // 當付款方式為 ALL 時，建議增加的參數。
                        if(payData.payType == 1)
                        {
                            oPayment.SendExtend.PaymentInfoURL = baseURI + Url.Action("orderATM", "Order");
                        }
                        /* 產生訂單 */
                        enErrors.AddRange(oPayment.CheckOut());
                        /* 產生產生訂單 Html Code 的方法 */
                        //string szHtml = String.Empty;
                        enErrors.AddRange(oPayment.CheckOutString(ref szHtml));
                        WebOrderModel order = new WebOrderModel();
                        order.boughtList = StatuList;
                        order.webpay = payData;
                        order.customer = cust;
                        order.payDate = oPayment.Send.MerchantTradeDate;
                        Session[oPayment.Send.MerchantTradeNo] = order;
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
                return szHtml;
            }
            return String.Empty;
        }


    }
}