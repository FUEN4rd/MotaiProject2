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
        private tEmployee employee;
        public tEmployee Employee
        {
            get
            {
                if (employee == null)
                {
                    employee = new tEmployee();
                }
                return employee;
            }
            set => employee = value;
        }
        [DisplayName("員工編號")]
        public int EmployeeId { get { return this.Employee.EmployeeId; } set { Employee.EmployeeId = value; } }
        [DisplayName("員工帳號")]
        public string eAccount { get { return this.Employee.eAccount; } set { Employee.eAccount = value; } }
        [DisplayName("員工姓名")]
        public string eName { get { return this.Employee.eName; } set { Employee.eName = value; } }
        [DisplayName("員工職位")]
        public int ePosition { get { return this.Employee.ePosition; } set { Employee.ePosition = value; } }
        [DisplayName("員工部門")]
        public int eBranch { get { return this.Employee.eBranch; } set { Employee.eBranch = value; } }
        [DisplayName("員工密碼")]
        public string ePassword { get { return this.Employee.ePassword; } set { Employee.ePassword = value; } }
    }

    public class EmployeeLoginViewModel
    {
        [DisplayName("員工帳號")]
        public string eAccount { get; set; }
        [DisplayName("員工密碼")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "必須有英文大、小寫與數字，長度介於6~12字元")]
        public string ePassword { get; set; }
    }

    public class DiaryViewModel
    {
        private tDiary tdiary;
        public tDiary Diary
        {
            get
            {
                if (tdiary == null)
                {
                    tdiary = new tDiary();
                }
                return tdiary;
            }
            set
            {
                tdiary = value;
            }
        }
        private tEmployee employeegetid;
        public tEmployee employeesetid
        {
            get
            {
                if (employeegetid == null)
                {
                    employeegetid = new tEmployee();
                }
                return employeegetid;
            }
            set
            {
                employeegetid = value;
            }
        }
        public int DiaryId { get { return this.Diary.DiaryId; } set { this.Diary.DiaryId = value; } }
        public int dEmployeeId { get { return this.Diary.dEmployeeId; } set { this.Diary.dEmployeeId = value; } }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime dDate { get { return DateTime.Now; } set { this.Diary.dDate = value; } }
        public string dWeather { get { return this.Diary.dWeather; } set { this.Diary.dWeather = value; } }
        public string dDiaryNote { get { return this.Diary.dDiaryNote; } set { this.Diary.dDiaryNote = value; } }
        public int dWarehouseNameId { get { return this.Diary.dWarehouseNameId; } set { this.Diary.dWarehouseNameId = value; } }
        public string eName { get { return this.employeesetid.eAccount; } set { employeesetid.eAccount = value; } }

        public IEnumerable<SelectListItem> WarehouseName { get; set; }
    }

    public class ProductViewModel
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
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
        public int ProductId { get { return this.Product.ProductId; } set { Product.ProductId = value; } }

        [DisplayName("產品編號")]
        public string pNumber { get { return this.Product.pNumber; } set { Product.pNumber = value; } }

        [DisplayName("產品名稱")]
        public string pName { get { return this.Product.pName; } set { Product.pName = value; } }

        [DisplayName("分類")]
        public string psCategory { get { return this.Product.tProductCategory.Category; } set { Product.tProductCategory.Category = value; } }
        [DisplayName("產品種類")]
        public int pCategory { get { return this.Product.pCategory; } set { Product.pCategory = value; } }

        [DisplayName("材質")]
        public string psMaterial { get { return this.Product.tProductMaterial.Material; } set { Product.tProductMaterial.Material = value; } }
        [DisplayName("產品材質")]
        public int pMaterial { get { return this.Product.pMaterial; } set { Product.pMaterial = value; } }

        [DisplayName("規格")]
        public string psSize { get { return this.Product.tProductSize.Size; } set { Product.tProductSize.Size = value; } }
        [DisplayName("產品大中小")]
        public int pSize { get { return this.Product.pSize; } set { Product.pSize = value; } }

        [DisplayName("產品尺寸")]
        public string pLxWxH { get { return this.Product.pLxWxH; } set { Product.pLxWxH = value; } }

        [DisplayName("產品重量")]
        public double pWeight { get { return this.Product.pWeight; } set { Product.pWeight = value; } }

        [DisplayName("產品介紹")]
        public string pIntroduction { get { return this.Product.pIntroduction; } set { Product.pIntroduction = value; } }

        [DisplayName("產品價格")]
        public decimal pPrice { get { return this.Product.pPrice; } set { Product.pPrice = value; } }

        [DisplayName("產品數量")]
        public int pQty { get; set; }

        private tProductImage prodImage;
        public tProductImage ProdImage
        {
            get
            {
                if (prodImage == null)
                {
                    prodImage = new tProductImage();
                }
                return prodImage;
            }
            set => prodImage = value;
        }

        private List<string> images;
        [DisplayName("產品圖片")]
        public List<string> psImage
        {
            get
            {
                if (images == null)
                {
                    images = new List<string>();
                }
                foreach (var item in this.dbContext.tProductImages.Where(i => i.ProductId == Product.ProductId))
                {
                    images.Add(item.pImage);
                }
                return images;
            }
            set => images = value;
        }

        public List<HttpPostedFileBase> pImage { get; set; }

        public string epsImage
        {
            get
            {
                if (psImage.FirstOrDefault() == null)
                {
                    return "";
                }
                else
                {
                    return psImage[0];
                }
            }
            set => psImage[0] = value;
        }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Materials { get; set; }
        public IEnumerable<SelectListItem> Sizes { get; set; }
    }

    public class EmpPromotionViewModel
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