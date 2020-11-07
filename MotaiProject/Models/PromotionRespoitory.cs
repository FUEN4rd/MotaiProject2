using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Models
{
    public class PromotionRespoitory
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();

        public List<DetailPromotionViewModel> GetPromotionAll()
        {
            List<tPromotion> promo = dbContext.tPromotions.ToList();
            List<DetailPromotionViewModel> promotionlist = new List<DetailPromotionViewModel>();
            foreach (tPromotion item in promo)
            {
                DetailPromotionViewModel Promo = new DetailPromotionViewModel();
                Promo.pADimage = item.pADimage;
                Promo.pCondition = item.pCondition;
                Promo.pDiscount = item.pDiscount;
                Promo.pPromotionDeadline = item.pPromotionDeadline;
                Promo.pPromotionPostDate = item.pPromotionPostDate;
                Promo.pPromotionStartDate = item.pPromotionStartDate;
                Promo.pPromotionWeb = item.pPromotionWeb;
                Promo.sPromotinoCategory = item.tPromotionCategory.PromtionCategory;
                Promo.PromotionName = item.PromotionName;
                Promo.pDiscountCode = item.pDiscountCode;
                Promo.PromotionId = item.PromotionId;
                if (item.PromotionDescription.Length > 10)
                {
                    Promo.PromotionDescription = item.PromotionDescription.Substring(0, 7) + "...";
                }
                else
                {
                    Promo.PromotionDescription = item.PromotionDescription;
                }
                promotionlist.Add(Promo);
            }
            return promotionlist;
        }

        public DetailPromotionViewModel GetPromotionById(int PromotionId)
        {
            tPromotion promotion = dbContext.tPromotions.FirstOrDefault(p => p.PromotionId == PromotionId);
            DetailPromotionViewModel Promo = new DetailPromotionViewModel();
            Promo.pADimage = promotion.pADimage;
            Promo.pCondition = promotion.pCondition;
            Promo.pDiscount = promotion.pDiscount;
            Promo.pPromotionDeadline = promotion.pPromotionDeadline;
            Promo.pPromotionPostDate = promotion.pPromotionPostDate;
            Promo.pPromotionStartDate = promotion.pPromotionStartDate;
            Promo.pPromotionWeb = promotion.pPromotionWeb;
            Promo.PromotionDescription = promotion.PromotionDescription;
            Promo.sPromotinoCategory = promotion.tPromotionCategory.PromtionCategory;
            Promo.PromotionName = promotion.PromotionName;
            Promo.pDiscountCode = promotion.pDiscountCode;
            Promo.PromotionId = promotion.PromotionId;
            return Promo;
        }

        public Dictionary<int, string> GetPromoCategoryAll()
        {
            var categories = dbContext.tPromotionCategories.OrderBy(c => c.PromtionCategory);
            return categories.ToDictionary(cid => cid.PromotionCategoryID, cn => cn.PromtionCategory);
        }

    }
}