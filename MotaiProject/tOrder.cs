//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MotaiProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class tOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tOrder()
        {
            this.tOrderDetails = new HashSet<tOrderDetail>();
            this.tOrderPays = new HashSet<tOrderPay>();
        }
    
        public int OrderId { get; set; }
        public int oCustomerId { get; set; }
        public string oAddress { get; set; }
        public string oDate { get; set; }
        public string oDeliverDate { get; set; }
        public Nullable<int> oEmployeeId { get; set; }
        public string oCheck { get; set; }
        public string oCheckDate { get; set; }
        public string oCheckDate { get; set; }
        public Nullable<int> oPromotionId { get; set; }
    
        public virtual tCustomer tCustomer { get; set; }
        public virtual tEmployee tEmployee { get; set; }
        public virtual tPromotion tPromotion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tOrderDetail> tOrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tOrderPay> tOrderPays { get; set; }
    }
}
