﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class ProductViewModel
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
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

        [DisplayName("產品編號")]
        public string pNumber { get { return this.Product.pNumber; } set { Product.pNumber = value; } }

        [DisplayName("產品名稱")]
        public string pName { get { return this.Product.pName; } set { Product.pName = value; } }

        [DisplayName("產品種類")]
        public int pCategory { get { return this.Product.pCategory; } set { Product.pCategory = value; } }

        [DisplayName("產品材質")]
        public int pMaterial { get; set; }

        [DisplayName("產品大中小")]
        public int pSize { get; set; }

        [DisplayName("產品尺寸")]
        public string pLxWxH { get { return this.Product.pLxWxH; } set { Product.pLxWxH = value; } }

        [DisplayName("產品重量")]
        public double pWeight { get { return this.Product.pWeight; } set { Product.pWeight = value; } }

        [DisplayName("產品價格")]
        public decimal pPrice { get { return this.Product.pPrice; } set { Product.pPrice = value; } }

        [DisplayName("產品數量")]
        public int pQty { get; set; }

        public HttpPostedFileBase[] pImage { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Materials { get; set; }
        public IEnumerable<SelectListItem> Sizes { get; set; }

        //private List<string> image;

        //public List<string> pImgae
        //{
        //    get
        //    {
        //        if (image == null)
        //        {
        //            image = new List<string>();
        //        }
        //        foreach(var items in this.dbContext.tProductImages.Where(i => i.ProductId == Product.ProductId))
        //        {
        //            image.Add(items.pImage);
        //        }
        //        return image;
        //    }
        //    set => image = value;           
        //}
    }
}