using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class CommissionDetailsDTO
    {
        public int IdRise { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public double Hours { get; set; }
        public double Rate { get; set; }
        public double Commission { get; set; }
        public double ImportFS { get; set; }
        public double Adeudo { get; set; }
        public double Total { get; set; }
        public double Pendiente { get; set; }
        public string CodeMov { get; set; }
        public string IsSupervisor { get; set; }

        public string Position { get; set; }

        public string PositionDesc 
        {
            get
            {
                return Position == "S" ? "Supervisor" : "Operador";
            }
        }
    }
}
