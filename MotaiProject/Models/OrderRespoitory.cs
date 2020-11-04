using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;

namespace MotaiProject.Models
{
    public class OrderRespoitory
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
        public int SelectPromotionId(int money,DateTime date)
        {
            List<tPromotion> promotions = dbContext.tPromotions.OrderByDescending(p => p.pCondition).Where(p=>p.pPromotionDeadline>date&&p.pPromotionStartDate<date).ToList();
            foreach(var item in promotions)
            {
                if (Convert.ToInt32(item.pCondition) <= money)
                {
                    return item.PromotionId;
                }
            }
            int promotionId = 0;
            return promotionId;
        }

        
    }
}