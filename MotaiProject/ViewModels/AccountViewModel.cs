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
        [DisplayName("客戶編號")]
        public int CustomerId { get; set; }
        [DisplayName("客戶帳號")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cAccount { get; set; }
        [DisplayName("客戶密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cPassword { get; set; }
        [DisplayName("客戶名字")]
        public string cName { get; set; }
        [DisplayName("客戶市話")]
        public string cTelePhone { get; set; }
        [DisplayName("客戶手機")]
        //[RegularExpression(@"/^09\d{8}$/", ErrorMessage = "不符手機格式")]
        public string cCellPhone { get; set; }
        [DisplayName("客戶地址")]
        public string cAddress { get; set; }
        [DisplayName("客戶統一編號")]
        public string cGUI { get; set; }
        [DisplayName("客戶Email")]
        [EmailAddress]
        public string cEmail { get; set; }
    }
    public class MemberViewModel
    {        
        [DisplayName("帳號")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cAccount { get; set; }
        [DisplayName("密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cPassword { get; set; }
        [DisplayName("名字")]
        public string cName { get; set; }
        [DisplayName("市話")]
        public string cTelePhone { get; set; }
        [DisplayName("手機")]
        //[RegularExpression(@"/^09\d{8}$/", ErrorMessage = "不符手機格式")]
        public string cCellPhone { get; set; }
        [DisplayName("地址")]
        public string cAddress { get; set; }
        [DisplayName("統一編號")]
        public string cGUI { get; set; }
        [DisplayName("Email")]
        [EmailAddress]
        public string cEmail { get; set; }
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

}