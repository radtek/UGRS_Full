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

        [Field(Description = "Folio Asiento", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string JournalEntryId { get; set; }

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


        [Field(Description = "Nombre Chofer", Type = BoFieldTypes.db_Alpha, Size = 50)]
        public string DriverName { get; set; }

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

       [Field(Description = "Total Kilos", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string TotKG { get; set; }

       [Field(Description = "Importe KM", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
       public double AmountKM { get; set; }

       [Field(Description = "Cabezas", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string Heads { get; set; }

       [Field(Description = "Sacos-Pacas", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string Bags { get; set; }

       [Field(Description = "Varios", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string Varios { get; set; }

        /*--Ruta--*/
       [Field(Description = "Origen", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string Origin { get; set; }

       [Field(Description = "Municipio Origen", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string MOrigin { get; set; }


       [Field(Description = "Destination", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string Destination { get; set; }

       [Field(Description = "Municipio Destination", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string MDestination { get; set; }

       [Field(Description = "KmA", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string KmA { get; set; }

       [Field(Description = "KmB", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string KmB { get; set; }

       [Field(Description = "KmC", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string KmC { get; set; }

       [Field(Description = "KmD", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string KmD { get; set; }

       [Field(Description = "KmE", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string KmE { get; set; }

       [Field(Description = "KmF", Type = BoFieldTypes.db_Alpha, Size = 30)]
       public string KmF { get; set; }

    }
}
