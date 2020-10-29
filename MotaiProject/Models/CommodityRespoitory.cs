using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Models
{
    public class CommodityRespoitory
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
        //Warehouse
        public Dictionary<int, string> GetWarehouseAll()
        {
            var warehouses = dbContext.tWarehouseNames.OrderBy(w => w.WarehouseName);
            return warehouses.ToDictionary(wid => wid.WarehouseNameId, wn => wn.WarehouseName);
        }

        public List<SelectListItem> GetSelectList(Dictionary<int, string> dictionary)
        {
            List<SelectListItem> selectLists = new List<SelectListItem>();
            foreach (var items in dictionary)
            {
                selectLists.Add(new SelectListItem()
                {
                    Text = items.Value,
                    Value = items.Key.ToString()
                });
            }
            return selectLists;
        }
    }
}