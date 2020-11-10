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
            List<tPromotion> promotions = dbContext.tPromotions.OrderByDescending(p => p.pCondition).Where(p=>p.pPromotionDeadline>date&&p.pPromotionStartDate<date).ToList();
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

        //public List<OrderViewModel> GetOrderQty()
        //{
        //    List<tOrderDetail> order = dbContext.tOrderDetails.ToList();
        //    List<OrderViewModel> orderlist = new List<OrderViewModel>();
        //    foreach(tOrderDetail item in order)
        //    {
        //        OrderViewModel Order = new OrderViewModel();
                
         
        //    }
        //}

        public List<OrderViewModel> GetOrderAll()
        {
            List<tOrder> order = dbContext.tOrders.ToList();
            List<OrderViewModel> orderlist = new List<OrderViewModel>();
            foreach (tOrder item in order)
            {
                OrderViewModel Order = new OrderViewModel();
                Order.oAddress = item.oAddress;
                Order.oCheck = item.oCheck;
                Order.oCheckDate = item.oCheckDate;
                Order.oDate = item.oDate;
                Order.oDeliverDate = item.oDeliverDate;
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

                var receivedMoney = from tP in dbContext.tOrderPays
                                    where tP.oOrderId == item.OrderId
                                    select tP.oPayment;
                int receivedTotal = 0;
                foreach (var receivedM in receivedMoney)
                {
                    receivedTotal = (int)receivedM + receivedTotal;
                }
                Order.received = receivedTotal;
               
                var receivableMoney = from tp in dbContext.tOrderDetails
                                      where tp.oOrderId == item.OrderId
                                      select tp.oProductQty * tp.tProduct.pPrice ;                                     
                int receivableTotal = 0;
                foreach (var receivableM in receivableMoney)
                {
                    receivableTotal += (int)receivableM;
                }
                if (item.oPromotionId != null)
                {
                    Order.pDiscount = Convert.ToInt32(item.tPromotion.pDiscount);
                    receivableTotal -= Convert.ToInt32(item.tPromotion.pDiscount);
                }
                //receivableTotal -= Convert.ToInt32(dbContext.tPromotions.Where(w => w.PromotionId.Equals(item.oPromotionId)).FirstOrDefault().pDiscount);
                //List<tOrderDetail> money = dbContext.tOrderDetails.Where(od => od.oOrderId.Equals(item.OrderId)).ToList();
                //int receivableTotal = 0;
                //foreach(var itemmoney in money)
                //{
                //    int price = (int)dbContext.tProducts.Where(p => p.ProductId.Equals(itemmoney.oProductId)).FirstOrDefault().pPrice;
                //    receivableTotal += itemmoney.oProductQty * price;
                //}

                Order.receivable = receivableTotal;
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
            Order.oDate = item.oDate;
            Order.oDeliverDate = item.oDeliverDate;
            Order.oEmployeeId = item.oEmployeeId;
            Order.OrderId = item.OrderId;

            Order.sWarehouseName = item.tWarehouseName.WarehouseName;
            Order.seName = item.tEmployee.eName;
            Order.scName = item.tCustomer.cName;
            int receivedM(int PayId)
            {
                var cashList = from tp in dbContext.tOrderPays
                               where tp.oPayId == PayId
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
            Order.consignment = receivedM(4);
            int receivedTotal = 0;
            for (int i = 1; i < 5; i++)
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
                Order.receivable = receivableTotal -= Convert.ToInt32(item.tPromotion.pDiscount);
                Order.PromotionName = item.tPromotion.PromotionName;
                Order.pDiscount = Convert.ToInt32(item.tPromotion.pDiscount);
            }
            return Order;
        }

    }
}