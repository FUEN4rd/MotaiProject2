using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class TransferViewModel
    {
        private tTransfer tran;
        public tTransfer Transfer
        {
            get
            {
                if (tran == null)
                {
                    tran = new tTransfer();
                }
                return tran;
            }
            set => tran = value;
        }

        public int TransferId { get { return this.Transfer.TransferId; } set { Transfer.TransferId = value; } }
        public int tProductId { get { return this.Transfer.tProductId; } set { Transfer.tProductId = value; } }
        [DisplayName("商品名")]
        public string ProductName { get { return this.Transfer.tProduct.pName; } set { Transfer.tProduct.pName = value; } }
        public int tProductQty { get { return this.Transfer.tProductQty; } set { Transfer.tProductQty = value; } }
        [DisplayName("員工名")]
        public string EmpName { get { return this.Transfer.tEmployee.eName; } set { Transfer.tEmployee.eName = value; } }
        public int tEmployeeId { get { return this.Transfer.tEmployeeId; } set { Transfer.tEmployeeId = value; } }
        [DisplayName("調出倉儲")]
        public string WareHouseOutName { get { return this.Transfer.tWarehouseName.WarehouseName; } set { Transfer.tWarehouseName.WarehouseName = value; } }
        public int tWNIdOut { get { return this.Transfer.tWNIdOut; } set { Transfer.tWNIdOut = value; } }
        [DisplayName("調進倉儲名")]
        public string WareHouseInName { get { return this.Transfer.tWarehouseName.WarehouseName; } set { Transfer.tWarehouseName.WarehouseName = value; } }
        public int tWNIdIn { get { return this.Transfer.tWNIdIn; } set { Transfer.tWNIdIn = value; } }
        public System.DateTime tDate { get { return DateTime.Now; } set { Transfer.tDate = value; } }
        public string tNote { get { return this.Transfer.tNote; } set { Transfer.tNote = value; } }

        public IEnumerable<SelectListItem> ProductNames { get; set; }
        public IEnumerable<SelectListItem> EmpNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseOutNames { get; set; }
        public IEnumerable<SelectListItem> WareHouseInNames { get; set; }
    }
}