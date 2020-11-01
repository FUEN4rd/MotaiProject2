using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        [DisplayName("產品編號")]
        public string pNumber { get; set; }

        [DisplayName("產品名稱")]
        public string pName { get; set; }

        [DisplayName("分類")]
        public string psCategory { get; set; }
        [DisplayName("產品種類")]
        public int pCategory { get; set; }

        [DisplayName("材質")]
        public string psMaterial { get; set; }
        [DisplayName("產品材質")]
        public int pMaterial { get; set; }

        [DisplayName("規格")]
        public string psSize { get; set; }
        [DisplayName("產品大中小")]
        public int pSize { get; set; }

        [DisplayName("產品尺寸")]
        public string pLxWxH { get; set; }

        [DisplayName("產品重量")]
        public double pWeight { get; set; }

        [DisplayName("產品介紹")]
        public string pIntroduction { get; set; }

        [DisplayName("產品價格")]
        public decimal pPrice { get; set; }

        [DisplayName("產品數量")]
        public int pQty { get { return 10; } set { } }

        [DisplayName("產品圖片")]
        public List<string> psImage { get; set; }
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

    public class NewProductViewModel
    {
        [DisplayName("產品名稱")]
        public string pName { get; set; }
        [DisplayName("產品介紹")]
        public string pIntroduction { get; set; }
    }

    public class HotProductViewModel
    {

    }

    public class EmpProductViewModel
    {
        public int ProductId { get; set; }
        [DisplayName("產品編號")]
        public string pNumber { get; set; }

        [DisplayName("產品名稱")]
        public string pName { get; set; }

        [DisplayName("分類")]
        public string psCategory { get; set; }
        [DisplayName("產品種類")]
        public int pCategory { get; set; }

        [DisplayName("材質")]
        public string psMaterial { get; set; }
        [DisplayName("產品材質")]
        public int pMaterial { get; set; }

        [DisplayName("規格")]
        public string psSize { get; set; }
        [DisplayName("產品大中小")]
        public int pSize { get; set; }

        [DisplayName("產品尺寸")]
        public string pLxWxH { get; set; }

        [DisplayName("產品重量")]
        public double pWeight { get; set; }

        [DisplayName("產品介紹")]
        public string pIntroduction { get; set; }

        [DisplayName("產品價格")]
        public decimal pPrice { get; set; }
    }


}