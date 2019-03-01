/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfers Items Data Table Object
Date: 03/09/2018
Company: Qualisys
*/

namespace UGRS.Core.SDK.DI.FoodTransfer.DTO {
    /// <summary>
    /// Components of Porduction Items
    /// </summary>
    public class Component {

        public int LineNum { get; set; }
        public string Item { get; set; }
        public string Desc { get; set; }
        public string Whs { get; set; }
        public double Exist { get; set; }
        public double Plan { get; set; }
        public double Qty { get; set; }
        public double Bags { get; set; }
        public string AccCode { get; set; }
        public int Prod { get; set; }
        public int BagsRequired { get; set; }
        public string Method { get; set; }
        public double Consumed { get; set; }
        public int Resource { get; set; }
        public int Inventorial { get; set; }
        public double LineTotal { get; set; }
    }
}
