using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MotaiProject.ViewModels
{
    public class EmployeeViewModels
    {
        private tEmployee employee;
        public tEmployee Employee
        {
            get
            {
                if (employee == null)
                {
                    employee = new tEmployee();
                }
                return employee;
            }
            set => employee = value;
        }
        [DisplayName("員工編號")]
        public int EmployeeId { get { return this.Employee.EmployeeId; } set { Employee.EmployeeId = value; } }
        [DisplayName("員工姓名")]
        public string eName { get { return this.Employee.eName; } set { Employee.eName = value; } }
        [DisplayName("員工職位")]
        public int ePosition { get { return this.Employee.ePosition; } set { Employee.ePosition = value; } }
        [DisplayName("員工部門")]
        public int eBranch { get { return this.Employee.eBranch; } set { Employee.eBranch = value; }}
        [DisplayName("員工密碼")]
        public string ePassword { get { return this.Employee.ePassword; } set { Employee.ePassword = value; } }
    }
}