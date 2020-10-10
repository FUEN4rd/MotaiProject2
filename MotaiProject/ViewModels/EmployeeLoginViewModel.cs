using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class EmployeeLoginViewModel
    {
        [DisplayName("員工帳號")]
        public int EmployeeId { get; set; }
        [DisplayName("員工密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string ePassword { get; set; }
    }
}