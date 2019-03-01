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
    [Table(Name = "UG_TR_RODS", Description = "TR Lista de rutas", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Routes : Table
    {
        [Field(Description = "TypeA", Type = BoFieldTypes.db_Numeric)]
        public float TypeA { get; set; }

        [Field(Description = "TypeB", Type = BoFieldTypes.db_Numeric)]
        public float TypeB { get; set; }

        [Field(Description = "TypeC", Type = BoFieldTypes.db_Numeric)]
        public float TypeC { get; set; }

        [Field(Description = "TypeD", Type = BoFieldTypes.db_Numeric)]
        public float TypeD { get; set; }

        [Field(Description = "TypeE", Type = BoFieldTypes.db_Numeric)]
        public float TypeE { get; set; }

        [Field(Description = "TypeF", Type = BoFieldTypes.db_Numeric)]
        public float TypeF { get; set; }

        [Field(Description = "Destino", Type = BoFieldTypes.db_Alpha, Size = 400)]
        public string Destino { get; set; }

        [Field(Description = "Origen", Type = BoFieldTypes.db_Alpha, Size = 400)]
        public string Origen { get; set; }

        [Field(Description = "Municipio origen", Type = BoFieldTypes.db_Alpha, Size = 100)]
        public string TR_TOWNORIG { get; set; }

        [Field(Description = "Municipio destino", Type = BoFieldTypes.db_Alpha, Size = 100)]
        public string TR_TOWNDES { get; set; }

        [Field(Description = "Activo", Size = 1)]
        public string Activo { get; set; }

        [Field(Description = "CasetaTC", Type = BoFieldTypes.db_Numeric)]
        public float CasetaTC { get; set; }

        [Field(Description = "CasetaRB", Type = BoFieldTypes.db_Numeric)]
        public float CasetaRB { get; set; }

    }
}
