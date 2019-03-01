using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class InventoryItemsLinesDTO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double OriginalQty { get; set; }
        public double Quantity { get; set; }
        public string Category { get; set; }

        private string activoFijo;
        public string ActivoFijo 
        {
            get
            {
                if(string.IsNullOrEmpty(activoFijo))
                {
                    activoFijo = string.Empty;
                }

                return activoFijo;
            }
            set
            {
                activoFijo = value;
            }
        }
    }
}
