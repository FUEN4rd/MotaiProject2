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