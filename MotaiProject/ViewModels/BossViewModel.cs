using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class BossViewModel
    {
        [DisplayName("收藏數據")]
        public Dictionary<int,int> favorOrder { get; set; }
        [DisplayName("購買數據")]
        public Dictionary<int, int>  buyOrder { get; set; }
    }
}