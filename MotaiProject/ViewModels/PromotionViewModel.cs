using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class PromotionViewModel
    {        
        [DisplayName("編號")]
        public int PromotionId { get; set; }
        [DisplayName("類別")]
        public string sPromotinoCategory { get; set; }
        public int PromotinoCategory { get; set; }
        [DisplayName("名稱")]
        public string PromotionName { get; set; }
        [DisplayName("活動內容")]
        public string PromotionDescription { get; set; }
        [DisplayName("開始時間")]
        public DateTime pPromotionStartDate { get; set; }
        [DisplayName("結束時間")]
        public DateTime pPromotionDeadline { get; set; }
        [DisplayName("網頁連結")]
        public string pPromotionWeb { get; set; }
        [DisplayName("圖片")]
        public string pADimage { get; set; }
        [DisplayName("優惠碼")]
        public string pDiscountCode { get; set; }
        [DisplayName("折扣")]
        public double pDiscount { get; set; }
        [DisplayName("公告日期")]
        public System.DateTime pPromotionPostDate { get; set; }
    }

    public class NewPromotionViewModel
    {

    }
}