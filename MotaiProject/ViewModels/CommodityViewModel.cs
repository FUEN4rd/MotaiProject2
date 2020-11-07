﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class CommodityViewModel
    {
    }
    public class WareHouseViewModel
    {
        public int WarehouseId { get; set; }
        public int WarehouseNameId { get; set; }
        [DisplayName("倉儲名")]
        public string WarehouseName { get; set; }
        public int wProductId { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        public int wPQty { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
    //倉儲查詢
    public class WareInventorySelectViewModel
    {
        [DisplayName("倉儲名")]
        public string WarehouseName { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        [DisplayName("倉儲庫存")]
        public int ProductQty { get; set; }
    }
    public class TransferViewModel
    {
        public int TransferId { get; set; }
        public int tProductId { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        public int tProductQty { get; set; }
        [DisplayName("員工名")]
        public string EmpName { get; set; }
        public int tEmployeeId { get; set; }
        [DisplayName("調出倉儲")]
        public string WareHouseOutName { get; set; }
        public int tWNIdOut { get; set; }
        [DisplayName("調進倉儲名")]
        public string WareHouseInName { get; set; }
        public int tWNIdIn { get; set; }
        public System.DateTime tDate { get; set; }
        public string tNote { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> EmpNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseOutNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseInNames { get; set; }
    }
    //進貨
    //進貨單建立
    public class StockListViewModel
    {
        public int StockId { get; set; }
        public int sEmployeeId { get; set; }
        [DisplayName("進貨單號")]
        public int sStockSerialValue { get; set; }
        [DisplayName("聯絡人")]
        public string sVendor { get; set; }
        [DisplayName("聯絡方式")]
        public string sVendorTel { get; set; }
        [DisplayName("進貨日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime sStockDate { get { return DateTime.Now.Date; } set { } }
        [DisplayName("進貨備註")]
        public string sStockNote { get; set; }
        public List<StockDetailViewModel> StockDetails { get; set; }
    }
    public class StockDetailViewModel
    {
        public int StockDetailId { get; set; }
        public int sStockId { get; set; }
        public int sProductId { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        [DisplayName("成本")]
        public decimal sCost { get; set; }
        [DisplayName("數量")]
        public int sQuantity { get; set; }
        [DisplayName("倉儲名")]
        public string WareHouseName { get; set; }
        public int sWarehouseNameId { get; set; }
        [DisplayName("備註")]
        public string sNote { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
    public class StockCreateViewModel
    {
        public int StockId { get; set; }
        public int sEmployeeId { get; set; }
        [DisplayName("進貨單號")]
        public int sStockSerialValue { get; set; }
        [DisplayName("聯絡人")]
        public string sVendor { get; set; }
        [DisplayName("聯絡方式")]
        public string sVendorTel { get; set; }
        [DisplayName("進貨日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime sStockDate { get { return DateTime.Now.Date; } set { } }
        [DisplayName("進貨備註")]
        public string sStockNote { get; set; }
        public StockDetailViewModel StockDetail { get; set; }
        public List<StockDetailViewModel> StockDetails { get; set; }
    }
    //進貨單查詢
    public class StockSelectViewModel
    {
        [DisplayName("負責人")]
        public string EmployeeName { get; set; }
        [DisplayName("進貨單號")]
        public int sStockSerialValue { get; set; }
        [DisplayName("聯絡人")]
        public string sVendor { get; set; }
        [DisplayName("聯絡方式")]
        public string sVendorTel { get; set; }
        [DisplayName("進貨日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime sStockDate { get { return DateTime.Now.Date; } set { } }
        [DisplayName("進貨備註")]
        public string sStockNote { get; set; }
        public List<StockSelectDetailModel> StockDetails { get; set; }
    }
    public class StockSelectDetailModel
    {
        [DisplayName("商品編號")]
        public string ProductNum { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        [DisplayName("成本")]
        public decimal sCost { get; set; }
        [DisplayName("數量")]
        public int sQuantity { get; set; }
        [DisplayName("倉儲名")]
        public string WareHouseName { get; set; }
        [DisplayName("備註")]
        public string sNote { get; set; }
    }
    //出貨
    public class ShipListViewModel
    {
        public int ShipId { get; set; }
        public int sEmployeeId { get; set; }
        [DisplayName("出貨單號")]
        public int sShipSerialValue { get; set; }
        [DisplayName("訂單號")]
        public int sOrderId { get; set; }
        [DisplayName("出貨日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime sShipDate { get { return DateTime.Now.Date; } set { } }
        [DisplayName("出貨備註")]
        public string sShipNote { get; set; }

    }
    public class ShipDetailViewModel
    {
        public int ShipDetailId { get; set; }
        public int sStockId { get; set; }
        public int sOrderDetailId { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        public int sProductId { get; set; }
        [DisplayName("數量")]
        public int sQuantity { get; set; }
        [DisplayName("倉儲名")]
        public string WareHouseName { get; set; }
        public int sWarehouseNameId { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
    public class ShipCreateViewModel
    {
        public int ShipId { get; set; }
        public int sEmployeeId { get; set; }
        [DisplayName("出貨單號")]
        public int sShipSerialValue { get; set; }
        [DisplayName("訂單號")]
        public int sOrderId { get; set; }
        [DisplayName("出貨日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime sShipDate { get { return DateTime.Now.Date; } set { } }
        [DisplayName("出貨備註")]
        public string sShipNote { get; set; }

    }
    //出貨單建立
    public class OrderShipShowViewModel
    {
        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        [DisplayName("訂單地址")]
        public string oAddress { get; set; }
        [DisplayName("訂單時間")]
        public DateTime oDate { get; set; }
        [DisplayName("審核")]
        public string oCheck { get; set; }
        [DisplayName("審核時間")]
        public Nullable<System.DateTime> oCheckDate { get; set; }
        [DisplayName("備註")]
        public string cNote { get; set; }
    }
    public class OrderDetailShipShowViewModel
    {
        [DisplayName("產品編號")]
        public string ProductNum { get; set; }
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }
        [DisplayName("產品數量")]
        public int oProductQty { get; set; }
        [DisplayName("備註")]
        public string oNote { get; set; }
    }
    //出貨單查詢
    public class ShipSelectViewModel
    {
        [DisplayName("負責人")]
        public string EmployeeName { get; set; }
        [DisplayName("出貨單號")]
        public int ShipSerialValue { get; set; }
        [DisplayName("出貨日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ShipDate { get { return DateTime.Now.Date; } set { } }
        [DisplayName("出貨備註")]
        public string ShipNote { get; set; }
        public List<ShipSelectDetailModel> ShipDetails { get; set; }
    }
    public class ShipSelectDetailModel
    {
        [DisplayName("商品編號")]
        public string ProductNum { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        [DisplayName("對應訂單")]
        public int OrderId { get; set; }
        [DisplayName("數量")]
        public int sQuantity { get; set; }
        [DisplayName("倉儲名")]
        public string WareHouseName { get; set; }
    }
}