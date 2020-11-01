using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Models
{
    public class ProductRespoitory
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
        //Product
        public List<ProductViewModel> GetProductAll()
        {            
            List<tProduct> prod = dbContext.tProducts.ToList();
            List<ProductViewModel> productlist = new List<ProductViewModel>();
            foreach (tProduct item in prod)
            {
                List<tProductImage> images = dbContext.tProductImages.Where(i => i.ProductId.Equals(item.ProductId)).ToList();
                ProductViewModel Prod = new ProductViewModel();
                Prod.ProductId = item.ProductId;
                Prod.pNumber = item.pNumber;
                Prod.pName = item.pName;
                Prod.psCategory = item.tProductCategory.Category;
                Prod.psMaterial = item.tProductMaterial.Material;
                Prod.psSize = item.tProductSize.Size;
                Prod.pLxWxH = item.pLxWxH;
                Prod.pWeight = item.pWeight;
                Prod.pIntroduction = item.pIntroduction;
                Prod.pPrice = item.pPrice;
                Prod.pQty = (int)item.pQty;
                Prod.psImage = GetProductShowImages(item);               
                productlist.Add(Prod);
            }
            return productlist;
        }

        public List<string> GetProductShowImages(tProduct product)
        {
            List<string> psImage = new List<string>();
            List<tProductImage> images = dbContext.tProductImages.Where(i => i.ProductId.Equals(product.ProductId)).ToList();
            foreach (var imageitem in images)
            {
                psImage.Add(imageitem.pImage);
            }
            return psImage;
        }

        public ProductViewModel GetProductById(int ProductId)
        {
            tProduct product = dbContext.tProducts.FirstOrDefault(p => p.ProductId == ProductId);
            ProductViewModel Prod = new ProductViewModel();
            Prod.ProductId = product.ProductId;
            Prod.pNumber = product.pNumber;
            Prod.pName = product.pName;
            Prod.psCategory = product.tProductCategory.Category;
            Prod.psMaterial = product.tProductMaterial.Material;
            Prod.psSize = product.tProductSize.Size;
            Prod.pLxWxH = product.pLxWxH;
            Prod.pWeight = product.pWeight;
            Prod.pIntroduction = product.pIntroduction;
            Prod.pPrice = product.pPrice;
            Prod.pQty = (int)product.pQty;
            Prod.psImage = GetProductShowImages(product);
            return Prod;
        }

        public Dictionary<int, string> GetNameAll()
        {
            var pNames = dbContext.tProducts.OrderBy(p => p.pName);
            return pNames.ToDictionary(pid => pid.ProductId, pn => pn.pName);
        }

        public Dictionary<int, string> GetCategoryAll()
        {
            var categories = dbContext.tProductCategories.OrderBy(c => c.Category);                            
            return categories.ToDictionary(cid=>cid.pCategoryId,cn=>cn.Category);
        }

        public Dictionary<int, string> GetMaterialAll()
        {
            var materials = dbContext.tProductMaterials.OrderBy(c => c.Material);            
            return materials.ToDictionary(mid => mid.pMaterialId, mn => mn.Material);
        }

        public Dictionary<int, string> GetSizeAll()
        {
            var sizes = dbContext.tProductSizes.OrderBy(c => c.Size);
            return sizes.ToDictionary(mid => mid.pSizeId, mn => mn.Size);
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