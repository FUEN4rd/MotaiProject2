using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{

    public class NewPromotionViewModel
    {
        [DisplayName("類別")]
        public string sPromotinoCategory { get; set; }
        [DisplayName("標題名稱(原活動名稱)")]
        public string PromotionDescription { get; set; }
        [DisplayName("圖片")]
        public string pADimage { get; set; }
        [DisplayName("網頁連結")]
        public string pPromotionWeb { get; set; }
        [DisplayName("公告日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionPostDate { get; set; }


        [DisplayName("Id")]
        public int PromotionId { get; set; }
    }

    public class Createpromotion
    {
        [DisplayName("類別")]
        public int PromotinoCategory { get; set; }
        [DisplayName("標題名稱(原活動名稱)")]
        public string PromotionDescription { get; set; }
        [DisplayName("圖片")]
        public string pADimage { get; set; }
        [DisplayName("網頁連結")]
        public string pPromotionWeb { get; set; }
        [DisplayName("公告日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionPostDate { get; set; }

        public int PromotionId { get; }
        public string PromotionName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionDeadline { get; set; }
        public string pDiscountCode { get; set; }
        public string pDiscount { get; set; }
        public string pCondition { get; set; }

    }




    public class DetailPromotionViewModel
    {
        [DisplayName("類別")]
        public string sPromotinoCategory { get; set; }
        [DisplayName("活動介紹")]
        public string PromotionDescription { get; set; }
        [DisplayName("圖片")]
        public string pADimage { get; set; }
        [DisplayName("網頁連結")]
        public string pPromotionWeb { get; set; }
        [DisplayName("公告日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionPostDate { get; set; }

        [DisplayName("Id")]
        public int PromotionId { get; set; }
        [DisplayName("名稱")]
        public string PromotionName { get; set; }
        [DisplayName("條件")]
        public string pCondition { get; set; }
        [DisplayName("折扣")]
        public string pDiscount { get; set; }
        [DisplayName("折扣碼")]
        public string pDiscountCode { get; set; }
        [DisplayName("開始日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionStartDate { get; set; }
        [DisplayName("結束日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionDeadline { get; set; }
        
    }

    public class news
    {
        //public PromotionViewModel newProm { get; set; }
        public NewPromotionViewModel newPromotion { get; set; }
        public NewProductViewModel newProduct { get; set; }
        public DetailPromotionViewModel detailPromotion { get; set; }
    }
}