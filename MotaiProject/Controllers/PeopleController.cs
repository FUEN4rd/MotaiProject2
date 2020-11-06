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
    public class PeopleController : Controller
    {
        // GET: People
        public ActionResult People首頁()
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


        private EmployeeRespoitory employeeRespoitory = new EmployeeRespoitory();
        public ActionResult 人員檢視()
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            tEmployee empse = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
            var empall = dbContext.tEmployees.OrderBy(c => c.eBranch).ToList();
            List<EmployeeViewModels> employees = new List<EmployeeViewModels>();
            foreach(var item in empall)
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
   
        public ActionResult 修改員工(int id)
        {
            if (CSession關鍵字.SK_LOGINED_EMPLOYEE==null)
            {
                return RedirectToAction("員工登入", "Employee");
            }
            MotaiDataEntities dbcontext = new MotaiDataEntities();
            tEmployee empse = dbcontext.tEmployees.FirstOrDefault(c=>c.EmployeeId==id);
            EmployeeViewModels empall = new EmployeeViewModels();
            var vBranch = new ProductRespoitory().GetBranchName();
            List<SelectListItem> Branch = new ProductRespoitory().GetPositionName(vBranch);
            empall.Branch = Branch;
            var vPosition = new ProductRespoitory().GetPositionName();
            List<SelectListItem> Position = new ProductRespoitory().GetPositionName(vPosition);
            empall.Position= Position;
            empall.eName = empse.eName;
            empall.ePosition = empse.ePosition;
            empall.EmployeeId = empse.EmployeeId;
            return View(empall);
        }
        [HttpPost]
        public ActionResult 修改員工(EmployeeViewModels employee)
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {
                MotaiDataEntities db = new MotaiDataEntities();
                tEmployee emp = db.tEmployees.Find(employee.EmployeeId);
                if (emp != null)
                {
                    emp.eBranch = employee.eBranch;
                    emp.ePosition = employee.ePosition;
                    emp.eName = employee.eName;
                    db.SaveChanges();
                }
                return RedirectToAction("人員檢視");
            }
            return RedirectToAction("員工登入", "Employee");
        }


        public ActionResult 新增員工()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] == null)
            {
                return RedirectToAction("員工登入", "Employee");
            }

            EmployeeViewModels empall = new EmployeeViewModels();
            var vBranch = new ProductRespoitory().GetBranchName();
            List<SelectListItem> Branch = new ProductRespoitory().GetPositionName(vBranch);
            empall.Branch = Branch;
            var vPosition = new ProductRespoitory().GetPositionName();
            List<SelectListItem> Position = new ProductRespoitory().GetPositionName(vPosition);
            empall.Position = Position;
            return View(empall);
        }
        [HttpPost]
        public ActionResult 新增員工(EmployeeViewModels create員工)
        {
            MotaiDataEntities dbContext = new MotaiDataEntities();
            if (dbContext.tEmployees.Count().Equals(0))
            {
                create員工.EmployeeId = 1;
            }
            

            tEmployee n新員工 = new tEmployee();
            n新員工.eAccount = create員工.eAccount;
            n新員工.EmployeeId = create員工.EmployeeId;
            n新員工.ePassword = create員工.ePassword;
            n新員工.eName = create員工.eName;
            n新員工.ePosition = create員工.ePosition;
            n新員工.eBranch = create員工.eBranch;
            dbContext.tEmployees.Add(n新員工);


            dbContext.SaveChanges();
            return RedirectToAction("人員檢視");
        }
        public ActionResult 工作日誌()
        {
            if (Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] != null)
            {

                MotaiDataEntities dbContext = new MotaiDataEntities();
                tEmployee emp = Session[CSession關鍵字.SK_LOGINED_EMPLOYEE] as tEmployee;
                var dlist = dbContext.tDiaries.OrderBy(c => c.dEmployeeId).ToList();


                List<DiaryViewModel> DSaw = new List<DiaryViewModel>();
                foreach (var item in dlist)
                {
                    tWarehouseName warename = dbContext.tWarehouseNames.Where(w => w.WarehouseNameId.Equals(item.dWarehouseNameId)).FirstOrDefault();
                    DiaryViewModel show = new DiaryViewModel();
                    show.eName = item.tEmployee.eName;
                    show.dDate = item.dDate;
                    show.dWeather = item.dWeather;
                    show.dDiaryNote = item.dDiaryNote;
                    show.dWarehouseNameId = item.dWarehouseNameId;
                    show.dWarehouseName = warename.WarehouseName;
                    DSaw.Add(show);
                }
                return View(DSaw);
            }
            return RedirectToAction("員工登入", "Employee");
        }        

    }
}