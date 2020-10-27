using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class WareHouseViewModel
    {
        private tWarehouse warehouse;
        public tWarehouse Warehouse
        {
            get
            {
                if (warehouse == null)
                {
                    warehouse = new tWarehouse();
                }
                return warehouse;
            }
            set => warehouse = value;
        }

        public int WarehouseId { get { return this.Warehouse.WarehouseId; } set { Warehouse.WarehouseId = value; } }
        public int WarehouseNameId { get { return this.Warehouse.WarehouseNameId; } set { Warehouse.WarehouseNameId = value; } }
        [DisplayName("倉儲名")]
        public string WarehouseName { get { return this.Warehouse.tWarehouseName.WarehouseName; } set { Warehouse.tWarehouseName.WarehouseName = value; } }
        public int wProductId { get { return this.Warehouse.wProductId; } set { Warehouse.wProductId = value; } }
        [DisplayName("商品名")]
        public string ProductName { get { return this.Warehouse.tProduct.pName; } set { Warehouse.tProduct.pName = value; } }
        public int wPQty { get { return this.Warehouse.wPQty; } set { Warehouse.wPQty = value; } }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
}