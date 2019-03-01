using System.Collections.Generic;

namespace UGRS.Core.SDK.DI.Purchases.DTO
{
    public class ConceptsXMLDTO
    {
        
            public string Description { get; set; }
            public string AccountNumber { get; set; }
            public string ClassificationCode { get; set; }
            public string Quantity { get; set; }
            public string UnitPrice { get; set; }
            public string Amount { get; set; }
            public string Subtotal { get; set; }
            public string UnitType { get; set; }
            public string Unit { get; set; }
            public string CodeItmProd { get; set; }
            public string NoIdentification { get; set; }
            public string CostingCode { get; set; }
            public List<TaxesXMLDTO> LstTaxes { get; set; }
            public List<TaxesXMLDTO> LstWithholdingTax { get; set; }
            public string AdmOper { get; set; }
            public string TaxCode { get; set; }
            public string TaxRate { get; set; }
            public string WareHouse { get; set; }
            public string Account { get; set; }
            public bool HasTax { get; set; }
            public bool HasWht { get; set; }
            public string Discount { get; set; }
            public string AF { get; set; }
            public string Project { get; set; }
            public string AGL { get; set; }
            
    }
}
