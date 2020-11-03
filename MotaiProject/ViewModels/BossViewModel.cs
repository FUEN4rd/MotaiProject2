using System;
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
}