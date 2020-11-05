using MotaiProject.Models;
using MotaiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Controllers
{
    public class BossController : Controller
    {
        // GET: Boss
        public ActionResult Boss登出()
        {
            Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = null;
            return RedirectToAction("員工登入", "Employee");
        }

        public ActionResult Boss首頁()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            else
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                EmployeeViewModels employee = new EmployeeViewModels();
                employee.EmployeeId = emp.EmployeeId;
                employee.eName = emp.eName;
                employee.eAccount = emp.eAccount;
                return View(employee);
            }
        }
        public JsonResult ChangePassword(int EmployeeId, string ePassword, string oldpass)
        {//要用到其他地方
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                MotaiDataEntities dbContext = new MotaiDataEntities();
                if (emp.ePassword == oldpass)
                {
                    emp.ePassword = ePassword;
                    Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] = emp;
                    tEmployee changePwd = dbContext.tEmployees.Where(e => e.EmployeeId.Equals(emp.EmployeeId)).FirstOrDefault();
                    changePwd.ePassword = ePassword;
                    dbContext.SaveChanges();
                    return Json(new { result = true, msg = "更新成功" });
                }
                else
                {
                    return Json(new { result = false, msg = "舊密碼錯誤" });
                }

            }
            else
            {
                return Json(new { result = false, msg = "請先登入" });
            }
        }

        public ActionResult Boss看工作日誌()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {

                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                var dlist = dbContext.tDiaries.ToList();
                List<DiaryViewModel> DSaw = new List<DiaryViewModel>();
                foreach (var item in dlist)
                {
                    DiaryViewModel show = new DiaryViewModel();
                    show.eName = item.tEmployee.eName;
                    show.dDate = item.dDate;
                    show.dWeather = item.dWeather;
                    show.dDiaryNote = item.dDiaryNote;
                    show.dWarehouseNameId = item.dWarehouseNameId;

                    DSaw.Add(show);
                }
                return View(DSaw);
            }
            return RedirectToAction("員工登入", "Employee");
        }
        private ProductRespoitory productRespotiory = new ProductRespoitory();
        public ActionResult Boss產品頁面()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
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
        public ActionResult 銷售數據()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            List<tFavorite> tFavorites = dbContext.tFavorites.ToList();
            List<tOrderDetail> tOrderDetails = dbContext.tOrderDetails.ToList();
            var favorOrder = (from i in tFavorites
                              group i by new {
                                  i.fProductId,
                                  i.tProduct.pName,
                                  i.tProduct.tProductCategory.Category,
                              } into j
                              select new
                              {
                                  Pid = j.Key.fProductId,
                                  Pcount = j.Count(),
                                  Pname = j.Key.pName,
                                  Pcategory=j.Key.Category
                                }).OrderByDescending(j=>j.Pcount).ToList();
            var buyOrder = (from i in tOrderDetails
                            group i by new
                            {
                                i.oProductId,
                                i.tProduct.pName,
                                i.tProduct.tProductCategory.Category,
                            } into j
                            select new
                            {
                                Pid = j.Key.oProductId,
                                Pcount = j.Sum(p => p.oProductQty),
                                Pname = j.Key.pName,
                                Pcategory = j.Key.Category
                            }).OrderByDescending(j => j.Pcount).ToList();
            BossViewModel boss = new BossViewModel();
            List<favorViewModel> favorV = new List<favorViewModel>();
            List<buyViewModel> buyV = new List<buyViewModel>();
            foreach(var item in favorOrder)
            {  
                favorViewModel favor = new favorViewModel();
                tProduct p = new tProduct();
                List<string> pimage = new List<string>();
                p.ProductId = item.Pid;
                pimage = productRespotiory.GetProductShowImages(p);
                if (pimage.Count > 0)
                {
                    favor.epsImage = Url.Content(pimage[0]);
                }
                else
                {
                    favor.epsImage = "";
                }
                favor.faverCount = item.Pcount;
                favor.psCategory = item.Pcategory;
                favor.pName = item.Pname;
                favorV.Add(favor);
            }
            foreach (var item in buyOrder)
            {
                buyViewModel buy = new buyViewModel();
                tProduct p = new tProduct();
                List<string> pimage = new List<string>();
                p.ProductId = item.Pid;
                pimage = productRespotiory.GetProductShowImages(p);
                if (pimage.Count > 0)
                {
                    buy.epsImage = Url.Content(pimage[0]);
                }
                else
                {
                    buy.epsImage = "";
                }            
                buy.buyCount = item.Pcount;
                buy.psCategory = item.Pcategory;
                buy.pName = item.Pname;
                buyV.Add(buy);
            }

            boss.buyV = buyV;
            boss.favorV = favorV;

            return View(boss);

        }

        public ActionResult 銷售報表()
        {//暫時完成
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();
            //var empd = from i in dbContext.tOrders
            //           join j in dbContext.tOrderDetails on i.OrderId equals j.oOrderId
            //           join k in dbContext.tEmployees on i.oEmployeeId equals k.EmployeeId
            //           join m in dbContext.tProducts on j.oProductId equals m.ProductId
            //           group i by new { i.oEmployeeId,i.oDate.Month,
            //               j.oProductQty,m.pPrice,k.eName } into dataE
            //           select new empData 
            //           {
            //               eName=dataE.Key.eName,
            //               tem =new temData{  oDate = dataE.Key.Month,
            //                   Sale =(dataE.Key.oProductQty)*(int)dataE.Key.pPrice,},
            //           };
            //void R(IQueryable<temData> tems)
            //{
            //    List<string> load = new List<string>();
            //    foreach (var item in tems)
            //    {
            //        if (load.Find(x => x.Contains(item.eName)) != null)
            //        {
            //            load.Add(item.eName);
            //            var q = from i in tems
            //                    where i.eName == item.eName
            //                    group i by i.oDate into j
            //                    select j;
            //            empData E = new empData();
            //            E.eName = item.eName;                       
            //        }
            //    }
            //}

            //List<tOrderDetail> tOrderDetails = dbContext.tOrderDetails.ToList();
            //List<empData> empDatas = new List<empData>();
            //foreach(tOrderDetail item in tOrderDetails)
            //{
            //    List<string> load = new List<string>();
            //    if (load.Find(x => x.Contains(item.tOrder.tEmployee.eName)) != null)
            //    {
            //        load.Add(item.tOrder.tEmployee.eName);
            //        empData emp = new empData();
            //        emp.eName = item.tOrder.tEmployee.eName;


            //        emp.tem = new temData
            //        {
            //            Sale = (item.oProductQty) * ((int)item.tProduct.pPrice),
            //            oDate = item.tOrder.oDate.Month,                       
            //            eName = emp.eName
            //        };
            //        empDatas.Add(emp);                   
            //    }
            //}

            List<tOrderDetail> tOrderDetails = dbContext.tOrderDetails.ToList();
            empData emp = new empData();
            emp.tem = new Dictionary<string, Dictionary<int, int>>();
            List<string> load = new List<string>();
            foreach (tOrderDetail item in tOrderDetails)
            {
                
                string ENAME = item.tOrder.tEmployee.eName;
                if (load.Find(x => x.Contains(ENAME)) == null)//抓人
                {
                    load.Add(ENAME);
                    Dictionary<int, int> temD = new Dictionary<int, int>();
                    //int K = item.tOrder.oDate.Month;
                    //int V = (item.oProductQty) * ((int)item.tProduct.pPrice);
                    for(int i = 1; i < 13; i++)
                    {//抓月份
                        var q = from search in tOrderDetails
                                where search.tOrder.oDate.Month == i && search.tOrder.tEmployee.eName == ENAME
                                select (search.oProductQty) * ((int)search.tProduct.pPrice);
                        int V = q.Sum();
                        temD.Add(i, V);
                    }
                    emp.tem.Add(ENAME, temD);
                }
            }
            return View(emp);
        }

        public ActionResult 店鋪報表()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbContext = new MotaiDataEntities();

            List<tOrderDetail> tOrderDetails = dbContext.tOrderDetails.ToList();
            wareData ware = new wareData();
            ware.waretem = new Dictionary<int, Dictionary<int, int>>();
            List<int> load = new List<int>();
            foreach (tOrderDetail item in tOrderDetails)
            {

                int wNAME = item.tOrder.tWarehouseName.WarehouseNameId;
                if (!load.Contains(wNAME))//抓店
                {
                    load.Add(wNAME);
                    Dictionary<int, int> temD = new Dictionary<int, int>();
                    for (int i = 1; i < 13; i++)
                    {//抓月份
                        var q = from search in tOrderDetails
                                where search.tOrder.oDate.Month == i && search.tOrder.tWarehouseName.WarehouseNameId == wNAME
                                select (search.oProductQty) * ((int)search.tProduct.pPrice);
                        int V = q.Sum();//合併月份內營收
                        temD.Add(i, V);
                    }
                    ware.waretem.Add(wNAME, temD);
                }
            }
            return View(ware);
        }

        private EmployeeRespoitory employeeRespoitory = new EmployeeRespoitory();
        public ActionResult 人員檢視()
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入");
            }
            tEmployee empse = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
            var empall = dbContext.tEmployees.OrderBy(c => c.eBranch).ToList();
            List<EmployeeViewModels> employees = new List<EmployeeViewModels>();
            foreach (var item in empall)
            {
                EmployeeViewModels employeeModel = new EmployeeViewModels();
                employeeModel.EmployeeId = item.EmployeeId;
                employeeModel.eAccount = item.eAccount;
                employeeModel.eName = item.eName;
                employeeModel.sPosition = item.tPosition.pPosition;
                employeeModel.sBranch = item.tBranch.bBranch;
                employees.Add(employeeModel);
            }
            return View(employees);
        }
    }
}