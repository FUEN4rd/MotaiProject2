using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class StatusCartViewModel
    {
        public int StatusId { get; set; }
        public int ProductId { get; set; }
        [DisplayName("產品名稱")]
        public string pName { get; set; }
        [DisplayName("產品單價")]
        public decimal pPrice { get; set; }
        [DisplayName("購買數量")]
        public int sProductQty { get; set; }
        [DisplayName("小計")]
        public decimal pTotal { get { return pPrice * sProductQty; } }
    }

    public class OrderViewModel
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
        private tOrder ord;
        public tOrder Order
        {
            get
            {
                if (ord == null)
                {
                    ord = new tOrder();
                }
                return ord;
            }
            set => ord = value;
        }

        [DisplayName("訂單編號")]
        public int OrderId { get { return this.Order.OrderId; } set { Order.OrderId = value; } }
        [DisplayName("客戶編號")]
        public int oCustomerId { get { return Convert.ToInt32(this.Order.oCustomerId); } set { Order.oCustomerId = value; } }
        [DisplayName("訂單地址")]
        public string oAddress { get { return this.Order.oAddress; } set { Order.oAddress = value; } }
        [DisplayName("訂單時間")]
        public DateTime oDate { get { return this.Order.oDate; } set { Order.oDate = value; } }
        [DisplayName("交貨時間")]
        public Nullable<System.DateTime> oDeliverDate { get { return this.Order.oDeliverDate; } set { Order.oDeliverDate = value; } }
        [DisplayName("員工編號")]
        public Nullable<int> oEmployeeId { get { return this.Order.oEmployeeId; } set { Order.oEmployeeId = value; } }
        [DisplayName("審核")]
        public string oCheck { get { return this.Order.oCheck; } set { Order.oCheck = value; } }
        [DisplayName("審核時間")]
        public Nullable<System.DateTime> oCheckDate { get { return this.Order.oCheckDate; } set { Order.oCheckDate = value; } }
        [DisplayName("優惠")]
        public Nullable<int> oPromotionId { get { return this.Order.oPromotionId; } set { Order.oPromotionId = value; } }
        [DisplayName("備註")]
        public string cNote { get; set; }
        [DisplayName("出貨倉儲")]
        public int oWarehouseName { get; set; }
    }

    public class OrderDetailViewModel
    {
        MotaiDataEntities db = new MotaiDataEntities();
        private tOrderDetail det;
        public tOrderDetail Det
        {
            get
            {
                if (det == null)
                {
                    det = new tOrderDetail();
                }
                return det;
            }
            set => det = value;
        }
        [DisplayName("詳細編號")]
        public int oDetailId { get { return this.Det.oOrderDetailId; } set { Det.oOrderDetailId = value; } }
        [DisplayName("訂單編號")]
        public int oOrderId { get { return this.Det.oOrderId; } set { Det.oOrderId = value; } }
        public int oProductId { get { return this.Det.oProductId; } set { Det.oProductId = value; } }
        [DisplayName("產品編號")]
        public string oProductNum { get { return this.Det.tProduct.pNumber; } set { Det.tProduct.pNumber = value; } }
        [DisplayName("產品名稱")]
        public string oProductName { get { return this.Det.tProduct.pName; } set { Det.tProduct.pName = value; } }
        [DisplayName("產品數量")]
        public int oProductQty { get { return this.Det.oProductQty; } set { Det.oProductQty = value; } }
        [DisplayName("備註")]
        public string oNote { get { return this.Det.oNote; } set { Det.oNote = value; } }
    }
}