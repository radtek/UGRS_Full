using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class CommissionDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int EmployeeId { get; set; }

        public double HrsQty { get; set; }

        public double Rate { get; set; }

        public double Commission { get; set; }

        public double ImportFS { get; set; }

        public double Debit { get; set; }

        public double Total { get; set; }

        public double Pending { get; set; }

        public int RiseId { get; set; }

        public int JournalEntryId { get; set; }

        public string Position { get; set; }

        public string PositionCs { get; set; }
    }
}
