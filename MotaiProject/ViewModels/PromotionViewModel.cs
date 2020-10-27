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
        [DisplayName("類別")]
        public string sPromotinoCategory { get { return this.Prom.tPromotionCategory.PromtionCategory; } set { Prom.tPromotionCategory.PromtionCategory = value; } }
        public int PromotinoCategory { get { return this.Prom.PromotinoCategory; } set { Prom.PromotinoCategory = value; } }
        [DisplayName("名稱")]
        public string PromotionName { get { return this.Prom.PromotionName; } set { Prom.PromotionName = value; } }
        [DisplayName("活動內容")]
        public string PromotionDescription { get { return this.Prom.PromotionDescription; } set { Prom.PromotionDescription = value; } }
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
        [DisplayName("公告日期")]
        public System.DateTime pPromotionPostDate { get { return this.Prom.pPromotionPostDate; } set { Prom.pPromotionPostDate = value; } }

    }
    
}