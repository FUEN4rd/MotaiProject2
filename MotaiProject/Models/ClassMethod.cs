using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotaiProject.Models
{
    public class ClassMethod
    {
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
    }
}