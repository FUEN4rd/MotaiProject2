using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class AddToCartViewModel
    {        
            public int StatusId { get; set; }
            public int sCustomerId { get; set; }
            public int sProductId { get; set; }
            public int sProductQty { get; set; }        
    }
}