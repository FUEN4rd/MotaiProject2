using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    //訂單付款
    public class Orderpay
    {
        public int oPayId { get; set; }
        public int oOrderId { get; set; }
        public int oOrderInstallment { get; set; }
        public decimal oPayment { get; set; }
        public int oPayDate { get; set; }
        public int oPayTypeId { get; set; }
        public string oPayType { get; set; }

    }

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
        public decimal pTotal { get { return sProductQty * pPrice; } }
    }

    public class OrderViewModel
    {
        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        [DisplayName("客戶編號")]
        public int oCustomerId { get; set; }
        [DisplayName("訂單地址")]
        public string oAddress { get; set; }
        [DisplayName("訂單時間")]
        public DateTime oDate { get; set; }
        [DisplayName("交貨時間")]
        public Nullable<System.DateTime> oDeliverDate { get; set; }
        [DisplayName("員工編號")]
        public Nullable<int> oEmployeeId { get; set; }
        [DisplayName("審核")]
        public string oCheck { get; set; }
        [DisplayName("審核時間")]
        public Nullable<System.DateTime> oCheckDate { get; set; }
        [DisplayName("優惠")]
        public Nullable<int> oPromotionId { get; set; }
        [DisplayName("備註")]
        public string cNote { get; set; }
        [DisplayName("出貨倉儲")]
        public int oWarehouseName { get; set; }
    }

    public class OrderDetailViewModel
    {
        [DisplayName("詳細編號")]
        public int oDetailId { get; set; }
        [DisplayName("訂單編號")]
        public int oOrderId { get; set; }
        public int oProductId { get; set; }
        [DisplayName("產品編號")]
        public string oProductNum { get; set; }
        [DisplayName("產品名稱")]
        public string oProductName { get; set; }
        [DisplayName("產品數量")]
        public int oProductQty { get; set; }
        [DisplayName("備註")]
        public string oNote { get; set; }
    }
}