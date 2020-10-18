using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MotaiProject.ViewModels
{

    public class PromotionViewModel
    {
        private tPromotion prom;
        public tPromotion Prom
        {
            get
            {
                if (prom == null)
                {
                    prom = new tPromotion();
                }
                return prom;
            }
            set => prom = value;
        }
        [DisplayName("編號")]
        public int PromotionId { get { return this.Prom.PromotionId; } set { Prom.PromotionId = value; } }
        [DisplayName("名稱")]
        public string PromotionName { get { return this.Prom.PromotionName; } set { Prom.PromotionName = value; } }
        [DisplayName("開始時間")]
        public DateTime pPromotionStartDate { get { return this.Prom.pPromotionStartDate; } set { Prom.pPromotionStartDate = value; } }
        [DisplayName("結束時間")]
        public DateTime pPromotionDeadline { get { return this.Prom.pPromotionDeadline; } set { Prom.pPromotionDeadline = value; } }
        [DisplayName("網頁連結")]
        public string pPromotionWeb { get { return this.Prom.pPromotionWeb; } set { Prom.pPromotionWeb = value; } }
        [DisplayName("圖片")]
        public string pADimage { get { return this.Prom.pADimage; } set { Prom.pADimage = value; } }
        [DisplayName("優惠碼")]
        public string pDiscountCode { get { return this.Prom.pDiscountCode; } set { Prom.pDiscountCode = value; } }
        [DisplayName("折扣")]
        public double pDiscount { get { return this.Prom.pDiscount; } set { Prom.pDiscount = value; } }

    }
    
}