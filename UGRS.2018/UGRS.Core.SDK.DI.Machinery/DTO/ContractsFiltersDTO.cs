using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class ContractsFiltersDTO
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public double HrsFeet { get; set; }
        public int ExtrasInvoices { get; set; }
        public double Import { get; set; }
        public double RealHrs { get; set; }
        public double Difference { get; set; }
        public int Status { get; set; }

        public string StatusDescription
        {
            get
            {
                if (Status <= 0)
                {
                    return "Abierto";
                }
                else
                {
                    return "Cerrado";
                }
            }
        }
    }
}
