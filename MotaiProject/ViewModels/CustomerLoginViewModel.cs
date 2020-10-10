using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class CustomerLoginViewModel
    {
        [DisplayName("客戶帳號")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cAccount { get; set; }
        [DisplayName("客戶密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string cPassword { get; set; }
    }
}