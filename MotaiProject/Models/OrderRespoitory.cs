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
                Order.spDiscount = Convert.ToInt32(item.tPromotion.pDiscount);
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

                var receivedMoney = from td in dbContext.tOrders
                               from tp in dbContext.tOrderPays
                               where td.OrderId == tp.oOrderId
                               select tp.oPayment;
                int receivedTotal = 0;
                foreach (var receivedM in receivedMoney)
                {
                    receivedTotal = (int)receivedM + receivedTotal;
                }
                Order.received = receivedTotal;

                var receivableMoney = from td in dbContext.tOrders
                                    from tp in dbContext.tOrderDetails
                                    where td.OrderId == tp.oOrderId
                                    select tp.oProductQty*tp.tProduct.pPrice;
                int receivableTotal = 0;
                foreach (var receivableM in receivableMoney)
                {
                    receivableTotal = (int)receivableM + receivableTotal;
                }
                Order.receivable = receivableTotal;

                Order.surplus = receivableTotal - receivedTotal;

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

            //var receivableMoney = from tp in dbContext.tOrderPays
            //                      from tpt in dbContext.tOrderPayTypes
            //                      where tp.oPayType == tpt.oPayTypeId
            //                      select


            //                      Order.card = item.tOrderPays.
            return Order;
        }

        //public OrderViewModel saveGetOrderbyId(int Id)
        //{
        //    tOrder Order = dbContext.tOrders.FirstOrDefault(p => p.OrderId == Id);
        //    OrderViewModel item = new OrderViewModel();
        //    Order.oAddress = item.oAddress;
        //    Order.oCheck = item.oCheck;
        //    Order.oCheckDate = item.oCheckDate;
        //    Order.oDate = item.oDate;
        //    Order.oDeliverDate = item.oDeliverDate;
        //    Order.oEmployeeId = item.oEmployeeId;
        //    Order.OrderId = item.OrderId;

        //    Order.oCheck = item.oCheck;
        //    var date = DateTime.Now;
        //    Order.oCheckDate = date;
        //    return Order;
        //}

    }
}