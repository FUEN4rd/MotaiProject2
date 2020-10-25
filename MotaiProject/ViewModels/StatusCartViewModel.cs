using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public int StatusId { get { return this.Status.StatusId; } set { Status.StatusId = value; } }
        public int ProductId { get { return this.Product.ProductId; } set { Product.ProductId = value; } }
        [DisplayName("產品名稱")]
        public string pName { get { return this.Product.pName; } set { Product.pName = value; } }
        [DisplayName("產品單價")]
        public decimal pPrice { get { return this.Product.pPrice; } set { Product.pPrice = value; } }
        [DisplayName("購買數量")]
        public int sProductQty { get; set; }
        [DisplayName("小計")]
        public decimal pTotal{get{ return pPrice * sProductQty;}}

    }
}