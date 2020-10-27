using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class StockViewModel
    {
        public int WarehouseId { get; set; }
        public int WarehouseNameId { get; set; }
        public int wProductId { get; set; }
        public Nullable<int> wQuantity { get; set; }
        public int wSerialValue { get; set; }
        public System.DateTime wStockDate { get; set; }        
    }
}