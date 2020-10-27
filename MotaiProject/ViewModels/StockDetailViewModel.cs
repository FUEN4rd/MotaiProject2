using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class StockDetailViewModel
    {
        private tStockDetail stockdetail;
        public tStockDetail StockDetail
        {
            get
            {
                if (stockdetail == null)
                {
                    stockdetail = new tStockDetail();
                }
                return stockdetail;
            }
            set => stockdetail = value;
        }
        public int StockDetailId { get { return this.StockDetail.StockDetailId; } set { StockDetail.StockDetailId = value; } }
        public int sStockId { get { return this.StockDetail.sStockId; } set { StockDetail.sStockId = value; } }
        [DisplayName("商品名")]
        public string ProductName { get { return this.StockDetail.tProduct.pName; } set { StockDetail.tProduct.pName = value; } }
        public int sProductId { get { return this.StockDetail.sProductId; } set { StockDetail.sProductId = value; } }
        public decimal sCost { get { return this.StockDetail.sCost; } set { StockDetail.sCost = value; } }
        public int sQuantity { get { return this.StockDetail.sQuantity; } set { StockDetail.sQuantity = value; } }
        [DisplayName("倉儲名")]
        public string WareHouseName { get { return this.StockDetail.tWarehouseName.WarehouseName; } set { StockDetail.tWarehouseName.WarehouseName = value; } }
        public int sWarehouseNameId { get { return this.StockDetail.sWarehouseNameId; } set { StockDetail.sWarehouseNameId = value; } }
        public string sNote { get { return this.StockDetail.sNote; } set { StockDetail.sNote = value; } }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
}