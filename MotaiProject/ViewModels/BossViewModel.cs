﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class BossViewModel
    {
        public favorViewModel favorV { get; set; }
        public buyViewModel buyV{ get; set; }
        
    }

    public class favorViewModel
    {
        public int favorID { get; set; }
        public int faverCount { get; set; }
    }
    public class buyViewModel
    {
        public int buyID { get; set; }
        public int buyCount { get; set; }
    }




    public class empData
    {
        public Dictionary<string,Dictionary<int,int>> tem { get; set; }
    }

    public class wareData
    {
        [DisplayName("店面")]
        public int oWarehouseName { get; set; }
        [DisplayName("店面名")]
        public string oWName { get; set; }
        public int oProductId { get; set; }
        public int oProductQty { get; set; }
        public int pPrice { get; set; }
    }


}