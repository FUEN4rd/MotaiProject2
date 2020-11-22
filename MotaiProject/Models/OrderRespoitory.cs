using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;

namespace MotaiProject.Models
{
    public class OrderRespoitory
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
        public int SelectPromotionId(int money,DateTime date)
        {
            List<tPromotion> promotions = dbContext.tPromotions.OrderByDescending(p => p.pCondition).Where(p=>p.pPromotionDeadline>=date&&p.pPromotionStartDate<=date&&p.pCondition!=null).ToList();
            foreach(var item in promotions)
            {
                if (Convert.ToInt32(item.pCondition) <= money)
                {
                    return item.PromotionId;
                }
            }
            int promotionId = 0;
            return promotionId;
        }

        public OrderPayStatus GetPayStatus(int orderId)
        {
            tOrder order = dbContext.tOrders.Where(o => o.OrderId == orderId).FirstOrDefault();
            List<tOrderDetail> detaillist = dbContext.tOrderDetails.Where(od => od.oOrderId == orderId).ToList();
            List<tOrderPay> paylist = dbContext.tOrderPays.Where(op => op.oOrderId == orderId).ToList();
            OrderPayStatus payStatus = new OrderPayStatus();
            foreach(var detail in detaillist)
            {
                int price = Convert.ToInt32(dbContext.tProducts.Where(p => p.ProductId == detail.oProductId).FirstOrDefault().pPrice);
                payStatus.TotalAmount += detail.oProductQty * price;
            }
            tPromotion promotion = dbContext.tPromotions.Where(pm => pm.PromotionId == order.oPromotionId).FirstOrDefault();
            if (promotion != null)
            {
                payStatus.TotalAmount -= Convert.ToInt32(promotion.pDiscount);
            }
            if(paylist.Count() == 0)
            {
                payStatus.AlreadyPay = 0;
            }
            else
            {
                foreach(var payitem in paylist)
                {
                    payStatus.AlreadyPay += Convert.ToInt32(payitem.oPayment);
                }
            }
            payStatus.Unpaid = payStatus.TotalAmount - payStatus.AlreadyPay;
            return payStatus;
        }
        //public List<OrderViewModel> GetOrderQty()
        //{
        //    List<tOrderDetail> order = dbContext.tOrderDetails.ToList();
        //    List<OrderViewModel> orderlist = new List<OrderViewModel>();
        //    foreach(tOrderDetail item in order)
        //    {
        //        OrderViewModel Order = new OrderViewModel();
        //    }
        //}

        //public List<orderselect> GetEmpOrderAll()
        //{
        //    List<tOrder> order = dbContext.tOrders.ToList();
        //    List<orderselect> orderlist = new List<orderselect>();
        //    foreach (tOrder item in order)
        //    {
        //        orderselect Order = new orderselect();
        //        OrderViewModel orderview = new OrderViewModel();
        //        orderview.oAddress = item.oAddress;
        //        orderview.oCheck = item.oCheck;
        //        orderview.oCheckDate = item.oCheckDate;
        //        orderview.oDate = item.oDate;
        //        orderview.oDeliverDate = item.oDeliverDate;
        //        orderview.OrderId = item.OrderId;

        //        orderview.sWarehouseName = item.tWarehouseName.WarehouseName;
        //        orderview.seName = item.tEmployee.eName;
        //        orderview.scName = item.tCustomer.cName;
        //        var note = item.cNote;
        //        if (note != null)
        //        {
        //            if (note.Length > 10)
        //            {
        //                orderview.cNote = note.Substring(0, 10) + "...";
        //            }

        //            else
        //            {
        //                orderview.cNote = note;
        //            }
        //        }

        //        var receivedMoney = from tP in dbContext.tOrderPays
        //                            where tP.oOrderId == item.OrderId
        //                            select tP.oPayment;
        //        int receivedTotal = 0;
        //        foreach (var receivedM in receivedMoney)
        //        {
        //            receivedTotal = (int)receivedM + receivedTotal;
        //        }
        //        orderview.received = receivedTotal;

        //        var receivableMoney = from tp in dbContext.tOrderDetails
        //                              where tp.oOrderId == item.OrderId
        //                              select tp.oProductQty * tp.tProduct.pPrice;
        //        int receivableTotal = 0;
        //        foreach (var receivableM in receivableMoney)
        //        {
        //            receivableTotal += (int)receivableM;
        //        }
        //        if (item.oPromotionId != null)
        //        {
        //            orderview.pDiscount = Convert.ToInt32(item.tPromotion.pDiscount);
        //            receivableTotal -= Convert.ToInt32(item.tPromotion.pDiscount);
        //        }

        //        orderview.receivable = receivableTotal;
        //        var surplus = receivableTotal - receivedTotal;
        //        orderview.surplus = surplus;

        //        if (item.oCheck != null)
        //        {
        //            orderview.htmlName = "tr_hidden1";
        //        }
        //        else if (surplus <= 0)
        //        {
        //            orderview.htmlName = "tr_hidden2";
        //        }
        //        else
        //        {
        //            orderview.htmlName = "tr_hidden3";
        //        }
        //        Order.orderwatch = orderview;

        //        orderlist.Add(Order);
        //    }
        //    return orderlist;
        //}

        public List<OrderViewModel> GetOrderAll()
        {
            List<tOrder> order = dbContext.tOrders.ToList();
            var orderlists = order.OrderByDescending(c => c.oDate).ToList();
            List<OrderViewModel> orderlist = new List<OrderViewModel>();
            foreach (tOrder item in orderlists)
            {
                OrderViewModel Order = new OrderViewModel();
                Order.oAddress = item.oAddress;
                Order.oCheck = item.oCheck;
                if (item.oCheckDate != null)
                {
                    Order.oCheckDate = item.oCheckDate.Value.Date;
                }
                
                
                Order.oDate = item.oDate.Date;
                //if(item.oDeliverDate)
                //Order.oDeliverDate = item.oDeliverDate.Value.Date;
                Order.OrderId = item.OrderId;

                Order.sWarehouseName = item.tWarehouseName.WarehouseName;
                Order.seName = item.tEmployee.eName;
                Order.scName = item.tCustomer.cName;
                var note = item.cNote;
                if (note != null)
                {
                    if (note.Length > 10)
                    {
                        Order.cNote = note.Substring(0, 10) + "...";
                    }
                    else
                    {
                        Order.cNote = note;
                    }
                }
                //變數 - 觀察付了多少錢
                var receivedMoney = from tP in dbContext.tOrderPays
                                    where tP.oOrderId == item.OrderId
                                    select tP.oPayment;
                //已收到
                int receivedTotal = 0;
                foreach (var receivedM in receivedMoney)
                {
                    receivedTotal = (int)receivedM + receivedTotal;
                }

                Order.received = receivedTotal;
               //應收款
                var receivableMoney = from tp in dbContext.tOrderDetails
                                      where tp.oOrderId == item.OrderId
                                      select tp.oProductQty * tp.tProduct.pPrice ;
                //全額
                int receivableTotal = 0;

                foreach (var receivableM in receivableMoney)
                {
                    receivableTotal += (int)receivableM;
                }
                //折扣
                if (item.oPromotionId != null)
                {
                    Order.pDiscount = Convert.ToInt32(item.tPromotion.pDiscount);
                    receivableTotal -= Convert.ToInt32(item.tPromotion.pDiscount);
                }

                Order.receivable = receivableTotal;
                //應付款額-收款
                var surplus = receivableTotal - receivedTotal;
                Order.surplus = surplus;
                if(item.oCheck != null)
                {
                    Order.htmlName = "tr_hidden1";
                }
                else if (surplus <= 0)
                {
                    Order.htmlName = "tr_hidden2";
                }
                else
                {
                    Order.htmlName = "tr_hidden3";
                }
                List<AccountOrderDetailViewModel> detailViewModels = new List<AccountOrderDetailViewModel>();
                List<tOrderDetail> detailLists = dbContext.tOrderDetails.Where(od => od.oOrderId == item.OrderId).ToList();
                foreach(tOrderDetail itemdetail in detailLists)
                {
                    AccountOrderDetailViewModel detail = new AccountOrderDetailViewModel();
                    tProduct product = dbContext.tProducts.Where(p => p.ProductId == itemdetail.oProductId).FirstOrDefault();
                    detail.ProductNum = product.pNumber;
                    detail.ProductName = product.pName;
                    detail.ProductPrice = product.pPrice;
                    detail.oProductQty = itemdetail.oProductQty;
                    detail.oNote = itemdetail.oNote;
                    detailViewModels.Add(detail);
                }
                Order.orderDetailViews = detailViewModels;
                orderlist.Add(Order);
            }
            return orderlist;
        }

        public List<OrderViewModel> GetOrderAllByEmp(int EmployeeId)
        {
            List<tOrder> order = dbContext.tOrders.OrderByDescending(o=>o.oDate).Where(o => o.oEmployeeId == EmployeeId).ToList();
            List<OrderViewModel> orderlist = new List<OrderViewModel>();
            foreach (tOrder item in order)
            {
                OrderViewModel Order = new OrderViewModel();
                Order.oAddress = item.oAddress;
                Order.oCheck = item.oCheck;
                if (item.oCheckDate != null)
                {
                    Order.oCheckDate = item.oCheckDate.Value.Date;
                }


                Order.oDate = item.oDate.Date;
                //if(item.oDeliverDate)
                //Order.oDeliverDate = item.oDeliverDate.Value.Date;
                Order.OrderId = item.OrderId;

                Order.sWarehouseName = item.tWarehouseName.WarehouseName;
                Order.seName = item.tEmployee.eName;
                Order.scName = item.tCustomer.cName;
                var note = item.cNote;
                if (note != null)
                {
                    if (note.Length > 10)
                    {
                        Order.cNote = note.Substring(0, 10) + "...";
                    }
                    else
                    {
                        Order.cNote = note;
                    }
                }
                //變數 - 觀察付了多少錢
                var receivedMoney = from tP in dbContext.tOrderPays
                                    where tP.oOrderId == item.OrderId
                                    select tP.oPayment;
                //已收到
                int receivedTotal = 0;
                foreach (var receivedM in receivedMoney)
                {
                    receivedTotal = (int)receivedM + receivedTotal;
                }

                Order.received = receivedTotal;
                //應收款
                var receivableMoney = from tp in dbContext.tOrderDetails
                                      where tp.oOrderId == item.OrderId
                                      select tp.oProductQty * tp.tProduct.pPrice;
                //全額
                int receivableTotal = 0;

                foreach (var receivableM in receivableMoney)
                {
                    receivableTotal += (int)receivableM;
                }
                //折扣
                if (item.oPromotionId != null)
                {
                    tPromotion promotion = dbContext.tPromotions.Where(p => p.PromotionId == item.oPromotionId).FirstOrDefault();

                    if (promotion.PromotionName.Length > 7)
                    {
                        Order.PromotionName = promotion.PromotionName.Substring(0, 6) + ".";
                    }
                    else
                    {
                        Order.PromotionName = promotion.PromotionName;
                    }

                    //Order.PromotionName = promotion.PromotionName;
                    Order.pDiscount = Convert.ToInt32(item.tPromotion.pDiscount);
                    receivableTotal -= Convert.ToInt32(item.tPromotion.pDiscount);
                }

                Order.receivable = receivableTotal;
                //應付款額-收款
                var surplus = receivableTotal - receivedTotal;
                Order.surplus = surplus;
                if (item.oCheck != null)
                {
                    Order.htmlName = "tr_hidden1";
                }
                else if (surplus <= 0)
                {
                    Order.htmlName = "tr_hidden2";
                }
                else
                {
                    Order.htmlName = "tr_hidden3";
                }
                List<AccountOrderDetailViewModel> detailViewModels = new List<AccountOrderDetailViewModel>();
                List<tOrderDetail> detailLists = dbContext.tOrderDetails.Where(od => od.oOrderId == item.OrderId).ToList();
                foreach (tOrderDetail itemdetail in detailLists)
                {
                    AccountOrderDetailViewModel detail = new AccountOrderDetailViewModel();
                    tProduct product = dbContext.tProducts.Where(p => p.ProductId == itemdetail.oProductId).FirstOrDefault();
                    detail.ProductNum = product.pNumber;
                    detail.ProductName = product.pName;
                    detail.ProductPrice = product.pPrice;
                    detail.oProductQty = itemdetail.oProductQty;
                    detail.oNote = itemdetail.oNote;
                    detailViewModels.Add(detail);
                }
                Order.orderDetailViews = detailViewModels;
                orderlist.Add(Order);
            }
            return orderlist;
        }

        public OrderViewModel poGetOrderbyId(int Id)
        {
            tOrder item = dbContext.tOrders.FirstOrDefault(p => p.OrderId == Id);
            OrderViewModel Order = new OrderViewModel();
            Order.oAddress = item.oAddress;
            Order.oCheck = item.oCheck;
            Order.oCheckDate = item.oCheckDate;
    
            Order.oDate = item.oDate.Date;
            Order.oDeliverDate = item.oDeliverDate;
            Order.oEmployeeId = item.oEmployeeId;
            Order.OrderId = item.OrderId;
            Order.sWarehouseName = item.tWarehouseName.WarehouseName;
            Order.seName = item.tEmployee.eName;
            Order.scName = item.tCustomer.cName;
            int receivedM(int PayId)
            {
                var cashList = from tp in dbContext.tOrderPays
                               where tp.oPayType == PayId && tp.oOrderId==Id
                               select tp.oPayment;
                int cashTotal = 0;
                foreach (var cashItem in cashList)
                {
                    cashTotal = (int)cashItem + cashTotal;
                }
                return cashTotal;
            }
            Order.cash = receivedM(1);
            Order.card = receivedM(2);
            Order.voucher = receivedM(3);
            int receivedTotal = 0;
            for (int i = 1; i < 4; i++)
            {
                receivedTotal+=receivedM(i);
            }
            Order.received = receivedTotal;

            var receivableMoney = from tp in dbContext.tOrderDetails
                                  where tp.oOrderId == item.OrderId
                                  select tp.oProductQty * tp.tProduct.pPrice;
            int receivableTotal = 0;
            foreach (var receivableM in receivableMoney)
            {
                receivableTotal += (int)receivableM;
            }
            Order.originalPrice = receivableTotal;
            if (item.oPromotionId != null)
            {
                Order.receivable = receivableTotal - Convert.ToInt32(item.tPromotion.pDiscount);
                Order.PromotionName = item.tPromotion.PromotionName;
                Order.pDiscount = Convert.ToInt32(item.tPromotion.pDiscount);
            }
            else
            {
                Order.receivable = receivableTotal;
                Order.PromotionName = "無參與折扣活動";
                Order.pDiscount=0;
            }
            List<AccountOrderDetailViewModel> orderDetails = new List<AccountOrderDetailViewModel>();
            List<tOrderDetail> orderDetailList = dbContext.tOrderDetails.Where(od => od.oOrderId == Id).ToList();
            foreach(var detail in orderDetailList)
            {
                AccountOrderDetailViewModel accountOrderdetail = new AccountOrderDetailViewModel();
                tProduct product = dbContext.tProducts.Where(p => p.ProductId == detail.oProductId).FirstOrDefault();
                accountOrderdetail.ProductNum = product.pNumber;
                accountOrderdetail.ProductName = product.pName;
                accountOrderdetail.ProductPrice = product.pPrice;
                accountOrderdetail.oProductQty = detail.oProductQty;
                accountOrderdetail.oNote = detail.oNote;
                orderDetails.Add(accountOrderdetail);
            }
            Order.orderDetailViews = orderDetails;
            return Order;
        }

    }
}