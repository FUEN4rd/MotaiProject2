using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotaiProject.ViewModels
{
    public class ShipListViewModel
    {
        private tShipList ship;
        public tShipList ShipList
        {
            get
            {
                if (ship == null)
                {
                    ship = new tShipList();
                }
                return ship;
            }
            set => ship = value;
        }
        public int ShipId { get { return this.ShipList.ShipId; } set { ShipList.ShipId = value; } }
        public int sEmployeeId { get { return this.ShipList.sEmployeeId; } set { ShipList.sEmployeeId = value; } }
        [DisplayName("員工名")]
        public string EmpName { get { return this.ShipList.tEmployee.eName; } set { ShipList.tEmployee.eName = value; } }
        public int sShipSerialValue { get { return this.ShipList.sShipSerialValue; } set { ShipList.sShipSerialValue = value; } }
        public int sOrderId { get { return this.ShipList.sOrderId; } set { ShipList.sOrderId = value; } }
        public DateTime sShipDate { get { return DateTime.Now; } set { ShipList.sShipDate = value; } }
        public string sShipNote { get { return this.ShipList.sShipNote; } set { ShipList.sShipNote = value; } }

        public IEnumerable<SelectListItem> EmpNames { get; set; }
    }
}