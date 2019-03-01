using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.Enums;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class ConsumablesDocumentsDTO
    {
        public ConsumablesDocumentsDTO()
        {

        }

        /*public int DocEntry { get; set; }
        public int DocNum { get; set; }*/
        public string Code { get; set; }
        public int IdRise { get; set; }
        public string ActivoCode { get; set; }
        public string EcoNum { get; set; }
        /*public string ItemCode { get; set; }
        public string ItemName { get; set; }*/
        public double DieselM { get; set; }
        public double DieselT { get; set; }
        public double Gas { get; set; }
        public double F15W40 { get; set; }
        public double Hidraulic { get; set; }
        public double SAE40 { get; set; }
        public double Transmition { get; set; }
        public double Oils { get; set; }
        public double KmHr { get; set; }
        //public InventoryPropsEnum InventoryPropEnum { get; set; }
        //public double OtherValue { get; set; }

        private int docType;
        public int DocType
        {
            get
            {
                return docType;
            }
            set
            {
                docType = value;
            }
        }

        public string DocTypeDescription
        {
            get
            {
                string typeDescription = string.Empty;
                switch (docType)
                {
                    case 18:
                        typeDescription = "Compras";
                        break;
                    case 1250000001:
                        typeDescription = "Traslado";
                        break;
                    default:
                        typeDescription = "Unk";
                        break;
                }

                return typeDescription;
            }
        }

        private string equipmentType;
        public string EquipmentType
        {
            get
            {
                if (string.IsNullOrEmpty(equipmentType))
                    equipmentType = string.Empty;

                return equipmentType;
            }
            set
            {
                equipmentType = value;
            }
        }
    }
}
