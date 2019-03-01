using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;

namespace UGRS.Core.SDK.DI.Machinery.Tables
{
    [Table(Name = "UG_GLO_AUTO", Description = "GLO Autorizaciones", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Authorizations : Table
    {
        [Field(Description = "Object type de la pantalla", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string ObjType { get; set; }

        [Field(Description = "Funcionalidad", Type = BoFieldTypes.db_Numeric)]
        public int Function { get; set; }

        [Field(Description = "Id del usuario", Type = BoFieldTypes.db_Numeric)]
        public int UserId { get; set; }
    }
}
