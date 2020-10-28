using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotaiProject.Models
{
    public class ClassMethod
    {
        //Product
        public Dictionary<int, string> GetProductAll()
        {
            var pNames = (new MotaiDataEntities()).tProducts.OrderBy(p => p.pName);
            return pNames.ToDictionary(pid => pid.ProductId, pn => pn.pName);
        }

        public Dictionary<int, string> GetCategoryAll()
        {
            var categories = (new MotaiDataEntities()).tProductCategories.OrderBy(c => c.Category);                            
            return categories.ToDictionary(cid=>cid.pCategoryId,cn=>cn.Category);
        }

        public Dictionary<int, string> GetMaterialAll()
        {
            var materials = (new MotaiDataEntities()).tProductMaterials.OrderBy(c => c.Material);            
            return materials.ToDictionary(mid => mid.pMaterialId, mn => mn.Material);
        }

        public Dictionary<int, string> GetSizeAll()
        {
            var sizes = (new MotaiDataEntities()).tProductSizes.OrderBy(c => c.Size);
            return sizes.ToDictionary(mid => mid.pSizeId, mn => mn.Size);
        }

        //Warehouse
        public Dictionary<int, string> GetWarehouseAll()
        {
            var warehouses = (new MotaiDataEntities()).tWarehouseNames.OrderBy(w => w.WarehouseName);
            return warehouses.ToDictionary(wid => wid.WarehouseNameId, wn => wn.WarehouseName);
        }

        //Employee
        public Dictionary<int, string> GetEmployeeAll()
        {
            var empNames = (new MotaiDataEntities()).tEmployees.OrderBy(e => e.eName);
            return empNames.ToDictionary(eid => eid.EmployeeId, en => en.eName);
        }
    }
}