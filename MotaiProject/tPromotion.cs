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
    
    public partial class tPromotion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tPromotion()
        {
            this.tOrders = new HashSet<tOrder>();
        }
    
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int PromotinoCategory { get; set; }
        public string PromotionDescription { get; set; }
        public System.DateTime pPromotionStartDate { get; set; }
        public System.DateTime pPromotionDeadline { get; set; }
        public string pPromotionWeb { get; set; }
        public string pADimage { get; set; }
        public string pDiscountCode { get; set; }
        public string pDiscount { get; set; }
        public System.DateTime pPromotionPostDate { get; set; }
        public string pCondition { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tOrder> tOrders { get; set; }
        public virtual tPromotionCategory tPromotionCategory { get; set; }
    }
}
