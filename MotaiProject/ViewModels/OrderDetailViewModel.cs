using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class OrderDetailViewModel
    {
        MotaiDataEntities db = new MotaiDataEntities();
        private tOrderDetail det;
        public tOrderDetail Det
        {
            get
            {
                if (det == null)
                {
                    det = new tOrderDetail();
                }
                return det;
            }
            set => det = value;
        }
        [DisplayName("詳細編號")]
        public int oDetailId { get { return this.Det.oOrderDetailId; } set { Det.oOrderDetailId = value; } }
        [DisplayName("訂單編號")]
        public int oOrderId { get { return this.Det.oOrderId; } set { Det.oOrderId = value; } }
        public int oProductId { get { return this.Det.oProductId; } set { Det.oProductId = value; } }
        [DisplayName("產品編號")]
        public string oProductNum { get { return this.Det.tProduct.pNumber; } set { Det.tProduct.pNumber = value; } }        
        [DisplayName("產品名稱")]
        public string oProductName { get { return this.Det.tProduct.pName; } set { Det.tProduct.pName = value; } }
        [DisplayName("產品數量")]
        public int oProductQty { get { return this.Det.oProductQty; } set { Det.oProductQty = value; } }        
        [DisplayName("備註")]
        public string oNote { get { return this.Det.oNote; } set { Det.oNote = value; } }
    }
}