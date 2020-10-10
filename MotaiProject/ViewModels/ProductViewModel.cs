using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class ProductViewModel
    {
        private tProduct prod;
        public tProduct Product
        {
            get
            {
                if (prod == null)
                {
                    prod = new tProduct();
                }
                return prod;
            }
            set => prod = value;
        }
        public int ProductId { get { return this.Product.ProductId; } set { Product.ProductId = value; } }
        public string pNumber { get { return this.Product.pNumber; } set { Product.pNumber = value; } }
        public string pName { get { return this.Product.pName; } set { Product.pName = value; } }
        public int pCategory { get { return this.Product.pCategory; }
            set { Product.pCategory = value; } }
        public int pMaterial { get { return this.Product.pMaterial; }
            set { Product.pMaterial = value; } }
        public int pSize { get { return this.Product.pSize; }
            set { Product.pSize = value; } }
        public string pLxWxH { get { return this.Product.pLxWxH; } set { Product.pLxWxH = value; } }
        public double pWeight { get { return this.Product.pWeight; } set { Product.pWeight = value; } }
        public Nullable<int> pImage { get { return this.Product.pImage; } set { Product.pImage = value; } }
        public decimal pPrice { get { return this.Product.pPrice; } set { Product.pPrice = value; } }
        public int pQty { get { return (int)this.Product.tWarehouses.FirstOrDefault(w => w.wProductId.Equals(ProductId)).wQuantity; }
            set { Product.tWarehouses.FirstOrDefault(w => w.wProductId.Equals(ProductId)).wQuantity = value; } }
    }
}