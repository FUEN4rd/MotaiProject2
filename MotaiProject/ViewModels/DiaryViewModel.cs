using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class DiaryViewModel
    {
        private tDiary tdiary;
        public tDiary Diary
        {
            get
            {
                if(tdiary == null)
                {
                    tdiary = new tDiary();
                }
                return tdiary;
            }
            set
            {
                tdiary = value;
            }
        }
        private tEmployee employeegetid;
        public tEmployee employeesetid
        {
            get
            {
                if (employeegetid == null)
                {
                    employeegetid = new tEmployee();
                }
                return employeegetid;
            }
            set
            {
                employeegetid = value;
            }
        }
        public int DiaryId { get { return this.Diary.DiaryId; } set { this.Diary.DiaryId = value; } }
        public int dEmployeeId { get { return this.Diary.dEmployeeId; } set { this.Diary.dEmployeeId = value; } }
        public DateTime dDate { get { return this.Diary.dDate; } set { this.Diary.dDate = value; } }
        public string dWeather { get { return this.Diary.dWeather; } set { this.Diary.dWeather = value; } }
        public string dDiaryNote { get { return this.Diary.dDiaryNote; } set { this.Diary.dDiaryNote = value; } }
        public int dWarehouseNameId { get { return this.Diary.dWarehouseNameId; } set { this.Diary.dWarehouseNameId = value; } }
        public string eName { get { return this.employeesetid.eAccount; } set { employeesetid.eAccount = value; } }
        public IEnumerable<SelectListItem> WarehouseName { get; set; }
    }
    


}