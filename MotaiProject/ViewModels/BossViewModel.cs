using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class BossViewModel
    {
        public List<favorViewModel> favorV { get; set; }
        public List<buyViewModel> buyV{ get; set; }
        
    }

    public class favorViewModel
    {
        [DisplayName("產品名稱")]
        public string pName { get; set; }
        public int faverCount { get; set; }
        //public List<string> psImage { get; set; }
        //public List<HttpPostedFileBase> pImage { get; set; }
        public string epsImage
        {
            get; set;
        }
        [DisplayName("分類")]
        public string psCategory { get; set; }
    }
    public class buyViewModel
    {
        [DisplayName("產品名稱")]
        public string pName { get; set; }
        public int buyCount { get; set; }
        //public List<string> psImage { get; set; }
        //public List<HttpPostedFileBase> pImage { get; set; }
        public string epsImage
        {
            get; set;
        }
        [DisplayName("分類")]
        public string psCategory { get; set; }
    }




    public class empData
    {
        public Dictionary<string,Dictionary<int,int>> tem { get; set; }

    }

    public class wareData
    {
        public Dictionary<int, Dictionary<int, int>> waretem { get; set; }
    }
}


