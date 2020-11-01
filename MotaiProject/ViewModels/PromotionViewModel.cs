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
    }

    public class news
    {
        //public PromotionViewModel newProm { get; set; }
        public NewPromotionViewModel newPrmotion { get; set; }
        public NewProductViewModel newProduct { get; set; }
    }
}