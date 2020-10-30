using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.Models
{
    public class EmployeeRespoitory
    {
        MotaiDataEntities dbContext = new MotaiDataEntities();
        //Employee
        public Dictionary<int, string> GetEmployeeAll()
        {
            var empNames = dbContext.tEmployees.OrderBy(e => e.eName);
            return empNames.ToDictionary(eid => eid.EmployeeId, en => en.eName);
        }

        public List<SelectListItem> GetSelectList(Dictionary<int, string> dictionary)
        {
            List<SelectListItem> selectLists = new List<SelectListItem>();
            foreach (var items in dictionary)
            {
                selectLists.Add(new SelectListItem()
                {
                    Text = items.Value,
                    Value = items.Key.ToString()
                });
            }
            return selectLists;
        }
    }
}