using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class ShipDetailViewModel
    {
        private tShipDetail shipdetail;
        public tShipDetail ShipDetail
        {
            get
            {
                if (shipdetail == null)
                {
                    shipdetail = new tShipDetail();
                }
                return shipdetail;
            }
            set => shipdetail = value;
        }
        public int ShipDetailId { get { return this.ShipDetail.ShipDetailId; } set { ShipDetail.ShipDetailId = value; } }
        public int sStockId { get { return this.ShipDetail.ShipId; } set { ShipDetail.ShipId = value; } }
        public int sOrderDetailId { get { return this.ShipDetail.sOrderDetailId; } set { ShipDetail.sOrderDetailId = value; } }
        [DisplayName("商品名")]
        public string ProductName { get { return this.ShipDetail.tProduct.pName; } set { ShipDetail.tProduct.pName = value; } }
        public int sProductId { get { return this.ShipDetail.sProductId; } set { ShipDetail.sProductId = value; } }
        public int sQuantity { get { return this.ShipDetail.sQuantity; } set { ShipDetail.sQuantity = value; } }
        [DisplayName("倉儲名")]
        public string WareHouseName { get { return this.ShipDetail.tWarehouseName.WarehouseName; } set { ShipDetail.tWarehouseName.WarehouseName = value; } }
        public int sWarehouseNameId { get { return this.ShipDetail.sWarehouseNameId; } set { ShipDetail.sWarehouseNameId = value; } }
        

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseNames { get; set; }
    }
}