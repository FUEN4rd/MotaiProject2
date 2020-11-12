using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class AccountantController : Controller
    {
        // GET: Accountant
        public ActionResult Accountant首頁()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            else
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                EmployeeViewModels employee = new EmployeeViewModels();
                employee.EmployeeId = emp.EmployeeId;
                employee.eName = emp.eName;
                employee.eAccount = emp.eAccount;
                employee.sPosition = emp.tPosition.pPosition;
                return View(employee);
            }
        }

        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult 會計看產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            List<ProductViewModel> productlist = new List<ProductViewModel>();
            productlist = productRespotiory.GetProductAll();
            foreach (var items in productlist)
            {
                if (items.psImage.Count > 0)
                {
                    items.epsImage = Url.Content(items.psImage[0]);
                }
                else
                {
                    items.epsImage = "";
                }
            }
            return View(productlist);
        }

        private OrderRespoitory orderRespoitory = new OrderRespoitory();
        public ActionResult 會計查詢()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            List<OrderViewModel> orderlist = new List<OrderViewModel>();
            orderlist = orderRespoitory.GetOrderAll();
            //foreach (var bag in orderlist)
            //{
            //    ViewData["bag.OrderId"] = "bag.surplus";
            //}
            return View(orderlist);
        }
        public ActionResult 會計審核(int Id)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            //OrderViewModel orderlist = new OrderViewModel(); orderlist = orderRespoitory.GetOrderbyId(Id);
            var orderlistId = new OrderRespoitory().poGetOrderbyId(Id);
            //List<OrderViewModel> orderlist = new List<OrderViewModel>();
            //orderlist.Add(orderlistId);
            return View(orderlistId);
        }
        [HttpPost]
        public ActionResult 會計審核(OrderViewModel checkOrder)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }  
            MotaiDataEntities dbContext = new MotaiDataEntities();
            tOrder tcheckOrder = dbContext.tOrders.FirstOrDefault(p => p.OrderId == checkOrder.OrderId);
            tcheckOrder.oCheck = checkOrder.oCheck;
            var date = DateTime.Now;
            checkOrder.oCheckDate = date;
            dbContext.SaveChanges();
            return RedirectToAction("會計查詢");
        }

        public JsonResult orderCheck(int Id)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return Json(new { result = false, msg = "請先登入" });
            }
            else
            {
                MotaiDataEntities dbContext = new MotaiDataEntities();
                tOrder order = dbContext.tOrders.Where(o => o.OrderId.Equals(Id)).FirstOrDefault();
                order.oCheck = "checked";
                order.oCheckDate = DateTime.Now;
                dbContext.SaveChanges();
                return Json(new { msg = "審核成功", url = Url.Action("會計查詢", "Accountant") });
            }
        }


        private PromotionRespoitory promotionRespoitory = new PromotionRespoitory();
        public ActionResult 員工看消息()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            List<DetailPromotionViewModel> promotionlist = new List<DetailPromotionViewModel>();
            promotionlist = promotionRespoitory.GetPromotionAll();
            return View(promotionlist);
        }

    }
}