using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class CustomerViewModel
    {      
            private tCustomer customer;
            public tCustomer Customer
            {
                get
                {
                    if (customer == null)
                    {
                        customer = new tCustomer();
                    }
                    return customer;
                }
                set => customer = value;
            }
        [DisplayName("客戶編號")]
        public int CustomerId { get { return this.Customer.CustomerId; } set { Customer.CustomerId = value; } }
        [DisplayName("客戶帳號")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cAccount { get { return this.Customer.cAccount; } set { Customer.cAccount = value; } }
        [DisplayName("客戶密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cPassword { get { return this.Customer.cPassword; } set { Customer.cPassword = value; } }
        [DisplayName("客戶名字")]
        public string cName { get { return this.Customer.cName; } set { Customer.cName = value; } }
        [DisplayName("客戶市話")]
        public string cTelePhone { get { return this.Customer.cTelePhone; } set { Customer.cTelePhone = value; } }
        [DisplayName("客戶手機")]
        //[RegularExpression(@"/^09\d{8}$/", ErrorMessage = "不符手機格式")]
        public string cCellPhone { get { return this.Customer.cCellPhone; } set { Customer.cCellPhone = value; } }
        [DisplayName("客戶地址")]
        public string cAddress { get { return this.Customer.cAddress; } set { Customer.cAddress = value; } }
        [DisplayName("客戶統一編號")]
        public string cGUI { get { return this.Customer.cGUI; } set { Customer.cGUI = value; } }
        [DisplayName("客戶Email")]
        [EmailAddress]
        public string cEmail { get { return this.Customer.cEmail; } set { Customer.cEmail = value; } }
    }

    public class RegisterViewModel
    {        
        [DisplayName("帳號")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cAccount { get; set; }
        [DisplayName("密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cPassword { get; set; }
        [DisplayName("確認密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cConfirmPassword { get; set; }
        [DisplayName("名字")]
        public string cName { get; set; }
        [DisplayName("市話")]
        public string cTelePhone { get; set; }
        [DisplayName("手機")]
        public string cCellPhone { get; set; }
        [DisplayName("地址")]
        public string cAddress { get; set; }
        [DisplayName("統一編號")]
        public string cGUI { get; set; }
        [DisplayName("Email")]
        [EmailAddress]
        public string cEmail { get; set; }
    }

    public class CustomerLoginViewModel
    {
        [DisplayName("客戶帳號")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cAccount { get; set; }
        [DisplayName("客戶密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
    }

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
                if (favor == null)
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
        public int FavoriteId { get { return this.Favor.FavoriteId; } set { Favor.FavoriteId = value; } }
        [DisplayName("客戶ID")]
        public int fCustomerId { get { return this.Favor.fCustomerId; } set { Favor.fCustomerId = value; } }
        [DisplayName("產品ID")]
        public int fProductId { get { return this.Favor.fProductId; } set { Favor.fProductId = value; } }

        [DisplayName("產品名稱")]
        public string pName { get { return this.Product.pName; } set { Product.pName = value; } }
        [DisplayName("產品單價")]
        public decimal pPrice { get { return this.Product.pPrice; } set { Product.pPrice = value; } }
    }

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