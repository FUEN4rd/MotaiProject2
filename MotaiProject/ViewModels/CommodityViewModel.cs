using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class StockViewModel
    {
        public int StockId { get; set; }
        public int sEmployeeId { get; set; }
        [DisplayName("員工名")]
        public string EmpName { get; set; }
        public int sStockSerialValue { get; set; }
        public string sVendor { get; set; }
        public string sVendorTel { get; set; }
        public DateTime sStockDate { get; set; }
        public string sStockNote { get; set; }

        public IEnumerable<SelectListItem> EmpNames { get; set; }
    }
    public class StockDetailViewModel
    {
        public int StockDetailId { get; set; }
        public int sStockId { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        public int sProductId { get; set; }
        public decimal sCost { get; set; }
        public int sQuantity { get; set; }
        [DisplayName("倉儲名")]
        public string WareHouseName { get; set; }
        public int sWarehouseNameId { get; set; }
        public string sNote { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }

    //出貨
    public class ShipListViewModel
    {
        public int ShipId { get; set; }
        public int sEmployeeId { get; set; }
        [DisplayName("員工名")]
        public string EmpName { get; set; }
        public int sShipSerialValue { get; set; }
        public int sOrderId { get; set; }
        public DateTime sShipDate { get; set; }
        public string sShipNote { get; set; }

        public IEnumerable<SelectListItem> EmpNames { get; set; }
    }
    public class ShipDetailViewModel
    {
        public int ShipDetailId { get; set; }
        public int sStockId { get; set; }
        public int sOrderDetailId { get; set; }
        [DisplayName("商品名")]
        public string ProductName { get; set; }
        public int sProductId { get; set; }
        public int sQuantity { get; set; }
        [DisplayName("倉儲名")]
        public string WareHouseName { get; set; }
        public int sWarehouseNameId { get; set; }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
}