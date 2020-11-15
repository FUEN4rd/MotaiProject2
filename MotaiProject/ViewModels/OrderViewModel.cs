using AllPay.Payment.Integration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public int pIntPrice { get { return Convert.ToInt32(pPrice); } }
        [DisplayName("小計")]
        public int pTotal { get { return sProductQty * pIntPrice; } }
    }
    public class StatusCartGoToPayViewModel
    {
        public List<StatusCartViewModel> Carts { get; set; }

        [DisplayName("運送門市選擇")]
        public int WarehouseNameId { get; set; }
        [DisplayName("門市名稱")]
        public string WarehouseName { get; set; }
        public IEnumerable<SelectListItem> warehouses { get; set; }
    }
    public class WebPay
    {
        public string totalPay { get; set; }
        public int payType { get; set; }
        public string shipAddress { get; set; }
    }
    public class WebOrderModel
    {
        public List<tStatu> boughtList { get; set; }
        public WebPay webpay { get; set; }
        public tCustomer customer { get; set; }
        public DateTime payDate { get; set; }
    }
    //訂單付款
    public class Orderpay
    {
        [DisplayName("訂單編號")]
        public int oOrderId { get; set; }        
        public int oOrderInstallment { get; set; }
        [DisplayName("付款金額")]
        public decimal oPayment { get; set; }
        [DisplayName("付款日期")]
        public int oPayDate { get; set; }
        public int oPayTypeId { get; set; }
        [DisplayName("付款方式")]
        public string oPayType { get; set; }
    }
    public class CustomerOrderViewModel
    {        
        [DisplayName("消費時間")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime oDate { get; set; }
        [DisplayName("門市編號")]
        public int oWarehouseName { get; set; }
        [DisplayName("門市")]
        public string WarehouseName { get; set; }
        [DisplayName("員工編號")]
        public Nullable<int> oEmployeeId { get; set; }
        [DisplayName("專員")]
        public string EmployeeName { get; set; }
        [DisplayName("備註")]
        public string cNote { get; set; }

        public List<CustomerOrderDetailViewModel> CustomerOrderDetails { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
    public class CustomerOrderDetailViewModel
    {
        [DisplayName("訂單編號")]
        public int oOrderId { get; set; }
        public int oProductId { get; set; }
        [DisplayName("產品編號")]
        public string ProductNum { get; set; }
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }
        [DisplayName("產品單價")]
        public decimal ProductPrice { get; set; }
        [DisplayName("產品數量")]
        public int oProductQty { get; set; }
        [DisplayName("備註")]
        public string oNote { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
    }
    public class EmployeeOrderViewModel
    {
        [DisplayName("客戶手機")]
        public string cCellphone { get; set; }
        [DisplayName("客戶編號")]
        public int oCustomerId { get; set; }
        [DisplayName("訂單地址")]
        public string oAddress { get; set; }
        [DisplayName("訂單時間")]
        public DateTime oDate { get; set; }
        [DisplayName("員工編號")]
        public Nullable<int> oEmployeeId { get; set; }
        [DisplayName("優惠")]
        public Nullable<int> oPromotionId { get; set; }
        [DisplayName("備註")]
        public string cNote { get; set; }
        [DisplayName("門市編號")]
        public int oWarehouseName { get; set; }
        [DisplayName("門市")]
        public string WarehouseName { get; set; }

        public EmployeeOrderDetailViewModel empOrderDetail { get; set; }
        public List<EmployeeOrderDetailViewModel> empOrderDetails { get; set; }

        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
    public class EmployeeOrderDetailViewModel
    {        
        [DisplayName("訂單編號")]
        public int oOrderId { get; set; }
        public int oProductId { get; set; }
        [DisplayName("產品編號")]
        public string ProductNum { get; set; }
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }
        [DisplayName("產品數量")]
        public int oProductQty { get; set; }
        [DisplayName("備註")]
        public string oNote { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
    }
    //訂單跳轉結帳
    public class EmployeeCheckoutViewModel
    {
        public EmployeeOrderViewModel Order { get; set; }
        public List<EmployeeOrderDetailViewModel> orderDetails { get; set; }
        [DisplayName("總金額")]
        public int TotalAmount { get; set; }
        [DisplayName("已付金額")]
        public int AlreadyPay { get; set; }
        [DisplayName("尚未付款")]
        public int Unpaid { get; set; }
        
        public Orderpay orderPay { get; set; }
    }
    //續期結帳跳轉
    public class SearchCustomerOrderModel
    {
        public int orderId { get; set; }
        [DisplayName("購買日期")]
        public string purchaseDate { get; set; }
        [DisplayName("購買門市")]
        public string WarehouseName { get; set; }
    }
    public class OrderViewModel //審核
    {
        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        [DisplayName("訂單地址")]
        public string oAddress { get; set; }
        [DisplayName("訂單時間")]
        public DateTime oDate { get; set; }
        [DisplayName("交貨時間")]
        public Nullable<System.DateTime> oDeliverDate { get; set; }
        [DisplayName("審核")]
        public string oCheck { get; set; }
        [DisplayName("審核時間")]
        public Nullable<System.DateTime> oCheckDate { get; set; }
        [DisplayName("備註")]
        public string cNote { get; set; }

        [DisplayName("優惠")]
        public Nullable<int> oPromotionId { get; set; }
        [DisplayName("折扣金額")]
        public Nullable<int> pDiscount { get; set; }
        [DisplayName("活動名稱")]
        public string PromotionName { get; set; }

        [DisplayName("門市編號")]
        public int oWarehouseName { get; set; }
        [DisplayName("門市名稱")]
        public string sWarehouseName { get; set; }

        [DisplayName("員工編號")]
        public Nullable<int> oEmployeeId { get; set; }
        [DisplayName("職員姓名")]
        public string seName { get; set; }

        [DisplayName("客戶編號")]
        public int oCustomerId { get; set; }
        [DisplayName("客戶姓名")]
        public string scName { get; set; }

        [DisplayName("應收帳款")]
        public Nullable<int> receivable { get; set; }
        [DisplayName("已收帳款")]
        public Nullable<int> received { get; set; }
        [DisplayName("剩餘帳款")]
        public Nullable<int> surplus { get; set; }

        [DisplayName("現金")]
        public Nullable<int> cash { get; set; }
        [DisplayName("信用卡")]
        public Nullable<int> card { get; set; }
        [DisplayName("代金券")]
        public Nullable<int> voucher { get; set; }
        [DisplayName("寄售代收")]
        public Nullable<int> consignment { get; set; }

        [DisplayName("原價")]
        public int originalPrice { get; set; }

        [DisplayName("htmlName")]
        public string htmlName { get; set; }

        [DisplayName("商品")]
        public string pName { get; set; }
        [DisplayName("數量")]
        public string oProductQty { get; set; }

        public List<AccountOrderDetailViewModel> orderDetailViews { get; set; }
    }

    public class OrderDetailViewModel
    {
        [DisplayName("詳細編號")]
        public int oDetailId { get; set; }
        [DisplayName("訂單編號")]
        public int oOrderId { get; set; }
        public int oProductId { get; set; }
        [DisplayName("產品編號")]
        public string ProductNum { get; set; }
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }
        [DisplayName("產品數量")]
        public int oProductQty { get; set; }
        [DisplayName("備註")]
        public string oNote { get; set; }
    }

    public class AccountOrderDetailViewModel
    {
        [DisplayName("訂單編號")]
        public int oOrderId { get; set; }
        [DisplayName("產品編號")]
        public string ProductNum { get; set; }
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }
        [DisplayName("產品單價")]
        public decimal ProductPrice { get; set; }
        [DisplayName("產品數量")]
        public int oProductQty { get; set; }
        [DisplayName("備註")]
        public string oNote { get; set; }
    }

    public class BusinessOrderDetailViewModel
    {
        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        [DisplayName("產品編號")]
        public string ProductNum { get; set; }
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }
        [DisplayName("產品數量")]
        public int ProductQty { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
    }
}