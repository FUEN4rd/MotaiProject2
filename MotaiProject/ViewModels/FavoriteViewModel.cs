using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class FavoriteViewModel
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

        private tFavorite favor;
        public tFavorite Favor
        {
            get
            {
                if(favor == null)
                {
                    favor = new tFavorite();
                }
                return favor;
            }
            set
            {
                favor = value;
            }
        }

        [DisplayName("收藏")]
        public int Favorited { get { return this.Favor.FavoriteId; } set { Favor.FavoriteId = value; } }
        [DisplayName("客戶ID")]
        public int fCustomerId { get { return this.Favor.fCustomerId; } set { Favor.fCustomerId = value; } }
        [DisplayName("產品ID")]
        public int fProductId { get { return this.Favor.fProductId; } set { Favor.fProductId = value; } }
    }
}