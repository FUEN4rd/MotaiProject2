using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class EmployeeViewModels
    {
        [DisplayName("員工編號")]
        public int EmployeeId { get; set; }
        [DisplayName("員工帳號")]
        public string eAccount { get; set; }
        [DisplayName("員工姓名")]
        public string eName { get; set; }
        [DisplayName("員工職位")]
        public int ePosition { get; set; }
        [DisplayName("員工部門")]
        public int eBranch { get; set; }
        [DisplayName("員工密碼")]
        public string ePassword { get; set; }
    }

    public class EmployeeLoginViewModel
    {
        [DisplayName("員工帳號")]
        public string eAccount { get; set; }
        [DisplayName("員工密碼")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string ePassword { get; set; }
    }

    //工作日誌


    //新增日誌
    public class DiaryViewModel
    {
        [DisplayName("員工姓名")]
        public string eName { get; set; }
        public int DiaryId { get; set; }
        public int dEmployeeId { get; set; }
        [DisplayName("日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime dDate { get { return DateTime.Now; } set { } }
        [DisplayName("天氣")]
        public string dWeather { get; set; }
        [DisplayName("備註")]
        public string dDiaryNote { get; set; }
        [DisplayName("倉儲")]
        public int dWarehouseNameId { get; set; }
        public IEnumerable<SelectListItem> WarehouseName { get; set; }

    }


    public class EmpPromotionViewModel
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime pPromotionPostDate { get; set; }

    }
}