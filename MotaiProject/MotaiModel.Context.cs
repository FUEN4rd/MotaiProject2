﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MotaiDataEntities : DbContext
    {
        public MotaiDataEntities()
            : base("name=MotaiDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tBranch> tBranches { get; set; }
        public virtual DbSet<tCustomer> tCustomers { get; set; }
        public virtual DbSet<tDiary> tDiaries { get; set; }
        public virtual DbSet<tEmployee> tEmployees { get; set; }
        public virtual DbSet<tFavorite> tFavorites { get; set; }
        public virtual DbSet<tOrder> tOrders { get; set; }
        public virtual DbSet<tOrderDetail> tOrderDetails { get; set; }
        public virtual DbSet<tOrderPay> tOrderPays { get; set; }
        public virtual DbSet<tOrderPayType> tOrderPayTypes { get; set; }
        public virtual DbSet<tPosition> tPositions { get; set; }
        public virtual DbSet<tProduct> tProducts { get; set; }
        public virtual DbSet<tProductCategory> tProductCategories { get; set; }
        public virtual DbSet<tProductImage> tProductImages { get; set; }
        public virtual DbSet<tProductMaterial> tProductMaterials { get; set; }
        public virtual DbSet<tProductSize> tProductSizes { get; set; }
        public virtual DbSet<tPromotion> tPromotions { get; set; }
        public virtual DbSet<tPromotionCategory> tPromotionCategories { get; set; }
        public virtual DbSet<tShipDetail> tShipDetails { get; set; }
        public virtual DbSet<tShipList> tShipLists { get; set; }
        public virtual DbSet<tStatu> tStatus { get; set; }
        public virtual DbSet<tStockDetail> tStockDetails { get; set; }
        public virtual DbSet<tStockList> tStockLists { get; set; }
        public virtual DbSet<tTransfer> tTransfers { get; set; }
        public virtual DbSet<tWarehouse> tWarehouses { get; set; }
        public virtual DbSet<tWarehouseName> tWarehouseNames { get; set; }
    }
}
