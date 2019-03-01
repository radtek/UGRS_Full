using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class ConsumablesDTO
    {
        private string code;
        public string Code 
        {
            get
            {
                if(string.IsNullOrEmpty(code))
                {
                    code = string.Empty;
                }

                return code;
            }
            set
            {
                code = value;
            }
        }

        public int IdRise { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public int TransferFolio { get; set; }
        public int TransferFolioDocNum { get; set; }
    }
}
