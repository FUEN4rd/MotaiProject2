using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class StatusCartViewModel
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
        private tStatu sta;
        public tStatu Status
        {
            get
            {
                if (sta == null)
                {
                    sta = new tStatu();
                }
                return sta;
            }
            set => sta = value;
        }

        public string pName { get { return this.Product.pName; } set { Product.pName = value; } }
        public string pCategory
        {
            get { return this.Product.tProductCategory.Category; }
            set { Product.tProductCategory.Category = value; }
        }
        public string pMaterial
        {
            get { return this.Product.tProductMaterial.Material; }
            set { Product.tProductMaterial.Material = value; }
        }
        public string pSize
        {
            get { return this.Product.tProductSize.Size; }
            set { Product.tProductSize.Size = value; }
        }
        public string pLxWxH { get { return this.Product.pLxWxH; } set { Product.pLxWxH = value; } }
        public double pWeight { get { return this.Product.pWeight; } set { Product.pWeight = value; } }
        public decimal pPrice { get { return this.Product.pPrice; } set { Product.pPrice = value; } }

        public int sProductQty { get { return this.Status.sProductQty; }set { Status.sProductQty = value; } }
    }
}