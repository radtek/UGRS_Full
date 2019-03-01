using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Extension.Enum;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class RiseDTO
    {
        public int Code { get; set; }
        public int IdRise { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Client { get; set; }
        public string ClientName { get; set; }
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public int FolioRelation { get; set; }
        public int UserId { get; set; }

        private RiseStatusEnum docStatus;
        public RiseStatusEnum DocStatus { get; set; }

        public string Status
        {
            get
            {
                return docStatus.GetDescription();
            }
        }
    }
}
