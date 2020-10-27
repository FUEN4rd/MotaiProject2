using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class StockViewModel
    {
        private tStockList stock;
        public tStockList StockList
        {
            get
            {
                if (stock == null)
                {
                    stock = new tStockList();
                }
                return stock;
            }
            set => stock = value;
        }

        public int StockId { get { return this.StockList.StockId; } set { StockList.StockId = value; } }
        public int sEmployeeId { get { return this.StockList.sEmployeeId; } set { StockList.sEmployeeId = value; } }
        [DisplayName("員工名")]
        public string EmpName { get { return this.StockList.tEmployee.eName; } set { StockList.tEmployee.eName = value; } }
        public int sStockSerialValue { get { return this.StockList.sStockSerialValue; } set { StockList.sStockSerialValue = value; } }
        public string sVendor { get { return this.StockList.sVendor; } set { StockList.sVendor = value; } }
        public string sVendorTel { get { return this.StockList.sVendorTel; } set { StockList.sVendorTel = value; } }
        public DateTime sStockDate { get { return DateTime.Now; } set { StockList.sStockDate = value; } }
        public string sStockNote { get { return this.StockList.sStockNote; } set { StockList.sStockNote = value; } }

        public IEnumerable<SelectListItem> EmpNames { get; set; }
    }
}