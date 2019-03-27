using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;


namespace UGRS.Core.SDK.DI.Transports.Tables
{
    [Table(Name = "UG_TR_INTLFRGHT", Description = "TR Fletes internos", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class InternalFreight : Table
    {
        [Field(Description = "Folio Interno", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string InternalFolio { get; set; }

        [Field(Description = "Compartido", Size = 1)]
        public bool Shared { get; set; }

        [Field(Description = "Papeleta", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string Ticket { get; set; }

        [Field(Description = "Tipo de vehiculo", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string VehicleType { get; set; }

        [Field(Description = "Numero Economico", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string Asset { get; set; }

        [Field(Description = "Tipo de carga", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string PayloadType { get; set; }

        [Field(Description = "Chofer", Type = BoFieldTypes.db_Alpha, Size = 50)]
        public string Driver { get; set; }

       [Field(Description = "Cargos extra", Type = BoFieldTypes.db_Numeric)]
        public float Extra { get; set; }

       [Field(Description = "Area", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string Area { get; set; }

       [Field(Description = "Comentarios", Type = BoFieldTypes.db_Alpha, Size = 150)]
       public string Comments { get; set; }

       [Field(Description = "Ruta", Type = BoFieldTypes.db_Numeric)]
       public int Route { get; set; }

       [Field(Description = "Estatus", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string Status { get; set; }

       [Field(Description = "Seguro", Size = 1)]
       public bool Insurance { get; set; }

    }
}
